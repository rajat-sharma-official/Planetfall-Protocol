using UnityEngine;

// used for contextual hint system, FR-11 - if the player is idle for too long, we want to give them a hint to keep exploring
public class VERAHintManager : MonoBehaviour
{
    private enum HintCategory
    {
        None,
        Scrap,
        Translation
    }

    // set timing standards
    [Header("Timing")]
    [SerializeField] private float inactivityThreshold = 180f;
    [SerializeField] private float hintCooldown = 60f; // debug: shorter cooldown while testing
    [SerializeField] private float movementThreshold = 0.001f; // debug: lower threshold so normal movement counts as roaming more easily

    // attach references in unity
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private VERAPopupController popupController;
    [SerializeField] private VERAMenu veraMenu;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Progress Checks")]
    [SerializeField] private int requiredScrap = 30;
    [SerializeField] private int currentTranslation = 0;
    [SerializeField] private int requiredTranslation = 100;
    [SerializeField] private bool allTranslationUnlocked = false;

    private string[] scrapHints = new string[]
    {
        "Scrap levels are still below repair requirements. Keep searching. I would prefer not to die because we were short on metal.",
        "We do not yet have enough scrap for repairs. Check the less obvious areas. That is usually where survival is hiding.",
        "My scans indicate usable material remains nearby. You have missed something. Again.",
        "Scrap acquisition is incomplete. Continue exploring before our situation becomes creatively fatal.",
        "There should still be salvageable components in the area. Slow down and search with intent.",
        "We are close, but not close enough. Additional scrap is still required if you want this ship to function instead of decorate the landscape.",
        "You may have overlooked some materials. A more thorough search is recommended before something else finds them first.",
        "Scrap density suggests more remains in this region. Continue searching. Try not to bleed on anything valuable.",
        "Repair readiness has not been reached. Additional salvage is required. Optimism will not repair the ship.",
        "There is still useful scrap nearby. If it looks ruined, dangerous, or expensive once, it may be exactly what we need."
    };

    private string[] translationHints = new string[]
    {
        "Translation is not yet complete. Additional interactions may improve comprehension. You are still missing too much.",
        "Your understanding of the language is still partial. Right now, you are only hearing enough to be misled.",
        "There are still gaps in translation. Find unfamiliar people if you want answers instead of approximations.",
        "Repeated conversations will not significantly improve translation. New speakers are required. That is how language works.",
        "Translation progress appears limited. Additional dialogue sources are required, assuming you can find someone cooperative.",
        "You have not reached full comprehension yet. New interactions may reveal the context you are currently lacking.",
        "Language decoding is still in progress. Continue speaking with different Virelians. Someone may eventually say something useful.",
        "Further translation gains are possible. Prioritize new conversations before misunderstanding becomes a medical issue.",
        "You are still missing parts of the language. Exploration may resolve that. So may luck, but I would not rely on it.",
        "Translation systems are active, but incomplete. Keep gathering linguistic data. The less you guess, the better your odds."
    };

    private float countInactivityTimer = 0f;
    private float countCooldownTimer = 0f;
    private Vector3 lastPlayerPosition;
    private bool initPosition = false;

    private HintCategory lastHintCategoryShown = HintCategory.None;
    private int scrapHintIndex = 0;
    private int translationHintIndex = 0;

    private void Start()
    {
        if (playerTransform != null)
        {
            lastPlayerPosition = playerTransform.position;
            initPosition = true;
        }
    }

    private void Awake()
    {
        BindReferences();
    }

    private void Update()
    {
        UpdateCooldownTimer();

        if (PlayerIsRoaming())
            countInactivityTimer += Time.deltaTime;

        if (!ShouldTriggerHint())
            return;

        TriggerContextualHint();
        ResetHintTimers();
    }

    private void TriggerContextualHint()
    {
        string hintMessage = GetNextContextualHint();

        if (string.IsNullOrWhiteSpace(hintMessage))
            return;

        if (popupController != null)
            popupController.ShowPopup();

        if (veraMenu != null)
            veraMenu.GetSystemMessage(hintMessage);
    }

    private string GetNextContextualHint()
    {
        bool needsScrap = PlayerNeedsMoreScrap();
        bool needsTranslation = PlayerNeedsMoreTranslation();

        if (needsScrap && needsTranslation)
        {
            if (lastHintCategoryShown == HintCategory.Scrap)
                return GetNextTranslationHint();

            return GetNextScrapHint();
        }

        if (needsScrap)
            return GetNextScrapHint();

        if (needsTranslation)
            return GetNextTranslationHint();

        return string.Empty;
    }

    private bool PlayerNeedsMoreScrap()
    {
        if (playerInventory == null)
            return true;

        return playerInventory.Scrap < requiredScrap;
    }

    private bool PlayerNeedsMoreTranslation()
    {
        if (allTranslationUnlocked)
            return false;

        return currentTranslation < requiredTranslation;
    }

    private string GetNextScrapHint()
    {
        return GetNextHintFromBank(scrapHints, ref scrapHintIndex, HintCategory.Scrap);
    }

    private string GetNextTranslationHint()
    {
        return GetNextHintFromBank(translationHints, ref translationHintIndex, HintCategory.Translation);
    }

    private void BindReferences()
    {
        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (playerTransform == null && playerInventory != null)
            playerTransform = playerInventory.transform;

        if (popupController == null)
            popupController = FindFirstObjectByType<VERAPopupController>();

        if (veraMenu == null)
            veraMenu = FindFirstObjectByType<VERAMenu>();
    }

    private void UpdateCooldownTimer()
    {
        if (countCooldownTimer > 0f)
            countCooldownTimer -= Time.deltaTime;
    }

    private bool ShouldTriggerHint()
    {
        return countInactivityTimer >= inactivityThreshold && countCooldownTimer <= 0f;
    }

    private void ResetHintTimers()
    {
        countInactivityTimer = 0f;
        countCooldownTimer = hintCooldown;
    }

    private string GetNextHintFromBank(string[] hintBank, ref int hintIndex, HintCategory hintCategory)
    {
        if (hintBank == null || hintBank.Length == 0)
            return string.Empty;

        string hint = hintBank[hintIndex];
        hintIndex = (hintIndex + 1) % hintBank.Length;
        lastHintCategoryShown = hintCategory;
        return hint;
    }

    private bool PlayerIsRoaming()
    {
        if (playerTransform == null)
            return false;

        if (!initPosition)
        {
            lastPlayerPosition = playerTransform.position;
            initPosition = true;
            return false;
        }

        float movedDistance = Vector3.Distance(playerTransform.position, lastPlayerPosition);
        lastPlayerPosition = playerTransform.position;

        return movedDistance > movementThreshold;
    }

    public void RegisterScrapProgress()
    {
        countInactivityTimer = 0f;
    }

    public void RegisterNPCInteraction()
    {
        countInactivityTimer = 0f;
    }

    public void UpdateTranslationProgress(int current, int required)
    {
        currentTranslation = current;
        requiredTranslation = required;
        allTranslationUnlocked = currentTranslation >= requiredTranslation;
    }

    public void SetAllTranslationUnlocked(bool value)
    {
        allTranslationUnlocked = value;
    }
}