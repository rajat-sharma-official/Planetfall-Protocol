/*
Credit to Edgar Lopez for writing this file
*/

using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;

    /** Scrap Locations **/
    public int scrapAmount;
    //Test scrap
    public bool testScrapScavenged;

    //VERA position 
    public Vector3 VERAPosition; 

    /** NPCs **/
    //NPC_Sch2 (Harvel)
    public Vector3 NPCsch2Position;
    public int NPCsch2ConversationState;

    public GameData()
    {   //Player
        playerPosition = Vector3.zero;

        //Scrap locations
        scrapAmount = 0;
        testScrapScavenged = false;

        //VERA 
        VERAPosition = Vector3.zero;
    }
}


