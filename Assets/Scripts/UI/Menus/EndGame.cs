using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        audioManager.Play("Start");
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void MainMenu()
    {
        audioManager.Stop("Start");
        SceneManager.LoadScene("EchoScene");
    }

    public void Quit()
    {
        Debug.Log("quit button pressed.. closing application now!");
        Application.Quit();
    }
}
