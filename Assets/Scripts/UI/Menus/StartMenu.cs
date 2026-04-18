using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button loadGameButton;
    private AudioManager audioManager;


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("Start");
        loadGameButton.interactable = DataPersistenceManager.instance.HasSaveData();
    }

    public void OnNewGameClicked()
    {
        audioManager.Stop("Start");
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("BaseScene");
    }

    public void OnLoadGameClicked()
    {
        audioManager.Stop("Start");
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene("BaseScene");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}