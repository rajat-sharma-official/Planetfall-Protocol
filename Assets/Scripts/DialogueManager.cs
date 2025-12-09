using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; 
    [SerializeField] private TextMeshProUGUI dialogueText; 
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private TextMeshProUGUI choice1Text;
    [SerializeField] private TextMeshProUGUI choice2Text;
    [SerializeField] private TextMeshProUGUI choice3Text;
    [SerializeField] private TextMeshProUGUI choice4Text;
    
    [Header("HUD To Hide During Dialogue")]
    [SerializeField] private GameObject healthHUD;   
    [SerializeField] private GameObject hazardUI;
    [SerializeField] private GameObject HUD; //containing: scrap, Interact panel, Crosshair
    [SerializeField] private GameObject Compass;
  
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

        // Only show HUD if choices aren't up either
        if (choicePanel == null || !choicePanel.activeSelf)
        SetHudVisible(true);
    }

    //Show the panel and overwrite the text.
    public void ShowDialogue(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = text;

        //hides HUD/UI when showing Dialogue
        SetHudVisible(false);

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

        // Only show HUD if dialogue isn't up either
        if (dialoguePanel == null || !dialoguePanel.activeSelf)
        SetHudVisible(true);
    }

    public void ShowChoices(string choice1, string choice2, string choice3, string choice4)
    {
        Debug.Log($"ShowChoices called - choice1: '{choice1}', choice2: '{choice2}', choice3: '{choice3}', choice4: '{choice4}'");

        if(choicePanel != null)
            choicePanel.SetActive(true);

        if(choice1Text != null)
            choice1Text.text = choice1;
        if(choice2Text != null)
            choice2Text.text = choice2;
        if(choice3Text != null)
            choice3Text.text = choice3;
        if(choice4Text != null)
            choice4Text.text = choice4;

        //Hides UI/HUD when choices menus is shown
        SetHudVisible(false);
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

    private void SetHudVisible(bool visible)
{
    if (healthHUD != null) healthHUD.SetActive(visible);
    if (hazardUI != null) hazardUI.SetActive(visible);
    if (HUD != null) HUD.SetActive(visible);
    if (Compass != null) Compass.SetActive(visible);
}
}