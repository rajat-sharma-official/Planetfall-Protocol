using UnityEngine;
using System.Collections;
using Ink.Runtime;

public abstract class NPC_Base : MonoBehaviour, IInteractable, IDataPersistence
{
    protected enum ConversationState
    {
        not_talked_to, 
        talked_to
    }

    protected ConversationState conversationState = ConversationState.not_talked_to;
    protected string npcName;
    [SerializeField] protected TextAsset inkJSONasset;
    protected Story story;
    protected int choiceClicked;

    protected virtual void Awake()
    {
        if(inkJSONasset != null)
        {
            story = new Story(inkJSONasset.text);
        } else
        {
            Debug.LogWarning($"Error: No ink story loaded for {npcName}");
        }
    }

    protected virtual void OnEnable()
    {
        DialogueManager.OnChoiceClicked += ChoiceClick;
    }

    protected virtual void OnDisable()
    {
        DialogueManager.OnChoiceClicked -= ChoiceClick;
    }

    public virtual void Interact()
    {
        StartConversation();
    }

    public virtual string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }

    protected void ChoiceClick(int choice)
    {
        choiceClicked = choice;
    }

    protected void StartConversation()
    {
        //Case 0: Not talked to
        if(conversationState == ConversationState.not_talked_to)
        {
            var dialogueMgr = DialogueManager.GetInstance();

            if(story == null)
            {
                Debug.LogWarning($"Error: No ink story loaded for {npcName}");
                return;
            }
            story.ResetState();

            if(story.canContinue) 
                StartCoroutine(RunStory(dialogueMgr));
            else
                Debug.LogWarning($"Error: {npcName} story cannot continue");
        }
    }

    protected IEnumerator RunStory(DialogueManager dialogueMgr)
    {
        if(dialogueMgr != null) 
            dialogueMgr.ShowDialogue(string.Empty);
        else
            Debug.LogWarning("Error: DialogueManager instance not found");
        
        while(story.canContinue)
        {
            string text = story.Continue().Trim();
            dialogueMgr?.AppendLine(text);
        }

        // After continuing all we can, check if there are choices
        if(story.currentChoices.Count > 0)
        {
            Debug.Log("Reached choice point");
            choiceClicked = 0;
            string[] choices = new string[4];
            for(int i = 0; i < story.currentChoices.Count; i++)
            {
                string choice = story.currentChoices[i].text.Trim();
                choices[i] = choice;
            }
            dialogueMgr.ShowChoices(choices[0] ?? "", choices[1] ?? "", choices[2] ?? "", choices[3] ?? "");
            
            while(choiceClicked == 0)
            {
                yield return null;
            }
            
            story.ChooseChoiceIndex(choiceClicked - 1);
            
            // After making a choice, continue the story again
            StartCoroutine(RunStory(dialogueMgr));
            yield break;
        }
        else
        {
            // Story is done
            dialogueMgr.HideDialogue();
            dialogueMgr.HideChoices();
        }
    }

    public abstract void LoadData(GameData data);
    public abstract void SaveData(ref GameData data);
}
