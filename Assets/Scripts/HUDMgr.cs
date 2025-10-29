using UnityEngine;
using TMPro;

public class HUDMgr : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scrapText;

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
        if(playerInventory != null)
        {
            UpdateScrapDisplay(playerInventory.Scrap);
        }   
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
}
