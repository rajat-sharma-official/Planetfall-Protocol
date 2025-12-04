using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class VERAScreenshotManager : MonoBehaviour
{
    [Header("AI Endpoint URL")]
    [SerializeField] private string flaskURL = "http://localhost:8000/predict"; 
    // flask backend endpoint used for ai requests

    public static VERAScreenshotManager Instance { get; private set; }

    private void Awake()
    {
        // ensure only one screenshot manager exists
        if (Instance != null && Instance != this)
        {
            Destroy(this); // avoid duplicates to prevent conflicts
        }
        else
        {
            Instance = this; // set global reference for easy access
        }
    }

    private Texture2D cachedScreenshot; // holds screenshot for repeated sends without recapturing

    public void CaptureScreenshotOnly(Action onCaptured)
    {
        // capture screenshot, store it, and call callback
        StartCoroutine(CaptureOnlyRoutine(onCaptured));
    }

    private IEnumerator CaptureOnlyRoutine(Action onCaptured)
    {
        yield return new WaitForEndOfFrame(); // wait until frame finishes rendering
        // delete previous screenshot to avoid memory buildup
        if (cachedScreenshot) Destroy(cachedScreenshot);
        cachedScreenshot = CaptureScreenshot(); // actually capture
        onCaptured?.Invoke(); // notify caller that screenshot is ready
    }

    private bool isBusy = false; // blocks multiple network requests at once

    public void SendCachedScreenshot(string question, Action<string> onComplete)
    {
        Debug.Log("[VERAScreenshotManager] SendCachedScreenshot called.");

        // prevent overlapping calls — helps avoid spam or freezing
        if (isBusy)
        {
            Debug.LogWarning("[VERAScreenshotManager] System is busy. Ignoring request.");
            onComplete?.Invoke("System is busy. Please wait...");
            return;
        }

        // ensure a screenshot exists
        if (cachedScreenshot == null)
        {
            Debug.LogError("[VERAScreenshotManager] No cached screenshot found!");
            onComplete?.Invoke("No screenshot captured!");
            return;
        }

        // run async send coroutine
        StartCoroutine(SendCachedRoutine(question, onComplete));
    }

    private IEnumerator SendCachedRoutine(string question, Action<string> onComplete)
    {
        isBusy = true; // lock the system while processing
        Debug.Log("[VERAScreenshotManager] Starting SendCachedRoutine...");

        // convert image to png then base64 for transfer
        byte[] png = cachedScreenshot.EncodeToPNG();
        string base64Image = Convert.ToBase64String(png);
        Debug.Log($"[VERAScreenshotManager] Screenshot encoded. Size: {base64Image.Length} chars.");

        AIRequest reqData = new AIRequest
        {
            userText = question, // text the user typed
            image = base64Image  // embedded screenshot
        };
        string jsonBody = JsonUtility.ToJson(reqData); // serialize as clean json

        // send request to backend
        yield return SendToAI(jsonBody, (response) =>
        {
            isBusy = false; // unlock so new requests can run
            onComplete?.Invoke(response); // return final text to caller
        });
    }

    public void CaptureAndAskAI(string question, GameObject uiToHide, Action<string> onComplete)
    {
        // full pipeline with ui hiding handled inside
        StartCoroutine(CaptureRoutine(question, uiToHide, onComplete));
    }

    private IEnumerator CaptureRoutine(string question, GameObject uiToHide, Action<string> onComplete)
    {
        bool wasActive = false; // track original state

        // hide ui temporarily so it doesn't appear in screenshot
        if (uiToHide != null)
        {
            wasActive = uiToHide.activeSelf;
            uiToHide.SetActive(false);
        }

        yield return new WaitForEndOfFrame(); // screenshot must happen after rendering is done

        Texture2D tex = CaptureScreenshot(); // capture visible frame

        // bring back ui if needed
        if (uiToHide != null && wasActive)
        {
            uiToHide.SetActive(true);
        }

        // encode screenshot to base64 string
        byte[] png = tex.EncodeToPNG();
        string base64Image = Convert.ToBase64String(png);
        Destroy(tex); // avoid memory leak

        AIRequest reqData = new AIRequest
        {
            userText = question,
            image = base64Image
        };
        string jsonBody = JsonUtility.ToJson(reqData);

        // send to ai backend
        yield return SendToAI(jsonBody, onComplete);
    }

    private Texture2D CaptureScreenshot()
    {
        // capture screenshot from main camera manually
        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // route camera output to render texture
        Camera.main.targetTexture = rt;
        Camera.main.Render(); // manually render frame
        RenderTexture.active = rt;

        // extract pixels from render texture into texture2d
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply(); // finalize texture in memory

        // clear camera output
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt); // cleanup render texture

        return tex;
    }

    private IEnumerator SendToAI(string jsonBody, Action<string> onComplete)
    {
        // create request for backend
        Debug.Log($"[VERA] Sending request to {flaskURL} (Body: {jsonBody.Length} bytes)");

        UnityWebRequest req = new UnityWebRequest(flaskURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw); // raw json body
        req.downloadHandler = new DownloadHandlerBuffer(); // store text response
        req.SetRequestHeader("Content-Type", "application/json");
        req.timeout = 120; // avoid infinite hanging

        // send async request
        yield return req.SendWebRequest();

        // handle network error
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[VERA] Request Failed: {req.error} | Response: {req.downloadHandler.text}");
            onComplete?.Invoke($"Error: {req.error}\nCheck Console for details.");
        }
        else
        {
            string responseText = req.downloadHandler.text;
            Debug.Log($"[VERA] Response received: {responseText}");

            try
            {
                // parse json reply from backend
                AIResponse resp = JsonUtility.FromJson<AIResponse>(responseText);

                // sanity check for empty replies
                if (string.IsNullOrEmpty(resp.reply))
                {
                    onComplete?.Invoke("AI returned empty reply.");
                }
                else
                {
                    onComplete?.Invoke(resp.reply); // pass clean text out
                }
            }
            catch (Exception e)
            {
                // json parse issues usually mean backend crashed or returned invalid data
                Debug.LogError($"[VERA] JSON Parse Error: {e.Message}");
                onComplete?.Invoke($"Error parsing response: {e.Message}");
            }
        }
    }

    [Serializable]
    public class AIRequest
    {
        public string image;      // base64-encoded screenshot
        public string sceneState; // optional field for future data
        public string userText;   // what player typed
    }

    [Serializable]
    public class AIResponse
    {
        public string reply;     // final ai message
        public string emotion;   // emotion classification
        public string caption;   // screenshot description
    }
}
