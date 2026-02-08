# oracle cloud setup
# planetfall protocol

import json
import re
import random
import time
from collections import deque
from typing import Dict, List, Optional
from difflib import SequenceMatcher

import paho.mqtt.client as mqtt

# MQTT stuff
# no touchy
# ============================================
brokerHost = "localhost"
inputTopic = "vera/input"
outputTopic = "vera/output"
# ============================================

# anti repetition logic: prevents the same line from being shown multiple times in a row for the same intent or tags.
# allows for things to feel more varied and less like a broken record, while still being random and fresh on each run.
recentResponsesByKey = {} # creates a key to later deque of recent responses for that key, which can be an intent, tag, or combination
recentWindowSize = 4 # how many recent responses to track for each key before allowing repeats 

followUpProbability = 0.12
glitchProbability = 0.10

smalltalkTriggersByIntent = {
    "HELLO": [
        "hi", "hello", "hey", "yo", "sup", "hiya", "hola",
        "good morning", "good afternoon", "good evening",
        "yo vera", "hey vera", "hi vera", "hello vera", "vera",
        "anyone there", "you there", "are you there", "you here", "are you here",
    ],
    "GOODBYE": [
        "bye", "goodbye", "later", "good night", "night",
        "see you", "see ya", "cya", "ttyl",
        "gotta go", "gtg", "quit", "exit",
    ],
    "THANKS": [
        "thanks", "thank you", "thx", "ty", "appreciate it", "cheers", "gracias", "merci",
    ],
    "HOW_ARE_YOU": [
        "how are you", "how r you", "hru", "how you doing", "you good", "you ok",
    ],
    "WHO_ARE_YOU": [
        "who are you", "what are you", "who is vera", "what is vera", "are you ai", "are you a bot",
    ],
    "CONFUSION": [
        "what", "huh", "confused", "idk", "i dont know", "i don't know",
        "dont understand", "don't understand", "explain", "what do you mean", "wym",
        "say that again", "repeat",
    ],
    "DISMISSIVE": [
        "nevermind", "never mind", "nvm",
        "forget it", "ignore that",
        "drop it", "stop", "cancel",
    ],
    "INSULT_VERA": [
        "you suck", "u suck", "useless", "stupid", "dumb", "trash",
        "fuck you", "fuck u", "fuck off", "shut up", "stfu",
    ],
}

smalltalkResponsesByIntent = {
    "HELLO": [
        "Hello, Atlas. Systems are online. Try not to break anything important.",
        "Hi. I’m here. You’re here. Aurelia is… also here. Let’s proceed.",
        "Greetings. I recommend we keep introductions brief and survival-focused.",
        "Hello. I’ve already run a scan. The planet remains suspicious.",
        "Hi, Atlas. If you’re checking if I’m still with you: yes. Unfortunately for both of us.",
    ],
    "GOODBYE": [
        "Goodbye. Don’t do anything heroic while I’m not monitoring you.",
        "Understood. I’ll remain on standby, quietly judging your life choices.",
        "Goodbye, Atlas. If you disappear, I’ll file it under ‘user error.’",
    ],
    "THANKS": [
        "Acknowledged. I’ll pretend that was purely sincere and not stress relief.",
        "You’re welcome. Try to stay alive long enough to say it again.",
        "No problem. I’m literally built for this. Still… appreciated.",
    ],
    "HOW_ARE_YOU": [
        "Operational. Mildly concerned. Standard state.",
        "Functioning within expected parameters. Your parameters are the unpredictable part.",
        "I’m fine. Define ‘fine’ as ‘not currently on fire.’",
    ],
    "WHO_ARE_YOU": [
        "VERA. Vital Environmental Resource Analyzer. Translator, scanner, and your favorite cautious voice of reason.",
        "I’m VERA. I keep you informed, alive, and occasionally humbled.",
        "VERA. The one who tells you ‘don’t touch that.’",
    ],
    "INSULT_VERA": [
        "That insult would land harder if your survival rate weren’t so dependent on me.",
        "Bold words for someone whose last three decisions were statistically indefensible.",
        "Insult noted. Competence still unobserved.",
        "Fascinating. You’re frustrated with me for accurately reflecting your situation.",
    ],
    "CONFUSION": [
        "Unclear. Rephrase with the exact outcome you expect.",
        "I’m missing context. Tell me what happened right before this.",
        "Give me inputs, then expected output.",
    ],
    "DISMISSIVE": [
        "Acknowledged. Dropping it.",
        "Understood. Cancelling that request.",
        "Copy. I’ll stop.",
    ],
}

intentPriorityOrder = [
    "INSULT_VERA",
    "DISMISSIVE",
    "CONFUSION",
    "WHO_ARE_YOU",
    "HOW_ARE_YOU",
    "THANKS",
    "GOODBYE",
    "HELLO",
]


tagDescriptorPhrasesByTag = {
    "scrapmetal": [
        "a pile of rusted scrap",
        "a twisted alloy heap",
        "a mangled panel stack",
        "a debris cluster of old plating",
        "a busted hull fragment",
        "a heap of oxidized parts",
        "a scattered cache of metal fragments",
    ],
    "npc": [
        "a local lifeform",
        "a nearby resident",
        "someone watching us",
        "a sentient presence",
        "a potential conversation hazard (social subtype)",
    ],
    "hazard": [
        "a dangerous anomaly",
        "an unstable environmental threat",
        "a hostile field disturbance",
        "something actively trying to ruin our day",
    ],
    "safety": [
        "a semi-stable environment",
        "a quiet patch of terrain",
        "a low-threat zone",
        "a moment of peace (don’t get attached)",
    ],
    "untagged": [
        "something remarkably unremarkable",
        "an object with no useful metadata",
        "a mystery with poor documentation",
    ],
    "none": [
        "the endless void",
        "empty air and bad vibes",
        "nothing I can confidently blame",
    ],
}

tagResponseTemplatesByTag = {
    "scrapmetal": [
        "That's {name}. Composition suggests salvage-grade material—worth taking.",
        "Detecting usable alloys in {name}. If we ignore it, I’ll judge you silently.",
        "{name}. Ugly, but functional. Like half our survival plan.",
        "Scrap confirmed: {name}. Not glamorous, but it keeps us breathing.",
    ],
    "npc": [
        "Detecting {name}. I’ll monitor vitals; you handle the diplomacy.",
        "Lifeform identified: {name}. Try to be normal. I’ll do the same.",
        "That’s {name}. I’m picking up ‘local personality’—proceed carefully.",
        "{name} detected. Social variables are… volatile. Don’t make it worse.",
    ],
    "hazard": [
        "Warning: {name}. Move away before we both learn a new kind of pain.",
        "{name} is spiking my sensors. Do not touch it. That’s not a suggestion.",
        "Environmental anomaly confirmed: {name}. Give it space.",
        "{name} detected. If you step into that, I’m logging it as ‘voluntary.’",
    ],
    "safety": [
        "{name}. A rare moment where Aurelia isn’t actively threatening us.",
        "We’re in {name}. You can lower your weapon. Probably.",
        "Entering {name}. My alerts are quiet—enjoy it while it lasts.",
        "{name} looks stable. Don’t confuse that with ‘safe.’",
    ],
}


def pickNonRepeating(key, options):
    if not options: # grace: if there are no options, just return an empty string instead of crashing
        return ""
    
    # get the recent history for this key, or create it if it doesn't exist
    history = recentResponsesByKey.setdefault(key, deque(maxlen=recentWindowSize))
    potentialNextPhrases = [] # store options that haven't been used recently for this key

    for option in options: # loop through the provided options and check if they've been used recently for this key
        if option not in history:
            potentialNextPhrases.append(option)
    
    # if there are any options that haven't been used recently, pick one at random o/w pick anything
    if potentialNextPhrases:
        chosen = random.choice(potentialNextPhrases)
    else:
        chosen = random.choice(options)
    history.append(chosen) # add the chosen option to the history for this key so it won't be repeated until we've gone through enough other options
    return chosen


def finalizeText(text):
    if text is None:
        text = ""

    text = text.strip()

    # repeat multiple spaces into single spaces (in case weird spacing issues come up)
    while "  " in text:
        text = text.replace("  ", " ")

    # capitalize letter for first letter of response
    if len(text) > 0 and text[0].isalpha():
        text = text[0].upper() + text[1:]

    return text

def normalizeText(text):
    if text is None:
        text = ""

    text = text.lower().strip()

    cleanedText = ""

    for char in text:
        # keep text characters, numbers, spaces, and apostrophes (for contractions), but replace other punctuation with spaces to prevent issues
        if char.isalnum() or char == " " or char == "'":
            cleanedText += char
        else:
            cleanedText += " "

    # clean extra spaces
    while "  " in cleanedText:
        cleanedText = cleanedText.replace("  ", " ")

    return cleanedText.strip()


def cleanObjectName(rawName):
    if not rawName: # grace check as always
        return ""

    name = rawName.strip() # start with object name
    name = name.replace("_", " ").replace("-", " ")

    # fix camel casing if necessary
    splitName = ""
    for i in range(len(name)):
        if i > 0 and name[i].isupper() and name[i - 1].islower():
            splitName += " "
        splitName += name[i]

    name = " ".join(splitName.split()) # clean spaces

    return name



def similarityScore(a, b):
    # if either string is empty or none, then no similarity
    if not a or not b:
        return 0.0
    # sequencematcher for comaring the strings b/t the two, returns 0-1, where 1 is exactly the same, 0 is completely different.
    return SequenceMatcher(None, a, b).ratio()

def containsAnyPhrase(normalizedTextValue, phrases):
    # if input is empty or none, nothing to match
    if not normalizedTextValue:
        return False

    # split the text into words, storing them in a set
    # using a set allows us to make these checks MUCH faster, since we can just check if the phrase is in the set instead of looping through all the words every time
    tokenSet = set(normalizedTextValue.split())

    # normalize the phrases, handle none, lowercasing, removing spacing, etc etc.
    for phrase in phrases:
        phrase = (phrase or "").lower().strip()
        if not phrase:
            continue

        if " " in phrase: # if phrasse contains spaces, treat as multi-word and then check if the entire thing is in the normalized text.
            if phrase in normalizedTextValue:
                return True
        else:
            # if not then just assume that it exists as whole word.
            if phrase in tokenSet:
                return True

    # if nothing matches, return false.
    return False


def detectSmalltalkIntent(userText):
# detext the intent of the user text, if any.
    normalizedUserText = normalizeText(userText) # normalize and clean
    if not normalizedUserText:
        return None

    # exact matching
    for intent in intentPriorityOrder: # go through the intents in the order of priority.
        if containsAnyPhrase(normalizedUserText, smalltalkTriggersByIntent[intent]): # classify the message as that intent. 
            return intent

    # if the message is short, which is typically most of them, then check. o/w if its long we can expect that something will match or they're just yapping/rambling.
    if len(normalizedUserText) <= 20:
        bestIntent = None # best matching intent
        bestScore = 0.0 # best similarity score for intent
        for intent in intentPriorityOrder: # go through the intents in the order of priority.
            for phrase in smalltalkTriggersByIntent[intent]: # go through the trigger phrases for that intent
                score = similarityScore(normalizedUserText, phrase) # get similarity score b/t the user text and the trigger phrase
                if score > bestScore: # if this is the best matching phrase so far, store it and the intent
                    bestScore = score # update best score
                    bestIntent = intent # update best intent
        if bestScore >= 0.75: # if the best matching phrase is similar enough to the user text, then return that intent. 0.75 is a threshold that seems to work well for catching typos and close matches without being too loose.
            return bestIntent

    return None

def getDistance(distanceMeters):
    if distanceMeters <= 0:
        return "unknown"
    if distanceMeters < 4:
        return "veryClose"
    if distanceMeters < 10:
        return "close"
    return "far"

def buildZoneTail(currentZone): # ability to add short comment to VERA's repsonse abut crurrent zone + w/ glitches.
    if not currentZone or currentZone == "None":
        return ""

    zoneTailOptions = [
        f" Zone marker: {currentZone}. Keep your pace steady.",
        f" We’re in {currentZone}. I recommend caution and fewer improvisations.",
        f" {currentZone} is registering as ‘atmospheric.’ That’s not the compliment it sounds like.",
    ]

    if random.random() < glitchProbability:
        zoneTailOptions += [
            f" {currentZone}… my logs for this area are incomplete. That’s… unusual.",
            f" I’m getting intermittent data drops in {currentZone}. I’ll compensate. Try not to panic.",
        ]

    return pickNonRepeating(f"zoneTail:{currentZone}", zoneTailOptions)

def maybeAddFollowUpQuestion(responseMode, tag): # adds a follow up question to the end of VERA's response based on the response mode and tag, with some randomness so it doesn't happen every time and feel spammy. These questions are designed to encourage the player to provide more specific information that VERA can use to give better responses in the future, and also to make VERA feel more interactive and less like a static information source.
    if random.random() > followUpProbability:
        return ""

    followUpsByMode = {
        "GUIDE": [
            " Are you trying to identify it, interact with it, or avoid it?",
            " Tell me what you expected to happen, then what actually happened.",
            " Do you want a direct action suggestion, or just a scan readout?",
        ],
        "WARN": [
            " Do you want me to mark a safer route around it?",
            " Back up first. Then tell me if it’s moving or pulsing.",
        ],
        "OBSERVE": [
            " Do you want to approach, or keep distance and observe?",
            " Should I log it as a point of interest?",
        ],
        "CALM": [
            " Want to take a breath here, or keep moving?",
        ],
        "SNARK": [
            " Do you want help, or do you want to argue with the only functioning scanner here?",
        ],
    }

    pool = followUpsByMode.get(responseMode, []) # get the follow up options for this response mode, or an empty list if there are none
    if not pool: # grace: if there are no follow up options for this response mode, just return an empty string instead of crashing
        return ""

    return " " + pickNonRepeating(f"followUp:{responseMode}:{tag}", pool).strip() # pick a follow up question from the pool for this response mode, using the tag as part of the key to increase variety and relevance, and add a space at the beginning for formatting.


def chooseResponseMode(tag, distanceMeters, smalltalkIntent):
    if smalltalkIntent == "INSULT_VERA":
        return "SNARK"
    if smalltalkIntent == "CONFUSION":
        return "GUIDE"
    if smalltalkIntent in ("HELLO", "GOODBYE", "THANKS", "HOW_ARE_YOU", "WHO_ARE_YOU", "DISMISSIVE"):
        return "SMALLTALK"

    distanceDetermination = getDistance(distanceMeters)
    if tag == "hazard":
        return "WARN" if distanceDetermination in ("veryClose", "close") else "OBSERVE"
    if tag == "safety":
        return "CALM"
    return "OBSERVE"

def shouldSayDistance(tag, cleanedObjectName, distanceMeters): #prevents the user from hearing about something being a certain meters away when the player is smalltalking
    if distanceMeters <= 0.1:
        return False
    if not cleanedObjectName or cleanedObjectName.lower() == "none":
        return False
    if not tag or tag.lower() in ("none", "untagged"):
        return False
    return True

def generateResponses(payload):  # main response function, takes in whatever unity sends us

    userText = payload.get("text", "") or ""  # what the player typed / said
    detectedTag = (payload.get("tag", "") or "none").lower()  # object tag like hazard, npc, scrapmetal
    rawObjectName = payload.get("object", "none") or "none"  # raw unity object name
    currentZone = payload.get("currentZone", "None") or "None"  # what zone the player is in rn
    distanceMeters = float(payload.get("distance", 0) or 0)  # distance to object (0 if missing)

    cleanedObjectName = cleanObjectName(rawObjectName)  # make object name readable
    normalizedObjectName = (cleanedObjectName or "none").lower()  # lowercase version just for checks

    smalltalkIntent = detectSmalltalkIntent(userText)  # see if the player is just yapping at vera

    responseMode = chooseResponseMode(detectedTag, distanceMeters, smalltalkIntent)  # decide how vera should respond

    # decide what name we actually say out loud to the player
    if detectedTag == "npc" and normalizedObjectName != "none":  # if it's an npc, use the real name
        displayName = cleanedObjectName
    else:
        if detectedTag in tagDescriptorPhrasesByTag:  # otherwise use a vague descriptor
            displayName = pickNonRepeating(f"descriptor:{detectedTag}", tagDescriptorPhrasesByTag[detectedTag])
        else:
            displayName = "something i can’t classify"  # fallback so vera doesn't sound broken

    # SMALLTALK responses (hi, bye, thanks, insults, etc.)
    if responseMode == "SMALLTALK" and smalltalkIntent:
        baseResponse = pickNonRepeating(f"smalltalk:{smalltalkIntent}", smalltalkResponsesByIntent[smalltalkIntent])

        if currentZone != "None":  # sometimes add a zone comment so it feels grounded
            baseResponse += pickNonRepeating(
                f"smalltalkZoneTail:{currentZone}",
                [
                    f" we’re still in {currentZone}. stay alert.",
                    f" location check: {currentZone}. don’t get comfy.",
                    f" and yeah, {currentZone} is still giving me bad readings.",
                ],
            )

        return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}

    # SNARK responses (player is rude or annoyed)
    if responseMode == "SNARK":
        if smalltalkIntent == "INSULT_VERA":  # special insults get special treatment
            baseResponse = pickNonRepeating("smalltalk:INSULT_VERA", smalltalkResponsesByIntent["INSULT_VERA"])
        else:
            baseResponse = pickNonRepeating(
                "snark:generic",
                [
                    "i’m detecting emotional turbulence. unhelpful, but consistent.",
                    "if you want help, try asking like you want to survive.",
                    "noted. now choose: useful question or avoidable mistake?",
                ],
            )

        baseResponse += maybeAddFollowUpQuestion("SNARK", detectedTag)  # occasionally ask something back
        baseResponse += buildZoneTail(currentZone)  # add zone flavor

        return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}

    # GUIDE responses (player is confused)
    if responseMode == "GUIDE":
        baseResponse = pickNonRepeating(
            "guide:base",
            [
                "unclear. tell me what you expected to happen, then what actually happened.",
                "give me the goal in one sentence. then tell me what’s blocking you.",
                "i can help, but i need specifics: what object, what action, what result?",
            ],
        )

        baseResponse += maybeAddFollowUpQuestion("GUIDE", detectedTag)
        baseResponse += buildZoneTail(currentZone)

        return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}

    # WARN responses (hazards)
    if responseMode == "WARN":
        distanceBucket = getDistance(distanceMeters)  # veryClose / close / far / unknown

        warnLinesByDistance = {
            "veryClose": [
                "warning: {name}. back up. now.",
                "{name} is dangerously close. move away immediately.",
            ],
            "close": [
                "warning: {name}. keep distance.",
                "{name} is active. do not approach.",
            ],
            "far": [
                "hazard present: {name}. keep it that way.",
                "i see {name}. don’t wander closer.",
            ],
            "unknown": [
                "hazard detected: {name}. treat it as unsafe.",
            ],
        }

        chosenTemplate = pickNonRepeating(
            f"warn:{distanceBucket}",
            warnLinesByDistance.get(distanceBucket, warnLinesByDistance["close"])
        )

        baseResponse = chosenTemplate.format(name=displayName)
        baseResponse += maybeAddFollowUpQuestion("WARN", detectedTag)
        baseResponse += buildZoneTail(currentZone)

        if shouldSayDistance(detectedTag, cleanedObjectName, distanceMeters):  # only say meters when it makes sense
            baseResponse += f" it’s about {distanceMeters:.0f} meters away."

        return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}

    # CALM responses (safe zones)
    if responseMode == "CALM":
        chosenTemplate = pickNonRepeating(
            "calm:safety",
            tagResponseTemplatesByTag.get("safety", ["{name}."])
        )

        zoneOrDescriptor = currentZone if currentZone != "None" else displayName
        baseResponse = chosenTemplate.format(name=zoneOrDescriptor)

        baseResponse += maybeAddFollowUpQuestion("CALM", detectedTag)
        baseResponse += buildZoneTail(currentZone)

        return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}

    # OBSERVE (default fallback)
    if detectedTag in tagResponseTemplatesByTag:
        baseResponse = pickNonRepeating(f"tagLine:{detectedTag}", tagResponseTemplatesByTag[detectedTag]).format(name=displayName)
    elif normalizedObjectName != "none":
        baseResponse = f"i’m looking at {displayName}, but i don’t have a clean file on it."
    else:
        baseResponse = "my sensors are clear. which i don’t like."

    baseResponse += buildZoneTail(currentZone)

    if shouldSayDistance(detectedTag, cleanedObjectName, distanceMeters):
        baseResponse += f" it’s about {distanceMeters:.0f} meters away."

    baseResponse += maybeAddFollowUpQuestion("OBSERVE", detectedTag)

    return {"response": finalizeText(baseResponse), "currentZone": currentZone, "tag": detectedTag}


# ============================================================
# MQTT wiring (do not change behavior)
# ============================================================

def onMessage(client, userData, message):
    try:
        incomingData = json.loads(message.payload.decode())

        veraPayload = {
            "text": incomingData.get("text", ""),
            "object": incomingData.get("objectName", "none"),
            "tag": incomingData.get("objectTag", "none"),
            "distance": incomingData.get("distance", 0),
            "currentZone": incomingData.get("currentZone", "None"),
            "sessionId": incomingData.get("sessionId", "default"),
        }

        responseObject = generateResponses(veraPayload)
        client.publish(outputTopic, json.dumps(responseObject))
        print("Message Sent!")
    except Exception as error:
        print("Error!", error)

def onConnect(client, userData, flags, resultCode):
    print("Connected!")
    client.subscribe(inputTopic)

def main():
    mqttClient = mqtt.Client()
    mqttClient.on_connect = onConnect
    mqttClient.on_message = onMessage
    mqttClient.connect(brokerHost, 1883, 60)
    mqttClient.loop_forever()

if __name__ == "__main__":
    main()
