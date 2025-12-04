using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private GameObject choiceCanvas;
    [SerializeField] private TextMeshProUGUI choice1Text;
    [SerializeField] private TextMeshProUGUI choice2Text;
    [SerializeField] private TextMeshProUGUI choice3Text;
    [SerializeField] private TextMeshProUGUI choice4Text;
  
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

    private void Start()
    {
        HideDialogue();
        HideChoices();
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
        Debug.Log($"[DialogueManager] AppendLine called with: {text}");

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
        if (choiceCanvas != null)
            choiceCanvas.SetActive(false);
        
    }

    public void ShowChoices(string choice1, string choice2, string choice3, string choice4)
    {
        if(choiceCanvas != null)
            choiceCanvas.SetActive(true);

        if(choice1Text != null)
            choice1Text.text = choice1;
        if(choice2Text != null)
            choice2Text.text = choice2;
        if(choice3Text != null)
            choice3Text.text = choice3;
        if(choice4Text != null)
            choice4Text.text = choice4;
    }

    public void Choice1Clicked()
    {
        OnChoiceClicked?.Invoke(1);
        HideChoices();
    }

    public void Choice2Clicked()
    {
        OnChoiceClicked?.Invoke(2);
        HideChoices();
    }

    public void Choice3Clicked()
    {
        OnChoiceClicked?.Invoke(3);
        HideChoices();
    }

    public void Choice4Clicked()
    {
        OnChoiceClicked?.Invoke(4);
        HideChoices();
    }
}