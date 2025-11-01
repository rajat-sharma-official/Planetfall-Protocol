using UnityEngine;
using Ink.Runtime;

public class BaseNPC : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Unnamed NPC";
    // Name of NPC: Initialized to "Unnamed NPC" but can be set in the Inspector for each NPC instance.
    // This helps identify which NPC the player is interacting with and makes debugging clearer.

    public TextAsset inkJSONAsset;
    // Ink story asset: Reference to the compiled Ink story file (.json) that contains this NPC’s dialogue.
    // Each NPC should have their own unique Ink JSON file assigned in the Inspector.

    private Story story;
    // Ink Story object: The runtime story instance created from the inkJSONAsset.
    // Handles the flow of dialogue lines, story progression, and available choices.

    private void Awake()
    {
        // Load the Ink story from the assigned JSON asset when the NPC initializes.
        // This prepares the story for when the player interacts with the NPC.
        if (inkJSONAsset != null)
        {
            story = new Story(inkJSONAsset.text);

        }
        else
        {
            Debug.LogWarning($"ERROR: No ink story loaded for {npcName}");
        }
    }

    // Called when the player uses the interact key while near the NPC to start the conversation.
    public void StartConversation()
    {
        if (story == null)
        {
            Debug.LogWarning($"ERROR: No ink story loaded for {npcName}");
            return;
        }
        story.ResetState();

        Debug.Log($"(BaseNPC) Conversation started with {npcName} !");

        if (story.canContinue)
            RunStory();
        else
            Debug.Log($"(BaseNPC) {npcName}'s story cannot continue yet.");

        Debug.Log($"(BaseNPC) Conversation finished with {npcName} !");
    }


    // Handles reading through the Ink story line-by-line.
    // Stops automatically when choices are reached or the story ends.
    private void RunStory()
    {
        // Continue reading as long as the Ink story has more content to display.
        while (story.canContinue)
        {
            // Retrieve the next line of text from the Ink story.
            string text = story.Continue().Trim();

            // Print the line to Unity’s console for debugging or simple dialogue output.
            Debug.Log($"(BaseNPC) [{npcName}] {text}");
        }

        // If the Ink story presents dialogue choices to the player, display them.
        if (story.currentChoices.Count > 0)
        {
            Debug.Log($"(BaseNPC) [{npcName}] Choices available:");

            // Loop through and display all available choices without selecting any.
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Debug.Log($"{i + 1}: {story.currentChoices[i].text.Trim()}");
            }
            // Note: Wait for playerInput here.
        }
        else
        {
            // If there are no further choices or text, the story has ended.
            Debug.Log($"(BaseNPC) {npcName} ended their story.");
        }
    }
}
