using UnityEngine;

[System.Serializable]
public class GameData
{
    /** Player **/
    public Vector3 playerPosition;
    public float playerHealth;

    /** VERA **/
    public Vector3 VERAPosition; 

    /** Scrap **/
    public int scrapAmount;
    public bool testScrapScavenged;

    /** NPCs**/
    //NPC Scholar2 (Harvel)
    public Vector3 NPCsch2Position;
    public int NPCsch2ConversationState;

    public GameData()
    {   /** Player **/
        playerPosition = Vector3.zero;
        playerHealth = 100f;

        /** VERA **/ 
        VERAPosition = Vector3.zero;

        /** Scrap **/
        scrapAmount = 0;
        testScrapScavenged = false;

        /** NPCs **/
        //NPC Scholar2 (Harvel)
        NPCsch2Position = new Vector3(-5, 1, -4);
        NPCsch2ConversationState = 0;
    }
}


