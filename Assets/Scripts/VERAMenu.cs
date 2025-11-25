using UnityEngine;
using UnityEngine.InputSystem; 

public class VERAMenu : MonoBehaviour
{
    [SerializeField] private GameObject VERAMenuPanel;
    private bool isMenuOpen = false; 

    void Start()
    {
        //hide VERA menu 
        VERAMenuPanel.SetActive(false);
    }

    //when key [q] is pressed
    public void OnVERAMenu(InputValue value)
    {
        if(isMenuOpen)
        {
            closeMenu();
        }
        else
        {
            openMenu();
        }
    }
    
    public void closeMenu()
    {
        //hide the menu and resume player movement 
        VERAMenuPanel.SetActive(false);
        isMenuOpen = false; 
        PlayerController.isPaused = false; 

        //hide the cursor 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    public void openMenu()
    {
        //show the menu and pause player movement
        VERAMenuPanel.SetActive(true);
        isMenuOpen = true; 
        PlayerController.isPaused = true; 

        //show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
    }
}