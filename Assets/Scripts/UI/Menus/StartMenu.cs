using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        loadGameButton.interactable = DataPersistenceManager.instance.HasSaveData();
    }

    public void OnNewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("BaseScene");
    }

    public void OnLoadGameClicked()
    {
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene("BaseScene");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}