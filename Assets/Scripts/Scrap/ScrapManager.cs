using UnityEngine;
using System;
using System.Collections.Generic;

public class ScrapManager : MonoBehaviour, IDataPersistence
{
    public static ScrapManager Instance { get; private set; }

    private Dictionary<GameObject, bool> scrapLocations = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        RegisterAllScrap();
    }

    void OnEnable()
    {
        PlayerController.OnScrapReset += ResetAllScrap;
    }

    void OnDisable()
    {
        PlayerController.OnScrapReset -= ResetAllScrap;
    }

    private void RegisterAllScrap()
    {
        GameObject[] scrapObjects = GameObject.FindGameObjectsWithTag("ScrapMetal");

        foreach (var obj in scrapObjects)
        {
            scrapLocations[obj] = false; // false = not yet scavenged
        }

        Debug.Log($"ScrapManager registered {scrapLocations.Count} scrap locations.");
    }

    public void LoadData(GameData data)
    {
        List<GameObject> keys = new List<GameObject>(scrapLocations.Keys);
        
        foreach (var key in keys)
        {
            if (data.scavengedScrapIds.Contains(key.name))
            {
                scrapLocations[key] = true;
                key.SetActive(false);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.scavengedScrapIds.Clear();

        foreach (var kvp in scrapLocations)
        {
            if (kvp.Value)
                data.scavengedScrapIds.Add(kvp.Key.name);
        }
    }

    public void Scavenge(GameObject scrapObject)
    {
        if (!scrapLocations.ContainsKey(scrapObject))
        {
            Debug.LogWarning($"{scrapObject.name} is not a registered scrap location.");
            return;
        }

        if (scrapLocations[scrapObject])
        {
            Debug.Log($"{scrapObject.name} has already been scavenged.");
            return;
        }

        scrapLocations[scrapObject] = true;
        scrapObject.SetActive(false);
    }

    public bool IsScavenged(GameObject scrapObject)
    {
        return scrapLocations.TryGetValue(scrapObject, out bool scavenged) && scavenged;
    }

    public int GetRemainingCount()
    {
        int count = 0;
        foreach (var scavenged in scrapLocations.Values)
            if (!scavenged) count++;
        return count;
    }

    private void ResetAllScrap()
    {
        List<GameObject> keys = new List<GameObject>(scrapLocations.Keys);

        foreach (var key in keys)
        {
            scrapLocations[key] = false;
            key.SetActive(true);
        }

        Debug.Log("All scrap has been reset.");
    }
}