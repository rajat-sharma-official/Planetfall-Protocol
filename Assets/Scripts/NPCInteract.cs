using UnityEngine;

[RequireComponent(typeof(BaseNPC))]
// This class is responsible for handling player interactions with an NPC. It implements the IInteractable interface, which defines the methods for interacting and getting interaction prompts.
// The NPCInteract class requires a BaseNPC component to function, ensuring that it has access to the necessary dialogue and story data for the NPC. When the player interacts with the NPC, it will trigger the StartConversation method of the BaseNPC, allowing the dialogue to begin.
public class NPCInteract : MonoBehaviour, IInteractable
{
    private BaseNPC npc;
    // Reference to BaseNPC.

    private void Awake()
    {
        // Cache the reference to the BaseNPC component on this GameObject. This allows us to call methods on the BaseNPC when the player interacts with this NPC.
        npc = GetComponent<BaseNPC>();
    }

    // Called when the player presses the interact key (e.g., E)
    public void Interact()
    {
        npc.StartConversation();
    }

    // Return interaction prompt (when player is nearby to talk to NPC)
    public string GetInteractionPrompt()
    {
        return $"Talk to {npc.npcName}";
    }
}
