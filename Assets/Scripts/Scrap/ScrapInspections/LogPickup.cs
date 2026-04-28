using UnityEngine;

public class LogPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string logId = "FRAG_01";
    [SerializeField] private string interactKey = "E";
    [SerializeField] private bool hideAfterCollect = true;

    [SerializeField] private Collider logCollider;

    [Header("Unlock Condition")]
    [SerializeField] private GameObject coveringScrap;
    
     // assign the ScrapCover object

    private void Awake()
    {
        if (logCollider == null) logCollider = GetComponent<Collider>();
    }
    private void Start()
    {
        // If log already collected, optionally hide it
        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
        {
            if (hideAfterCollect) gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (logCollider == null) return;

        bool shouldEnable = IsUnlocked() && !(LogManager.Instance?.IsCollected(logId) ?? false);

        if (logCollider.enabled != shouldEnable)
            logCollider.enabled = shouldEnable;
    }

    private bool IsUnlocked()
    {
        // If no scrap assigned, treat as unlocked
        if (coveringScrap == null) return true;

        // If ScrapManager missing, fail open 
        if (ScrapManager.Instance == null) return true;

        // Unlock only after scrap is scavenged
        return ScrapManager.Instance.IsScavenged(coveringScrap);
    }

    public string GetInteractionPrompt()
    {
        if (!IsUnlocked())
            return ""; // no prompt until scrap is collected

        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
            return "Already collected";

        return $"Press {interactKey} to read & collect";
    }

    public void Interact()
    {
        if (!IsUnlocked()) return;

        if (LogManager.Instance != null && LogManager.Instance.IsCollected(logId))
        {
            LogUI.Instance?.OpenAndShow(logId); // allow reread
            return;
        }

        LogManager.Instance?.Collect(logId);
        LogUI.Instance?.OpenAndShow(logId);

        if (hideAfterCollect) gameObject.SetActive(false);
    }
}