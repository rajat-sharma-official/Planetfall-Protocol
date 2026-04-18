using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        // Singleton so any script can trigger a fade
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fade out (clear -> black), do action, fade in (black -> clear)
    public IEnumerator FadeAndDo(System.Action onBlackScreen)
    {
        yield return StartCoroutine(Fade(0f, 1f));  // fade to black
        onBlackScreen?.Invoke();                    // teleport/reload here
        yield return new WaitForSeconds(0.1f);      // tiny buffer
        yield return StartCoroutine(Fade(1f, 0f));  // fade back in
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }
}