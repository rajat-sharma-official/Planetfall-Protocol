/*
Credit to Edgar Lopez for writing this file
*/

using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
