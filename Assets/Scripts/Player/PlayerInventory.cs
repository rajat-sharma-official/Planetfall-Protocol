using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    [Header("Resources")]
    [SerializeField] private int scrap = 0;
    public int Scrap => scrap;
    public static event Action<int> OnScrapChanged;

    [Header("Repair Unlock")] // all the stuff for VERA repair unlock trigger
    [SerializeField] private int requiredScrap = 30;
    [SerializeField] private VERAMenu veraMenu;
    [SerializeField] private VERAPopupController popupController;
    private bool repairUnlockTriggered = false;

    void OnEnable()
    {
        PlayerController.OnScrapReset += DEBUG_ResetScrapAmount;
        PlayerController.OnMaxScrap += DEBUG_GiveMaxScrap;
    }

    void OnDisable()
    {
        PlayerController.OnScrapReset -= DEBUG_ResetScrapAmount;
        PlayerController.OnMaxScrap -= DEBUG_GiveMaxScrap;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadData(GameData data)
    {
        this.scrap = data.scrapAmount;
        OnScrapChanged?.Invoke(scrap);
        CheckRepairUnlock();
    }
    
    public void SaveData(ref GameData data)
    {
        data.scrapAmount = this.scrap;
    }

    public void AddScrap(int amount)
    {
        scrap += amount;
        OnScrapChanged?.Invoke(scrap);
        CheckRepairUnlock();
    }

    public void RemoveScrap(int amount)
    {
        scrap = Mathf.Max(0, scrap - amount);
        OnScrapChanged?.Invoke(scrap);
        if (scrap < requiredScrap)
        {
            repairUnlockTriggered = false;
        }
    }
    
    private void DEBUG_ResetScrapAmount()
    {
        scrap = 0;
        OnScrapChanged?.Invoke(scrap);
        repairUnlockTriggered = false;
    }

    private void DEBUG_GiveMaxScrap()
    {
        scrap = 500;
        OnScrapChanged?.Invoke(scrap);
        CheckRepairUnlock();
    }

    private void CheckRepairUnlock()
{
    if (repairUnlockTriggered)
        return;

    if (scrap >= requiredScrap)
    {
        repairUnlockTriggered = true;

        if (popupController != null)
            popupController.ShowPopup();

        if (veraMenu != null)
            veraMenu.QueueRepairReadyMessage();
    }
}

    private void Awake()
    {
        if (veraMenu == null)
            veraMenu = FindFirstObjectByType<VERAMenu>();

        if (popupController == null)
            popupController = FindFirstObjectByType<VERAPopupController>();
    }
}
