using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour{
    
    //pause menu panel
    [SerializeField] private GameObject pauseMenu; 
    private bool isPaused = false; 

    void Start(){
        //hide menu at game start
        pauseMenu.SetActive(false);

        //hide cursor + lock for fps-style camera movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        //hide pause menu
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        //hide cursor + lock for fps-style camera movement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //resume player movement
        PlayerController.isPaused = false; 
    }

    public void pauseGame(){       
        //show pause menu
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        
        //show cursor for menu navigation
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //pause player movement 
        PlayerController.isPaused = true;
    }

    public void quitGame(){
        //close application
        Debug.Log("quit button pressed.. closing application now!");
        Application.Quit();
    }
}
