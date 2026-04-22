using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour, IDataPersistence
{
    public static LogManager Instance { get; private set; }

    [System.Serializable]
    public class LogDef
    {
        public string id;      // e.g., "FRAG_01"

        [TextArea(6, 30)]
        public string text; 

        [Header("Figure")]
        public Sprite figureSprite;         // image shown in log UI
        [TextArea(1, 3)]
        public string figureCaption;       
    }

    [Header("All Logs")]
    [SerializeField] private List<LogDef> allLogs = new List<LogDef>();

    private readonly HashSet<string> collected = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool IsCollected(string id) => collected.Contains(id);

    public void Collect(string id)
    {
        if (!string.IsNullOrWhiteSpace(id))
            collected.Add(id);
    }

    public List<string> GetCollectedList()
    {
        List<string> list = new List<string>();
        foreach (var def in allLogs)
            if (def != null && collected.Contains(def.id))
                list.Add(def.id);
        return list;
    }

    public string GetText(string id)
    {
        foreach (var def in allLogs)
            if (def != null && def.id == id)
                return def.text;

        return "(Log text not found for id: " + id + ")";
    }

  
    public Sprite GetFigureSprite(string id)
    {
        foreach (var def in allLogs)
            if (def != null && def.id == id)
                return def.figureSprite;

        return null;
    }


    public string GetFigureCaption(string id)
    {
        foreach (var def in allLogs)
            if (def != null && def.id == id)
                return def.figureCaption;

        return "";
    }

    public void LoadData(GameData data)
    {
        collected.Clear();
        if (data.collectedLogIds == null) return;
        foreach (var id in data.collectedLogIds)
            collected.Add(id);
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectedLogIds == null)
            data.collectedLogIds = new List<string>();

        data.collectedLogIds.Clear();
        data.collectedLogIds.AddRange(collected);
    }
}