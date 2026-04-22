using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    /** Player **/
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public float playerHealth;

    /** Translation **/
    public int uniqueNPCsTalkedTo;
    public int dialogueOptionsAvailable;

    /** VERA **/
    public Vector3 VERAPosition; 

    /** Scrap **/
    public int scrapAmount;
    public List<string> scavengedScrapIds;
    public int scrapRequiredToEndGame;
    public bool endGameAvailable;

    /** NPCs**/
    //NPC Scholar2 (Harvel)
    public Vector3 NPCsch2Position;
    public int NPCsch2ConversationState;

    // Ink globals save blob (stores npcs_talked, met_child, lists, etc.)
    public string inkGlobalsStateJSON;

    //Stores Logs from Scrap Inspections
    public System.Collections.Generic.List<string> collectedLogIds;

    public GameData()
    {   /** Player **/
        playerPosition = Vector3.zero;
        playerRotation = Quaternion.identity;
        playerHealth = 100f;

        /**Translation**/
        uniqueNPCsTalkedTo = 0;
        dialogueOptionsAvailable = 1;

        /** VERA **/ 
        VERAPosition = Vector3.zero;

        /** Scrap **/
        scrapAmount = 0;
        scavengedScrapIds = new List<string>();
        scrapRequiredToEndGame = 20;
        endGameAvailable = false;

        /** NPCs **/
        //NPC Scholar2 (Harvel)
        NPCsch2Position = new Vector3(-5, 1, -4);
        NPCsch2ConversationState = 0;

        inkGlobalsStateJSON = "";

        //ScrapInspections
        collectedLogIds = new System.Collections.Generic.List<string>();
    }
}


