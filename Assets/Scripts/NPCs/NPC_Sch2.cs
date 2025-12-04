using UnityEngine;
using System.Collections;
using Ink.Runtime;

public class NPC_Sch2 : MonoBehaviour, IInteractable, IDataPersistence
{
    enum ConversationState
    {
        not_talked_to, 
        talked_to
    }

    //NPC Vars
    private ConversationState conversationState = ConversationState.not_talked_to;
    private string npcName = "Harvel";
    
    //Ink Vars
    [SerializeField] private TextAsset inkJSONasset;
    private Story story;
    int choiceClicked;

    void Awake()
    {
        if(inkJSONasset != null)
        {
            story = new Story(inkJSONasset.text);
        } else
        {
            Debug.LogWarning($"Error: No ink story loaded for {npcName}");
        }
    }

    void OnEnable()
    {
        DialogueManager.OnChoiceClicked += ChoiceClick;
    }

    void OnDisable()
    {
        DialogueManager.OnChoiceClicked -= ChoiceClick;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.NPCsch2Position;
        this.conversationState = (ConversationState)data.NPCsch2ConversationState;
    }

    public void SaveData(ref GameData data)
    {
        data.NPCsch2Position = this.transform.position;
        data.NPCsch2ConversationState = (int)this.conversationState;
    }

    public void Interact()
    {
        StartConversation();
    }

    public string GetInteractionPrompt()
    {
        return $"Talk to {npcName}";
    }

    private void ChoiceClick(int choice)
    {
        choiceClicked = choice;
    }

    private void StartConversation()
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

    private IEnumerator RunStory(DialogueManager dialogueMgr)
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
            dialogueMgr.HideDialogue();
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
            dialogueMgr.HideChoices();
            
            // After making a choice, continue the story again
            StartCoroutine(RunStory(dialogueMgr));
            yield break;
        }
        else
        {
            // Story is done
            dialogueMgr.HideDialogue();
        }
    }
}
