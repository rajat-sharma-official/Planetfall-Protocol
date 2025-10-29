using UnityEngine;

public class ScrapLocation : MonoBehaviour, IInteractable
{
    [Header("Scrap Location Settings")]
    [SerializeField] private bool hasBeenScavenged = false;
    //[SerializeField] private int scavengeTime = 3; //seconds
    [SerializeField] private string interactKey = "E";

    private PlayerInventory playerInventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

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
    }
    
    public string GetInteractionPrompt()
    {
        return hasBeenScavenged ? "Already scavenged" : $"Press {interactKey} to scavenge";
    }
}
