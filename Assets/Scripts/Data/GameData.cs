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

    public GameData()
    {   //Player
        playerPosition = Vector3.zero;

        //Scrap locations
        scrapAmount = 0;
        testScrapScavenged = false;
    }
}


