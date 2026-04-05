using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu; 
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

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
}
