using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;      // assign Dialogue Panel
    [SerializeField] private TextMeshProUGUI dialogueText;  // assign DialogueText
  
    private static DialogueManager instance;

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

}