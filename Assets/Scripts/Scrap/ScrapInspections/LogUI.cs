using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public static event System.Action LogUIOpened;  
    public static event System.Action LogUIClosed;  
    public static LogUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI logText;

    [Header("Figure UI ")]
    [SerializeField] private Image figureImage;               
    [SerializeField] private TextMeshProUGUI figureCaptionText; 
    

    private List<string> collectedIds = new List<string>();
    private int index = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (panel != null) panel.SetActive(false);
    }

    public void OpenAndShow(string logId)
    {
        collectedIds = LogManager.Instance.GetCollectedList();
        index = Mathf.Max(0, collectedIds.IndexOf(logId));
        Open();
        Render();
    }

    public void OpenLogbook()
    {
        collectedIds = LogManager.Instance.GetCollectedList();
        index = Mathf.Clamp(index, 0, Mathf.Max(0, collectedIds.Count - 1));
        Open();
        Render();
    }

    public void Close()
    {
        if (panel != null) panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        
        LogUIClosed?.Invoke();

        Cursor.visible = false;
        Time.timeScale = 1f;
        
    }

    public void Next()
    {
        if (collectedIds.Count == 0) return;
        index = (index + 1) % collectedIds.Count;
        Render();
    }

    public void Prev()
    {
        if (collectedIds.Count == 0) return;
        index = (index - 1 + collectedIds.Count) % collectedIds.Count;
        Render();
    }

    private void Open()
    {

        if (panel != null) panel.SetActive(true);

        LogUIOpened?.Invoke();

        Cursor.lockState = CursorLockMode.None;
       
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void Render()
    {
        if (collectedIds.Count == 0)
        {
            if (logText != null) logText.text = "No logs collected yet.";

            if (figureImage != null) figureImage.gameObject.SetActive(false);

            return;
        }

        string id = collectedIds[index];

        if (logText != null) logText.text = LogManager.Instance.GetText(id);


        //figure
        Sprite s = LogManager.Instance.GetFigureSprite(id);
        string cap = LogManager.Instance.GetFigureCaption(id);

        if (figureImage != null)
        {
            bool hasSprite = (s != null);
            figureImage.gameObject.SetActive(hasSprite);
            if (hasSprite) figureImage.sprite = s;
        }

        if (figureCaptionText != null)
            figureCaptionText.text = cap ?? "";
    }
}