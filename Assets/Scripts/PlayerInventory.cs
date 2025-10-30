using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private int scrap = 0;
    public int Scrap => scrap;
    public static event Action<int> OnScrapChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
}
