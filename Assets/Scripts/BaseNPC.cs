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
       
     // NEW: track simple dialogue state
    private bool dialogueOpen = false;
    private bool storyFinished = false;

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
        var dialogueMgr = DialogueManager.GetInstance();
        // --- CASE 1: Story already finished & panel open -> close it on next E ---
         if (dialogueOpen && storyFinished)
        {
            Debug.Log($"(BaseNPC) {npcName} dialogue closed by player.");
            dialogueMgr?.HideDialogue();
            dialogueOpen = false;
            return;
        }

        // --- CASE 2: Start (or restart) the conversation ---
        if (story == null)
        {
            Debug.LogWarning($"ERROR: No ink story loaded for {npcName}");
            return;
        }
        story.ResetState();
        storyFinished = false;

        Debug.Log($"(BaseNPC) Conversation started with {npcName} !");
        
        if (dialogueMgr != null)
        {
            // Clear & show panel
            dialogueMgr.ShowDialogue(string.Empty);
            dialogueOpen = true;
        }
        else
        {
            Debug.LogWarning("(BaseNPC) DialogueManager instance is NULL");
        }

        if (story.canContinue)
            RunStory(dialogueMgr);
        else
            Debug.Log($"(BaseNPC) {npcName}'s story cannot continue yet.");

        storyFinished = true;

        Debug.Log($"(BaseNPC) Conversation finished with {npcName} !");
        // Panel stays open until the next E press.
    }


    // Handles reading through the Ink story line-by-line.
    // Stops automatically when choices are reached or the story ends.
    private void RunStory(DialogueManager dialogueMgr)
    {
        // Continue reading as long as the Ink story has more content to display.
        while (story.canContinue)
        {
            // Retrieve the next line of text from the Ink story.
            string text = story.Continue().Trim();
            string formatted = $"{npcName}] {text}";
            // Print the line to Unity’s console for debugging or simple dialogue output.
            Debug.Log($"(BaseNPC) [{npcName}] {formatted}");

            //Send to Dialogue HUD
            dialogueMgr?.AppendLine(formatted);
        }

        // If the Ink story presents dialogue choices to the player, display them.
        if (story.currentChoices.Count > 0)
        {
            //showing choices as text lines(buttons later?)
            dialogueMgr?. AppendLine("");
            dialogueMgr?.AppendLine("Choices: ");
            Debug.Log($"(BaseNPC) [{npcName}] Choices available:");

            // Loop through and display all available choices without selecting any.
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                string choiceLine = $"{i + 1}: {story.currentChoices[i].text.Trim()}";
                Debug.Log(choiceLine);
                dialogueMgr?.AppendLine(choiceLine);
            }
            // Note: Wait for playerInput here.
        }
        else
        {
            string endLine = $"{npcName} ended their story.";
            Debug.Log($"(BaseNPC) {endLine}");
            dialogueMgr?.AppendLine(endLine);
        }
    }
}
