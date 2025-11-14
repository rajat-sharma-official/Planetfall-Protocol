using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    // Assign the TextMeshProUGUI inside the InteractionPanel.

    [SerializeField] private CanvasGroup canvasGroup; 

    void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        Hide(); // Panel starts hidden; PlayerController/HUDMgr will show it when needed.
    }

    //Writes the prompt text and shows the panel.
    public void Show(string message)
    {
        if (promptText != null) promptText.text = message;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(true);
    }

    //Hides the panel.
    public void Hide()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(false);
    }
}
