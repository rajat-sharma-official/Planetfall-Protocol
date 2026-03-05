using UnityEngine;

public class NPC_Drayk : NPC_Base
{
    protected override void Awake()
    {
        npcName = "Drayk";
        base.Awake();

       //allows Ink to call giveScrap(amt)
        if (story != null) 
        {
            story.BindExternalFunction("giveScrap", (int amount) => GiveScrap(amount)); 
        }
    }

    private void GiveScrap(int amount) 
    {
        var inv = FindObjectOfType<PlayerInventory>(); 
        if (inv != null)
        {
            inv.AddScrap(amount); 
            Debug.LogWarning("Dialoguescrap added");
        }
        else
        {
            Debug.LogWarning("[NPC_Drayk] PlayerInventory not found; scrap not awarded."); 
        }
    }

    public override void LoadData(GameData data) { }
    public override void SaveData(ref GameData data) { }
}