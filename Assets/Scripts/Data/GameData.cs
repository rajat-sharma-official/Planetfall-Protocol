/*
Credit to Edgar Lopez for writing this file
*/

using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    //Scrap locations
    public int scrapAmount;
    public bool testScrapScavenged;

    //PLayer
    public float playerHealth;

    public GameData()
    {   //Player
        playerPosition = Vector3.zero;
        playerHealth = 100f;

        //Scrap locations
        scrapAmount = 0;
        testScrapScavenged = false;
    }
}


