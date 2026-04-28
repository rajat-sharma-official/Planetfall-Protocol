/*
Credit to Edgar Lopez for writing this file
*/
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "save.json";
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    public static event Action onSaveStarted;
    public static event Action onSaveFinished;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persiatnce Manager in the scene");

        }
        instance = this;
    }

    public void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        dataHandler.Delete();
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing dat to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPeristenceObj in dataPersistenceObjects)
        {
            dataPeristenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        onSaveStarted?.Invoke();

        foreach (IDataPersistence dataPeristenceObj in dataPersistenceObjects)
        {
            dataPeristenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);

        Debug.Log("Game saved.");
        onSaveFinished?.Invoke();
    }
    /*
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    */
    private List<IDataPersistence> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
            
    }

    public bool HasSaveData()
    {
        return System.IO.File.Exists(
            System.IO.Path.Combine(Application.persistentDataPath, fileName)
        );
    }
}

