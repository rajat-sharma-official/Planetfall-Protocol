using UnityEngine;
using System.Collections;
using Ink.Runtime;

public class NPC_Scholar2 : NPC_Base
{
    protected override void Awake()
    {
        npcName = "Harvel";
        base.Awake();
    }

    public override void LoadData(GameData data)
    {
        this.transform.position = data.NPCsch2Position;
        this.conversationState = (ConversationState)data.NPCsch2ConversationState;
    }

    public override void SaveData(ref GameData data)
    {
        data.NPCsch2Position = this.transform.position;
        data.NPCsch2ConversationState = (int)this.conversationState;
    }
}

