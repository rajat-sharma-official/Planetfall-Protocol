using UnityEngine;
using System.Collections;
using Ink.Runtime;
using System;

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
    public static event Action OnConversationStart;
    public static event Action OnConversationEnd;
    public static event Action FirstTalkedTo;
    protected bool pauseMenuOpen = false;
    protected bool veraMenuOpen = false;

    private DialogueVariables dialogueVariables;

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
        PauseMenu.PauseMenuActive += PauseMenuOpen;
        PauseMenu.PauseMenuInactive += PauseMenuClose;
        VERAMenu.VERAMenuActive += VERAMenuOpen;
        VERAMenu.VERAMenuInactive += VERAMenuClose;
    }

    protected virtual void OnDisable()
    {
        DialogueManager.OnChoiceClicked -= ChoiceClick;
        PauseMenu.PauseMenuActive -= PauseMenuOpen;
        PauseMenu.PauseMenuInactive -= PauseMenuClose;
        VERAMenu.VERAMenuActive -= VERAMenuOpen;
        VERAMenu.VERAMenuInactive -= VERAMenuClose;
    }

    protected void PauseMenuOpen()
    {
        pauseMenuOpen = true;
    }

    protected void PauseMenuClose()
    {
        pauseMenuOpen = false;
    }

    protected void VERAMenuOpen()
    {
        veraMenuOpen = true;
    }

    protected void VERAMenuClose()
    {
        veraMenuOpen = false;
    }

    public virtual void Interact()
    {
        if(pauseMenuOpen || veraMenuOpen)
            return;
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

            //story to shared globals (so met_child / npcs_talked persist across NPCs)
            if (dialogueVariables == null && InkGlobalsManager.Instance != null) 
                dialogueVariables = InkGlobalsManager.Instance.DialogueVariables; 

            dialogueVariables?.StartListening(story); 

            if(story.canContinue) 
            {
                StartCoroutine(RunStory(dialogueMgr));
                OnConversationStart?.Invoke();
            }
            else
                Debug.LogWarning($"Error: {npcName} story cannot continue");
        }
    }

    protected IEnumerator RunStory(DialogueManager dialogueMgr)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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
            //stop listening when the conversation fully ends
            dialogueVariables?.StopListening(story);
            
            // Story is done
            dialogueMgr.HideDialogue();
            dialogueMgr.HideChoices();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            OnConversationEnd?.Invoke();
            if(conversationState == ConversationState.not_talked_to) // First time having the conversation
                FirstTalkedTo?.Invoke();
            conversationState = ConversationState.talked_to;
        }
    }

    public abstract void LoadData(GameData data);
    public abstract void SaveData(ref GameData data);
}
