using UnityEngine;
using System;

// Central place that owns DialogueVariables and persists Ink globals via your save system.
public class InkGlobalsManager : MonoBehaviour, IDataPersistence
{
    public static InkGlobalsManager Instance { get; private set; }

    [Header("Compiled globals.ink JSON")]
    [SerializeField] private TextAsset globalsInkJSON;

    public DialogueVariables DialogueVariables { get; private set; }

    public static event Action<int> OnNPCsTalkedChanged;
    public int NPCsTalked { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (globalsInkJSON == null)
        {
            Debug.LogError("[InkGlobalsManager] globalsInkJSON is not assigned!");
            return;
        }

        DialogueVariables = new DialogueVariables(globalsInkJSON);
        
        RefreshNPCsTalked();
    }
     private void OnEnable() 
    {
        NPC_Base.OnConversationEnd += RefreshNPCsTalked; // (update after each convo)
    } 

    private void OnDisable() 
    { 
        NPC_Base.OnConversationEnd -= RefreshNPCsTalked; 
    } 

    private void RefreshNPCsTalked() 
    { 
        int newValue = GetInt("npcs_talked"); 
        if (newValue != NPCsTalked) 
        { 
            NPCsTalked = newValue; 
            OnNPCsTalkedChanged?.Invoke(NPCsTalked); 
            Debug.Log($"[InkGlobalsManager] NPCsTalked updated: {NPCsTalked}"); 
        } 
    } 

    // Convenience getters for other Unity systems
    public int GetInt(string varName) => DialogueVariables.GetInt(varName);
    public bool GetBool(string varName) => DialogueVariables.GetBool(varName);

    public void LoadData(GameData data)
    {
        DialogueVariables.LoadVariablesFromJson(data.inkGlobalsStateJSON);
         RefreshNPCsTalked(); 
    }

    public void SaveData(ref GameData data)
    {
        data.inkGlobalsStateJSON = DialogueVariables.SaveVariablesToJson();
    }
}