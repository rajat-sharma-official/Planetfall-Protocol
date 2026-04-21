using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameCondition : MonoBehaviour, IDataPersistence, IInteractable
{
    int scrapRequiredToEndGame = 20;
    bool endGameAvailable = false;
    private string interactKey = "E";

    //puzzle before you can end the game
    [SerializeField] private GameObject puzzleCanvas; 
    [SerializeField] private GameObject puzzleManager;

    private bool puzzleOpen = false;

    private void OnEnable()
    {
        PlayerInventory.OnScrapChanged += CheckEndGameCondition;
        ScrapManager.ScrapCountUpdated += SetScrapRequired;
        PuzzleManager.PuzzleOpened += PuzzleOpen;
        PuzzleManager.PuzzleClosed += PuzzleClose;
    }

    private void OnDisable()
    {
        PlayerInventory.OnScrapChanged -= CheckEndGameCondition;
        ScrapManager.ScrapCountUpdated -= SetScrapRequired;
        PuzzleManager.PuzzleOpened -= PuzzleOpen;
        PuzzleManager.PuzzleClosed -= PuzzleClose;
    }

    private void PuzzleOpen()
    {
        puzzleOpen = true;
    }

    private void PuzzleClose()
    {
        puzzleOpen = false;
    }

    private void CheckEndGameCondition(int scrapAmount)
    {
        if(scrapAmount >= scrapRequiredToEndGame)
            endGameAvailable = true;
    }

    private void SetScrapRequired(int scrapCount)
    {
        scrapRequiredToEndGame = scrapCount - 3; //Give player buffer of 3 scrap that they don't need to find
    }

    public void Interact()
    {
        if(endGameAvailable)
        {
            //show cursor for menu navigation
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            puzzleCanvas.SetActive(true);
            puzzleManager.SetActive(true);
        } 
        else
        {
            return;
        }
    }

    public string GetInteractionPrompt()
    {
        if(puzzleOpen) return "";
        return endGameAvailable ? $"Press {interactKey} to repair your ship and leave Aurelia" : $"Collect {scrapRequiredToEndGame} scrap to repair your ship";
    }

    public void LoadData(GameData data)
    {
        this.scrapRequiredToEndGame = data.scrapRequiredToEndGame;
        this.endGameAvailable = data.endGameAvailable;
    }

    public void SaveData(ref GameData data)
    {
        data.scrapRequiredToEndGame = this.scrapRequiredToEndGame;
        data.endGameAvailable = this.endGameAvailable;
    }
}