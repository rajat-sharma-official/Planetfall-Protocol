using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string logId = "FRAG_01";
    [SerializeField] private string interactKey = "E";
    [SerializeField] private bool hideAfterCollect = true;

    private void Start()
    {
        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
            if (hideAfterCollect) gameObject.SetActive(false);
    }

    public string GetInteractionPrompt()
    {
        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
            return "Already collected";

        return $"Press {interactKey} to read & collect";
    }

    public void Interact()
    {
        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
        {
            // Optional: allow reread in-world
            LogUI.Instance?.OpenAndShow(logId);
            return;
        }

        LogManager.Instance?.Collect(logId);
        LogUI.Instance?.OpenAndShow(logId);

        if (hideAfterCollect) gameObject.SetActive(false);
    }
}