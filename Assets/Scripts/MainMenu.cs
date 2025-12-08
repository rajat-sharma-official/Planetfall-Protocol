using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel;
   
    void Start(){
        PlayerController.isPaused = true; 
    }
   
   //load into basescene
    public void PlayGame(){
        menuPanel.SetActive(false);
        PlayerController.isPaused = false;
    }

    //quit application
    public void QuitGame(){
        Application.Quit();
    }
}
