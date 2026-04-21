using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button loadGameButton;
    [SerializeField] private CanvasGroup fadeScreen; // reference to UI object so we can fade alpha
    [SerializeField] private float fadeDuration = 1f;

    private AudioManager audioManager;
    private bool isStarting = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("Start");
        loadGameButton.interactable = DataPersistenceManager.instance.HasSaveData();

        if (fadeScreen != null) // make sure it is fully invisible on start
        {
            fadeScreen.alpha = 0f; // make sure everything is off just in case 
            fadeScreen.interactable = false;
            fadeScreen.blocksRaycasts = false;
        }
    }

    public void OnNewGameClicked()
    {
        if (isStarting) return;
        GameStartState.PlayWakeIntro = true; // set flag to play wake intro when the first scene loads
        StartCoroutine(NewGameRoutine());
    }

    private IEnumerator NewGameRoutine()
    {
        isStarting = true;

        audioManager.Stop("Start");
        DataPersistenceManager.instance.NewGame();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadeScreen.alpha = 1f;
        SceneManager.LoadScene("BaseScene");
    }

    public void OnLoadGameClicked()
    {
        audioManager.Stop("Start");
        DataPersistenceManager.instance.LoadGame();
        GameStartState.PlayWakeIntro = false; // no wake intro when loading a game, just go straight to the scene
        SceneManager.LoadScene("BaseScene");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}