using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    void Awake()
    {
        HideInteractionPrompt();
    }

    public void ShowInteractionPrompt(string message)
    {
        if (promptText != null) promptText.text = message;

        gameObject.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        gameObject.SetActive(false);
    }
}
