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

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private KeyCode skipTypingKey = KeyCode.Space;

    private int dialogueOptionsAvailable = 1;
    private static DialogueManager instance;
    public static event Action<int> OnChoiceClicked;

    private bool isTyping = false;
    private bool skipTypingRequested = false;

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

    private void Update()
    {
        if (isTyping && (Input.GetKeyDown(skipTypingKey) || Input.GetMouseButtonDown(0)))
        {
            skipTypingRequested = true;
        }
    }

    private void SetDialogueOptions(int options)
    {
        dialogueOptionsAvailable = options;
    }

    // Hide the panel and clear any text.
    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = string.Empty;

        isTyping = false;
        skipTypingRequested = false;
    }

    // Show the panel and overwrite the text.
    public void ShowDialogue(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = text;

        isTyping = false;
        skipTypingRequested = false;
    }

    // Keep this in case you still want instant appending somewhere else.
    public void AppendLine(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText == null)
        {
            Debug.LogWarning("[DialogueManager] dialogueText is NULL!");
            return;
        }

        if (!string.IsNullOrEmpty(dialogueText.text))
            dialogueText.text += "\n";

        dialogueText.text += text;
    }

    //type one line out
    public IEnumerator TypeLine(string text)
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText == null)
        {
            Debug.LogWarning("[DialogueManager] dialogueText is NULL!");
            yield break;
        }

        isTyping = true;
        skipTypingRequested = false;

        string baseText = dialogueText.text;

        if (!string.IsNullOrEmpty(baseText))
            baseText += "\n";

        dialogueText.text = baseText;

        string currentLine = "";

        foreach (char letter in text)
        {
            if (skipTypingRequested)
            {
                dialogueText.text = baseText + text;
                isTyping = false;
                skipTypingRequested = false;
                yield break;
            }

            currentLine += letter;
            dialogueText.text = baseText + currentLine;
            yield return new WaitForSeconds(typingSpeed);
        }

        dialogueText.text = baseText + text;
        isTyping = false;
        skipTypingRequested = false;
    }

    public void HideChoices()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    public void ShowChoices(string choice1, string choice2, string choice3, string choice4)
    {
        Debug.Log($"ShowChoices called - choice1: '{choice1}', choice2: '{choice2}', choice3: '{choice3}', choice4: '{choice4}'");

        if (choicePanel != null)
            choicePanel.SetActive(true);

        string[] choices = { choice1, choice2, choice3, choice4 };
        Button[] buttons = { choice1Button, choice2Button, choice3Button, choice4Button };

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