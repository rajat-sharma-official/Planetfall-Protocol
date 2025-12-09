using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    [Header("Resources")]
    [SerializeField] private int scrap = 0;
    public int Scrap => scrap;
    public static event Action<int> OnScrapChanged;

    void OnEnable()
    {
        PlayerController.OnScrapReset += DEBUG_ResetScrapAmount;
    }

    void OnDisable()
    {
        PlayerController.OnScrapReset -= DEBUG_ResetScrapAmount;
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
    }
    
    public void SaveData(ref GameData data)
    {
        data.scrapAmount = this.scrap;
    }

    public void AddScrap(int amount)
    {
        scrap += amount;
        OnScrapChanged?.Invoke(scrap);
    }

    public void RemoveScrap(int amount)
    {
        scrap = Mathf.Max(0, scrap - amount);
        OnScrapChanged?.Invoke(scrap);
    }
    
    private void DEBUG_ResetScrapAmount()
    {
        scrap = 0;
    }
}
