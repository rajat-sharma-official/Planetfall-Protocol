using UnityEngine;
using TMPro;

public class HUDMgr : MonoBehaviour
{
    [Header("HUD Text")]
    [SerializeField] private TextMeshProUGUI scrapText;

    [Header("Panels")]
    [SerializeField] private InteractionPromptUI interactionPrompt; // <-- drag your InteractionPanel here
    
    [Header("Crosshair")]
    [SerializeField] private RectTransform crosshairPanel;  // drag Crosshair/Panel here
    [SerializeField, Range(0.1f, 2f)] private float pressedScale = 0.50f;

    private Vector3 _originalScale = Vector3.one;

    void OnEnable()
    {
        PlayerInventory.OnScrapChanged += UpdateScrapDisplay;
    }

    void OnDisable()
    {
        PlayerInventory.OnScrapChanged -= UpdateScrapDisplay;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
        if (playerInventory != null)
        {
            UpdateScrapDisplay(playerInventory.Scrap);
        }

        //start with interaction prompt hidden
        interactionPrompt?.Hide();   

        //crosshair original scale
         if (crosshairPanel)
            _originalScale = crosshairPanel.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateScrapDisplay(int amount)
    {
        // ERROR: Updating the text is not currently working, only shows up in console. 
        scrapText.text = $"Scrap: {amount}";
        Debug.Log($"Scrap: {amount}");
    }
    
        // called by PlayerController
    public void SetCrosshairPressed(bool pressed)
    {
        if (!crosshairPanel) return;
        crosshairPanel.localScale = pressed ? _originalScale * pressedScale : _originalScale;
    }


    //Show the small prompt panel with a message
    public void ShowInteractPrompt(string text) => interactionPrompt?.Show(text);
    
    //Hide the small prompt panel.
    public void HideInteractPrompt() => interactionPrompt?.Hide();
}

