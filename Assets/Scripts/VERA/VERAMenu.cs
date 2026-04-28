using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;

public class VERAMenu : MonoBehaviour
{
    // reference to the vera menu ui panel shown on screen
    // this menu gets opened/closed based on player input
    [SerializeField] private GameObject VERAMenuPanel;

    [SerializeField] private VERAPopupController popupController;

    // tracks whether the vera menu is currently visible
    private bool isMenuOpen = false;

    // ui references for text fields and buttons
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TextMeshProUGUI responseText;
    [SerializeField] private Button submitButton;

    // preset quick-ask buttons
    [Header("Preset Buttons")]
    [SerializeField] private Button promptButton1;
    [SerializeField] private Button promptButton2;
    [SerializeField] private Button promptButton3;
    private VERAHTTPClient veraClient;

    private string pendingRawJson = null;
    private string pendingSystemResponse = null;

public static event Action VERAMenuActive;
public static event Action VERAMenuInactive;
private bool pauseMenuOpen;
private bool dialoguePanelOpen;
    private bool puzzleOpen;

    [System.Serializable]
    private class VeraInputPayload
    {
        public string text;
        public string objectName = "none";
        public string objectTag = "none";
        public float distance = 0f;
        public string currentZone = "None";
        public string zone = "None";
    }

    private readonly string[] repairReadyMessages =
    {

    "Heads up, Atlas — we have enough scrap to repair the ship. I’m also detecting a timed manual sequence once we begin, so do try to stay focused.",
    "Good news: enough scrap. Less good news: putting the ship back together appears to involve a timed repair sequence.",

    "We’ve got the scrap we need. Unfortunately, the ship seems to want proof of competence before it lets us leave.",
    "Repairs are possible now, Atlas. One complication: once we start, there’s a short window to finish the sequence.",

    "I’m seeing something I missed before. We have enough materials, but restoring the ship will require quick manual input once the repair starts.",
    "Atlas, we can repair the ship now. I should warn you — the restart procedure appears to be timed.",

    "We have enough scrap to begin repairs. We do not, however, have the luxury of doing them slowly.",
    "The ship is repairable now. Good. The actual repair sequence is time-sensitive, because apparently Aurelia was not done being difficult.",

    "We’ve reached the repair threshold. Once we begin at the crash site, you’ll need to complete the sequence before the system destabilizes.",
    "Enough scrap collected. One problem remains: the final repair step runs on a limited timer.",

    "I can confirm we have enough scrap. I can also confirm the ship expects a timed restoration sequence before it agrees to function.",
    "Repairs are available, Atlas. So is a brief and deeply inconvenient repair window.",

    "We have what we need to fix the ship. What we do not have is unlimited time once the sequence starts.",
    "The materials are sufficient. The ship’s recovery cycle, on the other hand, appears impatient.",

    "We can repair the ship now. Just be aware: once the procedure starts, you’ll need to put the pieces together quickly.",
    "Return to the crash site when you're ready. The repair itself is timed, and the ship will not wait for hesitation.",

    "Good. We have enough scrap. Bad: the final sequence only stays stable for a short time.",
    "The ship can be repaired now, Atlas. The systems are unstable enough that you’ll need to finish the sequence quickly.",

    "Heads up — enough scrap, yes. Easy repair, no.",
    "We’re ready to repair the ship. I’m reading a timed restart window, so this is the part where precision becomes important.",

    "We have enough scrap to leave this planet. First, however, the ship would like you to survive a timed repair sequence.",
    "Repairs are possible. The ship’s systems also appear to have developed a cruel sense of timing.",

    "Atlas, I’m detecting a short restoration window once repairs begin. We have enough scrap, but not enough time for indecision.",
    "The ship is ready for repairs. You should know now that the final sequence is timed, not automatic.",

    "We have enough scrap to start fixing the ship. Once we do, you’ll need to complete the repair sequence before the window closes.",
    "I didn’t flag this earlier, but the repair step is unstable. Enough scrap gets us started; speed is what gets us out.",

    };
    [System.Serializable]
    private class VeraOutputPayload
    {
        public string response;
    }

    private void OnEnable()
    {
        NPC_Base.OnConversationStart += DialoguePanelOpen;
        NPC_Base.OnConversationEnd += DialoguePanelClose;
        PauseMenu.PauseMenuActive += PauseMenuOpen;
        PauseMenu.PauseMenuInactive += PauseMenuClose;
        PuzzleManager.PuzzleOpened += PuzzleOpen;
        PuzzleManager.PuzzleClosed += PuzzleClose;
    }

    private void OnDisable()
    {
        NPC_Base.OnConversationStart -= DialoguePanelOpen;
        NPC_Base.OnConversationEnd -= DialoguePanelClose;
        PauseMenu.PauseMenuActive -= PauseMenuOpen;
        PauseMenu.PauseMenuInactive -= PauseMenuClose;
        PuzzleManager.PuzzleOpened -= PuzzleOpen;
        PuzzleManager.PuzzleClosed -= PuzzleClose;
    }

    private void Awake()
    {
        autoBindUI();

        if (popupController == null)
        {
            popupController = FindFirstObjectByType<VERAPopupController>();
        }
        veraClient = FindFirstObjectByType<VERAHTTPClient>();
        if (veraClient != null)
            veraClient.OnResponse += OnMessage;
    }


    void Start()
    {
        //hide VERA menu 
        // vera menu starts turned off so gameplay is uninterrupted at spawn
        if (VERAMenuPanel != null) VERAMenuPanel.SetActive(false);

        // attach button click handlers
        bindButtonEvents();

        // ensure input modules exist so ui receives events
        setupEventSystem();

        // make sure canvas has proper raycasting for clicking
        setupCanvasRaycaster();

        // disable raycast on the response label so typing feels normal
        if (responseText != null)
            responseText.raycastTarget = false;
    }

    //when key [q] is pressed
    // q toggles the vera menu open/closed
    public void OnVERAMenu(InputValue value)
    {
        // if the player is typing in the vera input field, do not toggle the menu
        if (IsTypingInUI())
            return;

        // menu is open -> close it
        if (isMenuOpen)
            closeMenu();
        else
            openMenu();
    }

    private void OnDestroy()
    {
        if (veraClient != null)
            veraClient.OnResponse -= OnMessage;
    }

        private void OnMessage(string rawJson)
    {
        pendingRawJson = rawJson;
    }

    void Update()
    {
        // allow pressing enter to submit when menu is open
        if (isMenuOpen &&
            (Keyboard.current.enterKey.wasPressedThisFrame ||
             Keyboard.current.numpadEnterKey.wasPressedThisFrame))
        {
            OnSubmit();
        }
        if (isMenuOpen && !string.IsNullOrEmpty(pendingRawJson))
        {
            string raw = pendingRawJson;
            pendingRawJson = null;

            try
            {
                var parsed = JsonUtility.FromJson<VeraOutputPayload>(raw);
                if (parsed != null && !string.IsNullOrWhiteSpace(parsed.response))
                    handleResponse(parsed.response);
                else
                    handleResponse(raw);
            }
            catch
            {
                handleResponse(raw);
            }
        }
    }

    private void PauseMenuOpen()
    {
        pauseMenuOpen = true;
    }

    private void PauseMenuClose()
    {
        pauseMenuOpen = false;
    }

    private void DialoguePanelOpen()
    {
        dialoguePanelOpen = true;
    }

    private void DialoguePanelClose()
    {
        dialoguePanelOpen = false;
    }

    private void PuzzleOpen()
    {
        puzzleOpen = true;
    }

    private void PuzzleClose()
    {
        puzzleOpen = false; 
    }

    public void closeMenu()
    {
        //hide the menu and resume player movement 
        // turns off the ui so player can play normally again
        if (VERAMenuPanel != null) VERAMenuPanel.SetActive(false);

        // menu state tracking
        isMenuOpen = false;

        // invoke menu close event
        VERAMenuInactive?.Invoke();

        //hide the cursor 
        // hides cursor + locks it for fps-style camera control
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void openMenu()
    {
        //don't let menu open if any other menus are open
        if(pauseMenuOpen || dialoguePanelOpen || puzzleOpen)
            return;

        if (popupController != null)
            popupController.HidePopup();

        //show the menu and pause player movement
        // makes the vera ui appear
        if (VERAMenuPanel != null) VERAMenuPanel.SetActive(true);

        // track open state
        isMenuOpen = true;

        // invoke menu open event
        VERAMenuActive?.Invoke();

        //show the cursor
        // needed for clicking buttons and typing
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // clear input so the field is ready for typing
        if (questionInput != null)
        {
            questionInput.text = "";
            questionInput.Select();
            questionInput.ActivateInputField();
        }

        // set default text for the response box when menu opens
        if (responseText != null)
        {
            bool showingSystemMessage = !string.IsNullOrWhiteSpace(pendingSystemResponse);

            if (showingSystemMessage)
            {
                responseText.text = pendingSystemResponse;
                pendingSystemResponse = null;
                responseText.margin = new Vector4(20, 35, 20, 20);
                responseText.textWrappingMode = TextWrappingModes.Normal;
            }
            else
            {
                responseText.text = "Ask VERA...";
                responseText.margin = Vector4.zero;
            }

            responseText.gameObject.SetActive(true);
            responseText.enabled = true;
            responseText.ForceMeshUpdate();
        }
    }


   public void OnSubmit()
{
    if (!isMenuOpen) return;

    string question = "What is this?";
    if (questionInput != null && !string.IsNullOrWhiteSpace(questionInput.text))
        question = questionInput.text;

    if (responseText != null)
    {
        responseText.text = "Analyzing...";
        responseText.ForceMeshUpdate();
    }

    if (veraClient == null) return;

    VERARaycast raycaster = FindFirstObjectByType<VERARaycast>();
    VeraInputPayload payload;

    if (raycaster != null)
    {
        string contextJson = raycaster.GetContext();

        payload = JsonUtility.FromJson<VeraInputPayload>(contextJson);

        if (payload == null)
            payload = new VeraInputPayload();

        if (!string.IsNullOrWhiteSpace(payload.zone) &&
            !payload.zone.Equals("None", StringComparison.OrdinalIgnoreCase))
        {
            payload.currentZone = payload.zone;
        }

        payload.text = question;
    }
    else
    {
        payload = new VeraInputPayload { text = question };
    }

    string finalJson = JsonUtility.ToJson(payload);
    veraClient.SendToVera(finalJson);
}

    private void handleResponse(string response)
    {
        // update text box with ai answer
        if (responseText != null)
        {
            responseText.text = response;
            responseText.gameObject.SetActive(true);
            responseText.enabled = true;
            responseText.margin = new Vector4(20, 35, 20, 20);
            responseText.textWrappingMode = TextWrappingModes.Normal;
            responseText.ForceMeshUpdate();
        }
    }

    public void GetRepairReadyMessage()
    {
        int randomIndex = UnityEngine.Random.Range(0, repairReadyMessages.Length);
        pendingSystemResponse = repairReadyMessages[randomIndex];
    }

    public void GetSystemMessage(string message)
    {
        pendingSystemResponse = message;
    }


    // quick preset questions
    public void AskWhereAmI() => AskSpecificQuestion("Where am I?");
    public void AskWhatNext() => AskSpecificQuestion("What should I do next?");
    public void AskAboutZone() => AskSpecificQuestion("Tell me about this zone");

    public void AskSpecificQuestion(string question)
    {
        // helper to set input field + submit automatically
        if (questionInput) questionInput.text = question;
        OnSubmit();
    }

    private void bindButtonEvents()
    {
        // hook the submit button
        if (submitButton != null)
        {
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(OnSubmit);
        }

        // hook preset button 1
        if (promptButton1 != null)
        {
            promptButton1.onClick.RemoveAllListeners();
            promptButton1.onClick.AddListener(AskWhereAmI);
        }

        // hook preset button 2
        if (promptButton2 != null)
        {
            promptButton2.onClick.RemoveAllListeners();
            promptButton2.onClick.AddListener(AskWhatNext);
        }

        // hook preset button 3
        if (promptButton3 != null)
        {
            promptButton3.onClick.RemoveAllListeners();
            promptButton3.onClick.AddListener(AskAboutZone);
        }
    }

    private void autoBindUI()
    {
        // automatically find panel if missing
        if (VERAMenuPanel == null)
        {
            Transform child = transform.Find("Panel");
            if (child != null) VERAMenuPanel = child.gameObject;
        }

        // bind within vera panel
        if (VERAMenuPanel != null)
        {
            // bind question input field
            if (questionInput == null)
            {
                Transform t = VERAMenuPanel.transform.Find("VERAInputQuestion");
                if (t != null) questionInput = t.GetComponent<TMP_InputField>();
                else questionInput = VERAMenuPanel.GetComponentInChildren<TMP_InputField>();
            }

            // bind response text
            if (responseText == null)
            {
                var allTransforms = GetComponentsInChildren<Transform>(true);
                Transform responsePanel = null;

                // try find panel object first
                foreach (var t in allTransforms)
                {
                    if (t.name == "VERAResponsePanel")
                    {
                        responsePanel = t;
                        break;
                    }
                }

                // fetch label from response panel
                if (responsePanel != null)
                    responseText = responsePanel.GetComponentInChildren<TextMeshProUGUI>();

                // try find response label directly
                if (responseText == null)
                {
                    foreach (var t in allTransforms)
                    {
                        if (t.name == "VERAResponseLabel")
                        {
                            responseText = t.GetComponent<TextMeshProUGUI>();
                            break;
                        }
                    }
                }

                // fallback: pick last viable text object
                if (responseText == null)
                {
                    var allTexts = VERAMenuPanel.GetComponentsInChildren<TextMeshProUGUI>();
                    if (allTexts.Length > 0)
                    {
                        for (int i = allTexts.Length - 1; i >= 0; i--)
                        {
                            var txt = allTexts[i];
                            if (txt.transform.parent.GetComponent<TMP_InputField>() != null) continue;
                            if (txt.gameObject.name.Contains("Placeholder")) continue;
                            if (txt.text.Contains("Question") || txt.text.Contains("MENU") || txt.text.Contains("VERA")) continue;

                            responseText = txt;
                            break;
                        }
                    }
                }
            }

            // bind submit button
            if (submitButton == null)
            {
                submitButton = findButtonByName(VERAMenuPanel.transform, "SubmitButton");
                if (submitButton == null) submitButton = findButtonByName(VERAMenuPanel.transform, "SendButton");

                if (submitButton == null) submitButton = findButtonByText("Send");
                if (submitButton == null) submitButton = findButtonByText("Submit");
            }

            // bind preset buttons
            if (promptButton1 == null) promptButton1 = findButtonByName(VERAMenuPanel.transform, "PromptButton1");
            if (promptButton2 == null) promptButton2 = findButtonByName(VERAMenuPanel.transform, "PromptButton2");
            if (promptButton3 == null) promptButton3 = findButtonByName(VERAMenuPanel.transform, "PromptButton3");
        }
        else
        {
            // fallback binding outside vera panel
            if (questionInput == null) questionInput = GetComponentInChildren<TMP_InputField>();
            if (responseText == null) responseText = GetComponentInChildren<TextMeshProUGUI>();

            if (submitButton == null)
            {
                submitButton = findButtonByText("Send");
                if (submitButton == null) submitButton = findButtonByText("Submit");
            }

            if (promptButton1 == null) promptButton1 = findButtonByName(transform, "PromptButton1");
            if (promptButton2 == null) promptButton2 = findButtonByName(transform, "PromptButton2");
            if (promptButton3 == null) promptButton3 = findButtonByName(transform, "PromptButton3");
        }

        // additional fallbacks for common names
        if (promptButton1 == null) promptButton1 = findButtonByText("Where am I");
        if (promptButton2 == null) promptButton2 = findButtonByText("What should I do");
        if (promptButton3 == null) promptButton3 = findButtonByText("Tell me about");
    }

    private Button findButtonByName(Transform root, string btnName)
    {
        // generic helper to locate button by object name
        var buttons = root.GetComponentsInChildren<Button>(true);
        foreach (var b in buttons)
            if (b.name == btnName) return b;
        return null;
    }

    private Button findButtonByText(string partialText)
    {
        // generic helper to locate button by label text
        var allButtons = GetComponentsInChildren<Button>(true);
        foreach (var btn in allButtons)
        {
            var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null && tmp.text.ToLower().Contains(partialText.ToLower())) return btn;

            var txt = btn.GetComponentInChildren<Text>();
            if (txt != null && txt.text.ToLower().Contains(partialText.ToLower())) return btn;
        }
        return null;
    }


    private bool IsTypingInUI()
    {
        if (questionInput != null && questionInput.isFocused)
            return true;

        return false;
    }

    private void setupEventSystem()
    {
        // ensures there is an event system so ui can receive clicks + input
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }
        else
        {
            var es = UnityEngine.EventSystems.EventSystem.current;
            var standalone = es.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            var uiInput = es.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

            // replace legacy event modules if needed
            if (standalone != null)
            {
                Destroy(standalone);
                es.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }
            else if (uiInput == null)
            {
                es.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }
        }
    }

    private void setupCanvasRaycaster()
    {
        // ensures canvas can detect pointer hits
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.GetComponent<GraphicRaycaster>() == null)
            canvas.gameObject.AddComponent<GraphicRaycaster>();
    }
}