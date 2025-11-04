using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour{
    public bool isPaused = false; 
    public GameObject pauseMenu; 

    void Start(){
        pauseMenu.SetActive(false);
    }

    public void OnPause(InputValue value){
        if(isPaused){
            resumeGame();
        }
        else{
            pauseGame();
        }
    }

    public void resumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void pauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
