using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void MainMenu()
    {
        //TODO: Add route back to Main Menu Scene
    }

    public void Quit()
    {
        Debug.Log("quit button pressed.. closing application now!");
        Application.Quit();
    }
}
