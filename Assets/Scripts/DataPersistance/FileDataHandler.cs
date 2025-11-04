using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;




public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                // translate from JSON to GamaData Using ~ Json.NET(https://www.newtonsoft.com/json/help/html/SerializingJSON.htm)
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "" + e);
            }

        }
        return loadedData;
    }
    
    public void Save(GameData data)
    {
        // guard: filename must be set
        if (string.IsNullOrWhiteSpace(dataFileName))
            throw new ArgumentException("Data file name is null or empty. Set it in the inspector or constructor.");
        
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // translate from GameData to Json ~ Using Json.NET(https://www.newtonsoft.com/json/help/html/SerializingJSON.htm)
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
          Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);  
        }
    }
}
