using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        RebuildVariablesDictionary(); 
    }

    private void RebuildVariablesDictionary() 
    { 
        variables = new Dictionary<string, Ink.Runtime.Object>(); 
        foreach (string name in globalVariablesStory.variablesState) 
        { 
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name); 
            variables[name] = value; 
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value); 
        } 
    } 

    public string SaveVariablesToJson() 
    { 
        if (globalVariablesStory == null) return ""; 
        VariablesToStory(globalVariablesStory); 
        return globalVariablesStory.state.ToJson(); 
    } 

    public void LoadVariablesFromJson(string jsonState) 
    { 
        if (globalVariablesStory == null) return; 
        if (string.IsNullOrEmpty(jsonState)) return; 
        globalVariablesStory.state.LoadJson(jsonState); 
        RebuildVariablesDictionary(); 
    } 

    public int GetInt(string name) 
    { 
        if (variables != null && variables.TryGetValue(name, out var obj) && obj is IntValue i) return i.value; 
        return 0; 
    } 

    public bool GetBool(string name) 
    { 
        if (variables != null && variables.TryGetValue(name, out var obj) && obj is BoolValue b) return b.value; 
        return false; 
    } 

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(name))
        {
            variables[name] = value;
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}