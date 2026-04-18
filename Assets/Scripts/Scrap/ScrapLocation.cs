using UnityEngine;
using System;
using UnityEngine.Assertions;

public class ScrapLocation : MonoBehaviour, IInteractable
{
    private string interactKey = "E";
    private PlayerInventory playerInventory;

    [SerializeField] private VERAFogPathController fogPathController; // for fog scrap tracking

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (fogPathController == null)
            fogPathController = FindObjectOfType<VERAFogPathController>();
    }

    public void Interact()
    {
        if (ScrapManager.Instance.IsScavenged(gameObject)) return;

        playerInventory.AddScrap(1);
        FindObjectOfType<AudioManager>().Play("CollectScrap");
    
        if (fogPathController != null)
            fogPathController.MarkTargetAsGrabbed(transform);

        ScrapManager.Instance.Scavenge(gameObject);
    }

    public string GetInteractionPrompt()
    {
        if (ScrapManager.Instance.IsScavenged(gameObject))
            return "Already scavenged";

        return $"Press {interactKey} to scavenge";
    }

    // [Header("Scrap Location Settings")]
    // [SerializeField] private bool hasBeenScavenged = false;
    // //[SerializeField] private int scavengeTime = 3; //seconds
    // [SerializeField] private string interactKey = "E";

    // private PlayerInventory playerInventory;

    // void OnEnable()
    // {
    //     PlayerController.OnScrapReset += DEBUG_ResetScrap;
    // }

    // void OnDisable()
    // {
    //     PlayerController.OnScrapReset -= DEBUG_ResetScrap;
    // }

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     playerInventory = FindFirstObjectByType<PlayerInventory>();

    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // public void LoadData(GameData data)
    // {
    //     this.hasBeenScavenged = data.testScrapScavenged;
    // }

    // public void SaveData(ref GameData data)
    // {
    //     data.testScrapScavenged = this.hasBeenScavenged;
    // }

    // public void Interact()
    // {
    //     if (hasBeenScavenged)
    //     {
    //         return;
    //     }

    //     //TODO: add scavenging coroutine, show progress bar
    //     Scavenge();
    // }

    // private void Scavenge()
    // {
    //     hasBeenScavenged = true;
    //     playerInventory.AddScrap(1);

    //     // play sfx for collecting scrap
    //     FindObjectOfType<AudioManager>().Play("CollectScrap");

    //     // make scrap disappear
    //     gameObject.SetActive(false);
    // }

    // private void DEBUG_ResetScrap()
    // {
    //     hasBeenScavenged = false;
    // }

    // public string GetInteractionPrompt()
    // {
    //     return hasBeenScavenged ? "Already scavenged" : $"Press {interactKey} to scavenge";
    // }

    // public void Test_ScrapScavenged()
    // {
    //     //store player's current scrap count as baseline before scavenging
    //     int scrapBefore = playerInventory.Scrap; 
    //     //trigger the scavenge
    //     Scavenge();
    //     try
    //     {
    //         //verify when scrap is collected that it is marked as scaveneged
    //         Assert.IsTrue(hasBeenScavenged);
    //         //verify the inventory increased by exactly one from baseline
    //         Assert.AreEqual(playerInventory.Scrap, scrapBefore + 1);
    //         //verify that when collected, object disappears
    //         Assert.IsFalse(gameObject.activeSelf);
    //     }
    //     catch (Exception e)
    //     {
    //         //if either assert fails, log the reason why the test failed
    //         Debug.Log("Scavenge Test Failed!" + e);
    //     }
    //     //if no exception was thrown, both asserts passed!
    //     Debug.Log("Scavenge Test Passed!");
    // }
}
