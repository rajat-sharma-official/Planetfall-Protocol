using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    public GameData()
    {   //instead of zero it should be starting point of new game
        playerPosition = Vector3.zero;
    }
}


