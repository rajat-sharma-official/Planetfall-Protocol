using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HazardRedHueUI : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float minAlpha = 0.05f;
    [SerializeField] private float maxAlpha = 0.65f;
    [SerializeField] private float cyclesPerSecond = 1.0f;
    [SerializeField] private float fadeInSeconds = 0.25f;

    [Header("Audio (optional)")]
    [SerializeField] private string sfxName = "Damage";

    [Header("Fall Damage Flash")]
    [SerializeField] private float flashInSeconds = 0.05f;
    [SerializeField] private float flashOutSeconds = 0.45f;

    private CanvasGroup group;
    private Coroutine breatheCo;
    
    private Coroutine flashCo;
    private AudioManager audioManager;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();

        // Don’t block clicks
        group.blocksRaycasts = false;
        group.interactable = false;

        // Start hidden
        group.alpha = 0f;

        // Cache once (don’t keep FindObjectOfType every enter/exit)
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartBreathing()
    {
        if (breatheCo != null)
            StopCoroutine(breatheCo);

        group.alpha = 0f;
        audioManager?.Play(sfxName);

        breatheCo = StartCoroutine(BreatheRoutine());
    }

    public void StopBreathing()
    {
        if (breatheCo != null)
            StopCoroutine(breatheCo);

        breatheCo = null;
        audioManager?.Stop(sfxName);
        group.alpha = 0f;
    }

    private IEnumerator BreatheRoutine()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            float s = Mathf.Sin(2f * Mathf.PI * cyclesPerSecond * t) * 0.5f + 0.5f;
            float eased = Mathf.SmoothStep(0f, 1f, s);

            float targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, eased);
            float ramp = (fadeInSeconds <= 0f) ? 1f : Mathf.Clamp01(t / fadeInSeconds);

            group.alpha = targetAlpha * ramp;
            yield return null;
        }
    }

    public void DoFlashingDamage(float normalizedDmg)
    {
        if (flashCo != null)
            StopCoroutine(flashCo);

        float targetAlpha = Mathf.Lerp(0.2f, 0.75f, Mathf.Clamp01(normalizedDmg));
        flashCo = StartCoroutine(FlashRoutine(targetAlpha));
    }

    private IEnumerator FlashRoutine(float targetAlpha)
    {
        float startAlpha = group.alpha;
        float t = 0f;

        while (t < flashInSeconds)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / flashInSeconds);
            yield return null;
        }

        group.alpha = targetAlpha;

        t = 0f;
        while (t < flashOutSeconds)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(targetAlpha, 0f, t / flashOutSeconds);
            yield return null;
        }

        group.alpha = 0f;
        flashCo = null;
    }
}