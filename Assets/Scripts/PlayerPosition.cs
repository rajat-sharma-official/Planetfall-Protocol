using UnityEngine;
using System.Collections;   

public class PlayerPosition : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }
    
     public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
