using UnityEngine;
using System;
using UnityEngine.Assertions;

public class ScrapLocation : MonoBehaviour, IInteractable, IDataPersistence
{
    [Header("Scrap Location Settings")]
    [SerializeField] private bool hasBeenScavenged = false;
    //[SerializeField] private int scavengeTime = 3; //seconds
    [SerializeField] private string interactKey = "E";

    private PlayerInventory playerInventory;

    void OnEnable()
    {
        PlayerController.OnScrapReset += DEBUG_ResetScrap;
    }

    void OnDisable()
    {
        PlayerController.OnScrapReset -= DEBUG_ResetScrap;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData data)
    {
        this.hasBeenScavenged = data.testScrapScavenged;

        //if already scavenged, hide scrap on load 
        if(hasBeenScavenged)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.testScrapScavenged = this.hasBeenScavenged;
    }

    public void Interact()
    {
        if (hasBeenScavenged)
        {
            return;
        }

        //TODO: add scavenging coroutine, show progress bar
        Scavenge();
    }

    private void Scavenge()
    {
        hasBeenScavenged = true;
        playerInventory.AddScrap(1);

        // play sfx for collecting scrap
        FindObjectOfType<AudioManager>().Play("CollectScrap");

        // make scrap disappear
        gameObject.SetActive(false);
    }

    private void DEBUG_ResetScrap()
    {
        hasBeenScavenged = false;
    }

    public string GetInteractionPrompt()
    {
        return hasBeenScavenged ? "Already scavenged" : $"Press {interactKey} to scavenge";
    }

    public void Test_ScrapScavenged()
    {
        //store player's current scrap count as baseline before scavenging
        int scrapBefore = playerInventory.Scrap; 
        //trigger the scavenge
        Scavenge();
        try
        {
            //verify when scrap is collected that it is marked as scaveneged
            Assert.IsTrue(hasBeenScavenged);
            //verify the inventory increased by exactly one from baseline
            Assert.AreEqual(playerInventory.Scrap, scrapBefore + 1);
        }
        catch (Exception e)
        {
            //if either assert fails, log the reason why the test failed
            Debug.Log("Scavenge Test Failed!" + e);
        }
        //if no exception was thrown, both asserts passed!
        Debug.Log("Scavenge Test Passed!");
    }
}
