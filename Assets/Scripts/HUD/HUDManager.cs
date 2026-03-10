using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrapText;
    [SerializeField] private InteractionPromptUI interactionPromptUI;

    void OnEnable()
    {
        PlayerInventory.OnScrapChanged += UpdateScrapDisplay;
        PlayerController.OnInteractionPromptChanged += HandleInteractionPromptChanged;
    }

    void OnDisable()
    {
        PlayerInventory.OnScrapChanged -= UpdateScrapDisplay;
        PlayerController.OnInteractionPromptChanged -= HandleInteractionPromptChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
        if (playerInventory != null)
        {
            UpdateScrapDisplay(playerInventory.Scrap);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleInteractionPromptChanged(string? prompt)
    {
        if (interactionPromptUI == null) return;

        if(string.IsNullOrEmpty(prompt))
        {
            interactionPromptUI.HideInteractionPrompt();
        } 
        else
        {
            interactionPromptUI.ShowInteractionPrompt(prompt);
        }
    }

    private void UpdateScrapDisplay(int amount)
    {
        scrapText.text = amount.ToString();
        //Debug.Log($"Scrap: {amount}");
    }
}

