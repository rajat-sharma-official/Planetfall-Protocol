using System.Collections;
using UnityEngine;

//<summary>
// Controls the "RedHue" overlay (full-screen red tint panel) and its children
//(red image + warning text) using a breathing/pulsing fade effect.
[RequireComponent(typeof(CanvasGroup))]
public class HazardRedHueUI : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Lowest alpha during the breathing pulse.")]
    [SerializeField] private float minAlpha = 0.05f;

    [Tooltip("Highest alpha during the breathing pulse.")]
    [SerializeField] private float maxAlpha = 0.65f;

    [Tooltip("How many breathing cycles per second. (1.0 = once per second)")]
    [SerializeField] private float cyclesPerSecond = 1.0f;

    [Tooltip("How quickly the overlay fades in when it first starts.")]
    [SerializeField] private float fadeInSeconds = 0.25f;

    private CanvasGroup group;
    private Coroutine breatheCo;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();

        // Make sure this overlay never blocks UI clicks (important for menus/HUD)
        group.blocksRaycasts = false;
        group.interactable = false;

        // Start hidden
        group.alpha = 0f;
    }

    // Starts the breathing/pulsing red overlay.
    // Safe to call multiple times (restarts cleanly).
    public void StartBreathing()
    {
        // Stop any previous pulsing coroutine
        if (breatheCo != null)
        {
            StopCoroutine(breatheCo);
            breatheCo = null;
        }

        // Reset alpha before starting
        group.alpha = 0f;

        // Start breathing animation
        breatheCo = StartCoroutine(BreatheRoutine());
    }


    // Stops the breathing effect and hides the overlay immediately.
    public void StopBreathing()
    {
        if (breatheCo != null)
        {
            StopCoroutine(breatheCo);
            breatheCo = null;
        }

        // Hide immediately
        group.alpha = 0f;
    }


    // Coroutine that smoothly oscillates alpha using a sine wave.
    // Also ramps in from 0 to full breathing range (fadeInSeconds).
    private IEnumerator BreatheRoutine()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            // sine wave 0..1
            float s = Mathf.Sin(2f * Mathf.PI * cyclesPerSecond * t) * 0.5f + 0.5f;

            // Smooth the curve so it feels like "breathing" instead of blinking
            float eased = Mathf.SmoothStep(0f, 1f, s);

            // Compute target alpha between min/max
            float targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, eased);

            //ramp-in so it doesn't instantly pop on
            float ramp = (fadeInSeconds <= 0f) ? 1f : Mathf.Clamp01(t / fadeInSeconds);

            group.alpha = targetAlpha * ramp;

            yield return null;
        }
    }
}
