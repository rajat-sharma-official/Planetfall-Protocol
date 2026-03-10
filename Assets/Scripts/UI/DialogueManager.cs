using TMPro;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button choice1Button;
    [SerializeField] private Button choice2Button;
    [SerializeField] private Button choice3Button;
    [SerializeField] private Button choice4Button;
    private int dialogueOptionsAvailable = 1;
    private static DialogueManager instance;
    public static event Action<int> OnChoiceClicked;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void OnEnable()
    {
        PlayerController.DialogueOptionsAvailableChanged += SetDialogueOptions;
    }

    private void OnDisable()
    {
        PlayerController.DialogueOptionsAvailableChanged -= SetDialogueOptions;
    }

    private void Start()
    {
        HideDialogue();
        HideChoices();
    }

    private void SetDialogueOptions(int options)
    {
        dialogueOptionsAvailable = options;
    }

    //Hide the panel and clear any text.
    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = string.Empty;
    }

    //Show the panel and overwrite the text.
    public void ShowDialogue(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = text;
    }

    //Show the panel and append a new line of text.
    public void AppendLine(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText == null){
            Debug.LogWarning("[DialogueManager] dialogueText is NULL!");
            return;
        }

        if (!string.IsNullOrEmpty(dialogueText.text))
            dialogueText.text += "\n";

        dialogueText.text += text;
    }

    public void HideChoices()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    public void ShowChoices(string choice1, string choice2, string choice3, string choice4)
    {
        Debug.Log($"ShowChoices called - choice1: '{choice1}', choice2: '{choice2}', choice3: '{choice3}', choice4: '{choice4}'");

        if(choicePanel != null)
            choicePanel.SetActive(true);

        // if(choice1Text != null)
        //     choice1Text.text = choice1;
        // if(choice2Text != null)
        //     choice2Text.text = choice2;
        // if(choice3Text != null)
        //     choice3Text.text = choice3;
        // if(choice4Text != null)
        //     choice4Text.text = choice4;

        string[] choices = {choice1, choice2, choice3, choice4};
        Button[] buttons = {choice1Button, choice2Button, choice3Button, choice4Button};

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue;
            
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
            buttons[i].interactable = i < dialogueOptionsAvailable;
        }
    }

    public void Choice1Clicked()
    {
        OnChoiceClicked?.Invoke(1);
    }

    public void Choice2Clicked()
    {
        OnChoiceClicked?.Invoke(2);
    }

    public void Choice3Clicked()
    {
        OnChoiceClicked?.Invoke(3);
    }

    public void Choice4Clicked()
    {
        OnChoiceClicked?.Invoke(4);
    }
}