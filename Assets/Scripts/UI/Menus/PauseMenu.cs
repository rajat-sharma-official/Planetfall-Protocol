using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseMenu : MonoBehaviour{
    
    //pause menu panel
    [SerializeField] private GameObject pauseMenu; 
    private bool isPaused = false; 

    public static event Action PauseMenuActive;
    public static event Action PauseMenuInactive;
    private bool dialoguePanelOpen = false;
    private bool veraMenuOpen = false;
    private bool settingsMenuOpen = false;
    public static event Action OpenSettingsMenuEvent;
    public static event Action CloseSettingsMenuEvent;

    private void OnEnable()
    {
        NPC_Base.OnConversationStart += DialoguePanelOpen;
        NPC_Base.OnConversationEnd += DialoguePanelClose;
        VERAMenu.VERAMenuActive += VERAMenuOpen;
        VERAMenu.VERAMenuInactive += VERAMenuClose;
    }

    private void OnDisable()
    {
        NPC_Base.OnConversationStart -= DialoguePanelOpen;
        NPC_Base.OnConversationEnd -= DialoguePanelClose;
        VERAMenu.VERAMenuActive -= VERAMenuOpen;
        VERAMenu.VERAMenuInactive -= VERAMenuClose;
    }

    void Start(){
        //hide menu at game start
        pauseMenu.SetActive(false);

        //hide cursor + lock for fps-style camera movement
        Debug.Log("cursor is hidden");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void VERAMenuOpen()
    {
        veraMenuOpen = true;
    }

    private void VERAMenuClose()
    {
        veraMenuOpen = false;
    }

    private void DialoguePanelOpen()
    {
        dialoguePanelOpen = true;
    }

    private void DialoguePanelClose()
    {
        dialoguePanelOpen = false;
    }

    public void OnPause(InputValue value){
        if(settingsMenuOpen)
        {
            CloseSettingsMenu();
        } 
        else if(isPaused){
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

        //invoke resume event
        PauseMenuInactive?.Invoke();

        //resume audio and sfx currently triggered
        FindObjectOfType<AudioManager>().ResumeAll();
    }

    public void pauseGame(){     
        //don't pause if any other menu is open
        if(dialoguePanelOpen || veraMenuOpen)
            return;

        //show pause menu
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        
        //show cursor for menu navigation
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //invoke resume event
        PauseMenuActive?.Invoke();

        //pause all audio 
        FindObjectOfType<AudioManager>().PauseAll();
    }

    public void quitGame(){
        Debug.Log("quit button pressed.. closing application now!");
        Application.Quit();
        
    }
    public void saveGame()
    {
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("Game saved.");
    }

    public void settingsMenu()
    {
        OpenSettingsMenuEvent?.Invoke();
        settingsMenuOpen = true;
    }

    private void CloseSettingsMenu()
    {
        CloseSettingsMenuEvent?.Invoke();
        settingsMenuOpen = false;
    }
}
