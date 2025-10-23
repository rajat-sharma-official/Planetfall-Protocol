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
        PlayerInventory inventory = FindFirstObjectByType<PlayerInventory>();
        if(inventory != null)
        {
            UpdateScrapDisplay(inventory.Scrap);
        }   
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void UpdateScrapDisplay(int amount)
    {
        scrapText.text = $"Scrap: {amount}";
    }
}
