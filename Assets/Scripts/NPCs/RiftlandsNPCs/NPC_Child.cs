using UnityEngine;

public class NPC_Child : NPC_Base
{
    protected override void Awake()
    {
        npcName = "Child";
        base.Awake();
    }

    public override void LoadData(GameData data) { }
    public override void SaveData(ref GameData data) { }
}