using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; 

    [Header("Audio Sliders")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    [Header("Keybinds")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button veraMenuButton;
    [SerializeField] private Button sprintButton;
    [SerializeField] private GameObject rebindOverlay;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private InputActionAsset inputActions => playerInput.actions;

    void OnEnable()
    {
        PauseMenu.OpenSettingsMenuEvent += OpenSettingsMenu;
        PauseMenu.CloseSettingsMenuEvent += CloseSettingsMenu;
    }

    void OnDisable()
    {
        PauseMenu.OpenSettingsMenuEvent -= OpenSettingsMenu;
        PauseMenu.CloseSettingsMenuEvent -= CloseSettingsMenu;
    }

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        moveUpButton.onClick.AddListener(() => StartRebind("Movement", "Move", 1));
        moveDownButton.onClick.AddListener(() => StartRebind("Movement", "Move", 2));
        moveLeftButton.onClick.AddListener(() => StartRebind("Movement", "Move", 3));
        moveRightButton.onClick.AddListener(() => StartRebind("Movement", "Move", 4));
        jumpButton.onClick.AddListener(() => StartRebind("Movement", "Jump", 0));
        interactButton.onClick.AddListener(() => StartRebind("Movement", "Interact", 0));
        veraMenuButton.onClick.AddListener(() => StartRebind("Movement", "VERAMenu", 0));
        sprintButton.onClick.AddListener(() => StartRebind("Movement", "Sprint", 0));
    }

    void Start()
    {
        //Hide menu at game start
        settingsMenu.SetActive(false);

        //Load saved values, defaulting to 1 (full volume)
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value  = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value    = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        //Load saved keybind values
        rebindOverlay.SetActive(false);
        inputActions.Enable();
        LoadBindingOverrides();
        UpdateAllBindingLabels();
    }

    private void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    private void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
        masterVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        musicVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
        sfxVolumeText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    private void StartRebind(string actionMap, string actionName, int bindingIndex)
    {
        InputAction action = inputActions.FindActionMap(actionMap).FindAction(actionName);
    
        inputActions.FindActionMap(actionMap).Disable(); // Disable to avoid conflicts
        rebindOverlay.SetActive(true);

        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                inputActions.FindActionMap(actionMap).Enable(); // Re-enable after
                rebindOverlay.SetActive(false);
                UpdateAllBindingLabels();
                SaveBindingOverrides();
                rebindOperation.Dispose();
            })
            .OnCancel(operation =>
            {
                inputActions.FindActionMap(actionMap).Enable(); // Re-enable on cancel
                rebindOverlay.SetActive(false);
                rebindOperation.Dispose();
            })
            .Start();
    }

    private void UpdateAllBindingLabels()
    {
        Debug.Log("Updating binding labels");
        moveUpButton.GetComponentInChildren<TMP_Text>().text    = GetBindingName("Movement", "Move", 1);
        moveDownButton.GetComponentInChildren<TMP_Text>().text  = GetBindingName("Movement", "Move", 2);
        moveLeftButton.GetComponentInChildren<TMP_Text>().text  = GetBindingName("Movement", "Move", 3);
        moveRightButton.GetComponentInChildren<TMP_Text>().text = GetBindingName("Movement", "Move", 4);
        jumpButton.GetComponentInChildren<TMP_Text>().text      = GetBindingName("Movement", "Jump", 0);
        interactButton.GetComponentInChildren<TMP_Text>().text  = GetBindingName("Movement", "Interact", 0);
        veraMenuButton.GetComponentInChildren<TMP_Text>().text  = GetBindingName("Movement", "VERAMenu", 0);
        sprintButton.GetComponentInChildren<TMP_Text>().text    = GetBindingName("Movement", "Sprint", 0);
    }

    private string GetBindingName(string actionMap, string actionName, int bindingIndex)
    {
        Debug.Log("inputActions is null: " + (inputActions == null));
    
        var map = inputActions.FindActionMap(actionMap);
        Debug.Log("map is null: " + (map == null));
        
        var action = map?.FindAction(actionName);
        Debug.Log("action is null: " + (action == null));
        
        return InputControlPath.ToHumanReadableString(
            action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }

    private void SaveBindingOverrides()
    {
        PlayerPrefs.SetString("InputBindings", inputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    private void LoadBindingOverrides()
    {
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("InputBindings"));
        }
    }

    public void ResetAllBindings()
    {
        inputActions.RemoveAllBindingOverrides();
        SaveBindingOverrides();
        UpdateAllBindingLabels();
    }
}
