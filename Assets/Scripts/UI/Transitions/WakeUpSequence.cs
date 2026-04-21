using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class WakeUpSequence : MonoBehaviour
{
    [Header("UI")] // ui references for the black screen and the text we show during the intro
    [SerializeField] private CanvasGroup blackOverlay;
    [SerializeField] private TMP_Text introText;
    [SerializeField] private InputActionReference veraAction;

    [Header("Camera")] // camera stuff for the fake waking up movement
    [SerializeField] private Transform cameraHolder;

    // where the camera starts at the beginning of the intro
    // this makes it feel like the player is lying down or slumped over
    [SerializeField] private Vector3 startLocalPositionOffset = new Vector3(0f, -0.15f, 0f);

    // where the camera ends up after the intro is done
    // zero means back to its normal position
    [SerializeField] private Vector3 endLocalPositionOffset = Vector3.zero;

    // starting rotation for the camera
    // this makes the player feel tilted while "waking up"
    [SerializeField] private Vector3 startLocalRotation = new Vector3(0f, 0f, 75f);

    // ending rotation after the player is fully awake
    [SerializeField] private Vector3 endLocalRotation = Vector3.zero;

    [Header("Player Control")] // scripts to disable movement/look while the intro plays
    [SerializeField] private MonoBehaviour playerMovementScript;
    [SerializeField] private MonoBehaviour playerLookScript;

    [Header("Timing")] // timing values for how long each part lasts
    [SerializeField] private float blackHoldTime = 2f;   // how long to stay fully black before showing text
    [SerializeField] private float lineHoldTime = 4f;  // how long each line stays visible
    [SerializeField] private float fadeTime = 0.35f;     // how fast the text fades in/out
    [SerializeField] private float wakeDuration = 3f;    // how long the actual wake-up camera movement takes

    // stores the camera's original position so we can return to it at the end
    private Vector3 originalLocalPosition;

    private void Start()
    {
        // if this is not a brand new game, skip the wake-up intro completely
        // this is what stops load game from doing the same intro
        if (!GameStartState.PlayWakeIntro)
        {
            blackOverlay.alpha = 0f; // make sure the black screen is hidden
            introText.text = "";     // clear any text just in case
            Color hiddenColor = introText.color;
            hiddenColor.a = 0f;
            introText.color = hiddenColor;
            SetPlayerControl(true);  // make sure the player can move normally
            return;
        }

        // reset the flag right away so the intro does not accidentally play again later
        GameStartState.PlayWakeIntro = false;

        // save the camera's normal local position before moving it for the intro
        originalLocalPosition = cameraHolder.localPosition;

        // start the full wake-up sequence
        StartCoroutine(PlayWakeSequence());
    }

    private IEnumerator PlayWakeSequence()
    {
        // disable player movement and look so they cannot move during the intro
        SetPlayerControl(false);

        // start with a full black screen
        blackOverlay.alpha = 1f;

        // hide text at the beginning
        Color startColor = introText.color;
        startColor.a = 0f;
        introText.color = startColor;
        introText.text = "";

        // move the camera into the "starting asleep" pose
        cameraHolder.localPosition = originalLocalPosition + startLocalPositionOffset;
        cameraHolder.localRotation = Quaternion.Euler(startLocalRotation);

        // hold on black for a short moment before showing text
        yield return new WaitForSeconds(blackHoldTime);

        string veraKeyName = "[BINDING MISSING]";

        if (veraAction != null && veraAction.action != null)
        {
            veraKeyName = veraAction.action.GetBindingDisplayString();
        }

        // show your intro lines one at a time
        yield return ShowLine("SYSTEM: Consciousness restored.");
        yield return ShowLine("VERA: Atlas... finally. Do you have any idea how embarrassing that landing was?");
        yield return ShowLine("ATLAS: ...VERA? Where are we?");
        yield return ShowLine("SYSTEM: Press <color=#D4AF37>" + veraKeyName + "</color> to access VERA.");
        yield return ShowLine("VERA: Unknown, but someone nearby had a front-row seat to your landing. Start with them.");

        // clear the text before the actual fade-in/wake-up motion starts
        introText.text = "";
        Color clearColor = introText.color;
        clearColor.a = 0f;
        introText.color = clearColor;

        float t = 0f;

        // gradually fade from black and move the camera back to normal
        while (t < wakeDuration)
        {
            t += Time.deltaTime;
            float p = t / wakeDuration;

            // fade the black screen out over time
            blackOverlay.alpha = Mathf.Lerp(1f, 0f, p);

            // move the camera from the sleepy position back to normal
            cameraHolder.localPosition = Vector3.Lerp(
                originalLocalPosition + startLocalPositionOffset,
                originalLocalPosition + endLocalPositionOffset,
                p
            );

            // rotate the camera from tilted to upright
            cameraHolder.localRotation = Quaternion.Lerp(
                Quaternion.Euler(startLocalRotation),
                Quaternion.Euler(endLocalRotation),
                p
            );

            yield return null;
        }

        // force final values so it ends cleanly
        blackOverlay.alpha = 0f;
        cameraHolder.localPosition = originalLocalPosition + endLocalPositionOffset;
        cameraHolder.localRotation = Quaternion.Euler(endLocalRotation);

        // now that the intro is over, let the player move again
        SetPlayerControl(true);
    }

    private IEnumerator ShowLine(string line)
    {
        // set the text to whatever line we want to show
        introText.richText = true;
        introText.text = line;

        // fade text in
        yield return FadeText(0f, 1f, fadeTime);

        // leave it on screen for a bit
        yield return new WaitForSeconds(lineHoldTime);

        // fade text back out
        yield return FadeText(1f, 0f, fadeTime);
    }

    private IEnumerator FadeText(float from, float to, float duration)
    {
        float t = 0f;

        // grab the current text color so we can only change alpha
        Color c = introText.color;

        while (t < duration)
        {
            t += Time.deltaTime;

            // lerp the alpha from one value to another
            c.a = Mathf.Lerp(from, to, t / duration);
            introText.color = c;

            yield return null;
        }

        // force the final alpha so it ends exactly where we want
        c.a = to;
        introText.color = c;
    }

    private void SetPlayerControl(bool enabled)
    {
        // turn movement on/off
        if (playerMovementScript != null)
            playerMovementScript.enabled = enabled;

    }
}