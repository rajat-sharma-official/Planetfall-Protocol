# VERA BACKEND API
# unity and python bridge for in-game AI companion

# how to run:
# make sure python is installed
# activate the virtual env:
   # run python3 -m venv venv on MACOS
   # run python -m venv venv on windows

   # run source venv/bin/activate on MACOS
   # run venv\Scripts\activate on windows

   # you may have to adjust execution policies on windows:
   # Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process

# get dependencies after:
# pip3 install -r requirements.txt on MACOS
# pip install -r requirements.txt on windows

# run the server:
# python3 app.py on MACOS
# python app.py on windows

# you should see a message like:
'''
WARNING: This is a development server. Do not use it in a production deployment. Use a production WSGI server instead.
 * Running on all addresses (0.0.0.0)
 * Running on http://xxxx:8000
 * Running on http://xxxx:8000
Press CTRL+C to quit
'''


from flask import Flask, request, jsonify # web api framework to facilitate communication with Unity
from transformers import ( # hf model and processing
    AutoTokenizer,
    pipeline,
    AutoProcessor,
    Qwen2_5_VLForConditionalGeneration,
)
from onnxruntime import InferenceSession # onnx runtime for emotion classification model
import numpy as np # argmax on onnx outputs
import torch # qwen <3 
import base64 # unity screenshot decoding 
from PIL import Image # convert decoded bytes to image
import os # paths
import io # buffer handling

# set the directory of the cache to local models folder if it exists
project_root = os.path.dirname(os.path.abspath(__file__))
models_dir = os.path.join(project_root, "models")
hf_cache_dir = os.path.join(models_dir, "hf_cache")

# if it exists, use it 
if os.path.exists(hf_cache_dir):
    os.environ["HF_HOME"] = hf_cache_dir
    print(f"Using local HF cache at: {hf_cache_dir}")

app = Flask(__name__) # initalize flask

# ==========================================================
# EMOTION CLASSIFICATION (ONNX)
# ==========================================================

emotion_labels = [ # labels matching onnx outputs
    "anger", "disgust", "fear", "joy",
    "neutral", "sadness", "surprise"
]

emotion_tokenizer = AutoTokenizer.from_pretrained( # tokenizer files for emotional model
    "j-hartmann/emotion-english-distilroberta-base"
)

try:
    # LOOK for the model in the local models folder first, ow check models/ folder, and load the onnx. o/w, fallback
    onnx_path = "model.onnx"
    if not os.path.exists(onnx_path):
        possible_path = os.path.join(models_dir, "model.onnx")
        if os.path.exists(possible_path):
            onnx_path = possible_path

    emotion_session = InferenceSession(onnx_path)
except Exception as e:
    print(f"Warning: Could not load emotion model (model.onnx): {e}")
    emotion_session = None

# assume emotion is neutral if the model DNE
def predict_emotion(text: str) -> str:
    if emotion_session is None:
        return "neutral"

# run inferencing 
    inputs = emotion_tokenizer(
        text,
        return_tensors="np",
        padding=True,
        truncation=True,
        max_length=64,
    )

    outputs = emotion_session.run(
        None,
        {
            "input_ids": inputs["input_ids"],
            "attention_mask": inputs["attention_mask"]
        }
    )[0]

    return emotion_labels[np.argmax(outputs)] # tabluate the highest scoring emotion and return. we assume this is the emotion


# ==========================================================
# INTENT CLASSIFIER
# ==========================================================

intent_classifier = pipeline( # zero shot intent classification
    "zero-shot-classification",
    model="facebook/bart-large-mnli"
)

INTENTS = [ # pre-defined intents as testing for demo
    "PLAYER_SMALLTALK",
    "PLAYER_ASK_WHERE_AM_I",
    "PLAYER_ASK_WHAT_IS_THIS",
    "PLAYER_ASK_FOR_HELP",
    "PLAYER_ASK_ABOUT_ENVIRONMENT",
]


def classify_intent(text: str) -> str:
    result = intent_classifier(
        text,
        candidate_labels=INTENTS,
        multi_label=False
        # this can be set to true for multi-label classification if desired, but for now we just want the most likely intent, and could potentially lead to better results
    )
    return result["labels"][0] # get the most likely intent w/o multi-labeling


# ==========================================================
# QWEN2.5-VL (IMAGE + TEXT)
# ==========================================================

qwen_model_id = "Qwen/Qwen2.5-VL-3B-Instruct" # model 

# clamp image resolution so attention doesn't explode
min_pixels = 256 * 28 * 28
max_pixels = 1024 * 28 * 28

try:
    qwen_processor = AutoProcessor.from_pretrained(
        qwen_model_id,
        trust_remote_code=True,
        min_pixels=min_pixels,
        max_pixels=max_pixels,
    )

    qwen_model = Qwen2_5_VLForConditionalGeneration.from_pretrained(
        qwen_model_id,
        torch_dtype=torch.float16,
        device_map="auto", # automatically map to available devices, like the cpu and the gpu
        trust_remote_code=True,
        low_cpu_mem_usage=True,
        offload_folder="./offload" # offloading for systems with low memory
    )
except Exception as e:
    print(f"Warning: Could not load Qwen model: {e}")
    qwen_processor = None
    qwen_model = None # fail fallback, so the server can still run but without qwen capabilities. note that this will impact scene captioning and reply generation.

tone_templates = { # tone dictionary controlled by emotion classifier. can be expanded or modified as needed
    "anger": "Sound calm and level-headed.",
    "disgust": "Stay neutral and detached.",
    "fear": "Speak gently and reassuringly.",
    "joy": "Respond warm and upbeat.",
    "neutral": "Stay clear and matter-of-fact.",
    "sadness": "Sound supportive yet comforting.",
    "surprise": "Keep your tone steady and observant.",
}


def decode_image(base64_string: str): 
    if not base64_string:
        return None # grace
    try: # convert to bytes
        image_bytes = base64.b64decode(base64_string) # load as rgb
        return Image.open(io.BytesIO(image_bytes)).convert("RGB") # convert to RGB
    except Exception: 
        return None # o/w return none if fail and grace


# ==========================================================
# SCENE CAPTIONING
# ==========================================================



def generate_scene_caption(image_b64: str) -> str:
    # uses a qwen 2.5 vl model to generate a short caption describing the scene in the image
    if qwen_model is None or qwen_processor is None: # skip if model not loaded
        return "Scene captioning unavailable (Model not loaded)."

    image = decode_image(image_b64)
    if image is None:
        return "" # no image leads to no caption!

    messages = [
        {
            "role": "user",
            "content": [
                {"type": "image"}, # add the screenshot image
                {"type": "text", "text": "Describe what is happening in this image in one short, clear sentence."} # prompt for captioning
            ]
        }
    ]

    prompt = qwen_processor.apply_chat_template( # convert to model prompt
        messages,
        tokenize=False,
        add_generation_prompt=True
    )

    inputs = qwen_processor( # prepare inputs again
        text=[prompt],
        images=[image],
        return_tensors="pt"
    ).to(qwen_model.device)

    with torch.no_grad(): # generate caption
        output_ids = qwen_model.generate(
            **inputs,
            max_new_tokens=150,
            temperature=0.2,
            top_p=0.8
        )

    #  decode the generated caption tokens
    input_len = inputs["input_ids"].shape[1]
    generated_ids = output_ids[:, input_len:]

    caption = qwen_processor.batch_decode(
        generated_ids,
        skip_special_tokens=True,
        clean_up_tokenization_spaces=True
    )[0].strip()

    # try to clean up the caption to be a single sentence
    if "." in caption:
        caption = caption.split(".")[0].strip() + "."
    else:
        caption = caption.strip() + "."

    return caption


# ==========================================================
# QWEN REPLY GENERATION
# ==========================================================


def generate_qwen_reply(emotion: str, user_text: str, image_b64: str) -> str:
    if qwen_model is None or qwen_processor is None:
        return "I am offline right now. (Model not loaded)"

    # grace
    tone = tone_templates[emotion] if emotion in tone_templates else "Stay clear and matter-of-fact."

    # grace
    image = decode_image(image_b64) if image_b64 else None

    # build the prompt messages to send to qwen model and generate a reply
    # basically our "RAG" without a "RAG" retrieval step, just relying on the image and text input.
    messages = []
    messages.append({
        "role": "system",
        "content": [
            {
                "type": "text",
                "text": (
                        "You are VERA, an in-game AI companion. "
                        "Only describe objects that are clearly visible in the screenshot. "
                        "Pink pill-shaped capsules are friendly NPCs the player can talk to. "
                        "Gray cylinder shapes are scrap metal used for ship repairs and can be collected. "
                        "Purple blocks or cubes are platforms the player can stand on as they explore!"
                        "The green surface is the grass-covered ground. "
                        "Do not guess or invent anything that is not directly visible. "
                        "Speak naturally, as if you are guiding the player. "
                        "Keep your response to one short, friendly sentence under twelve words. "
                        f"Tone: {tone}"
                )
            }
        ]
    })

    # user message with optional image
    # grace
    user_content = []
    if image is not None:
        user_content.append({"type": "image"})
    user_content.append({"type": "text", "text": user_text or "Hello."})

    messages.append({
        "role": "user",
        "content": user_content
    })

    prompt = qwen_processor.apply_chat_template(
        messages,
        tokenize=False,
        add_generation_prompt=True
    )

    if image is not None:
        inputs = qwen_processor(
            text=[prompt],
            images=[image],
            return_tensors="pt"
        ).to(qwen_model.device)
    else:
        inputs = qwen_processor(
            text=[prompt],
            return_tensors="pt"
        ).to(qwen_model.device)

    with torch.no_grad():
        output_ids = qwen_model.generate(
            **inputs,
            max_new_tokens=150,
            temperature=0.4,
            top_p=0.85,
            repetition_penalty=1.2
        )

    input_len = inputs["input_ids"].shape[1]
    generated_ids = output_ids[:, input_len:]

    decoded = qwen_processor.batch_decode(
        generated_ids,
        skip_special_tokens=True,
        clean_up_tokenization_spaces=True
    )[0].strip()

    return decoded


# ==========================================================
# API ENDPOINT
# ==========================================================

# main endpoint for unity to call
@app.route("/predict", methods=["POST"])
def predict():
    data = request.json or {} # fallback to empty for grace

    user_text = (data.get("userText") or "").strip() # user msg
    image_b64 = data.get("image", "") or "" # screenshot base64 string
    scene_state = data.get("sceneState", "") # future use

    if not user_text:
        user_text = "Hello" # fallback text

    emotion = predict_emotion(user_text) # detect emotion
    intent = classify_intent(user_text)

    if intent == "PLAYER_SMALLTALK": # skip captioning for smalltalk
        caption = ""
    else:
        caption = generate_scene_caption(image_b64) if image_b64 else "" # generate scene caption if image provided

    reply = generate_qwen_reply( #get reply from qwen
        emotion,
        user_text,
        image_b64 if intent != "PLAYER_SMALLTALK" else ""
)


    return jsonify({ # return the results as json
        "emotion": emotion,
        "intent": intent,
        "caption": caption,
        "reply": reply,
        "receivedImageBytes": len(image_b64)
    })


if __name__ == "__main__": # run on localhost flask 8000 port
    app.run(host="0.0.0.0", port=8000)
