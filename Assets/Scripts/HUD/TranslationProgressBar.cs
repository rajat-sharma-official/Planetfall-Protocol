using UnityEngine;
using UnityEngine.UI;

public class TranslationProgressBar : MonoBehaviour
{
    [SerializeField] private Image translationProgressBar;
    private int nextGoal = 3;
    private int uniqueNPCsTalkedTo = 0;

    private void OnEnable()
    {
        PlayerController.DialogueOptionsAvailableChanged += NextGoalChanged;
        PlayerController.UniqueNPCsTalkedToChanged += UniqueNPCsTalkedToChanged;
    }

    private void OnDisable()
    {
        PlayerController.DialogueOptionsAvailableChanged -= NextGoalChanged;
        PlayerController.UniqueNPCsTalkedToChanged -= UniqueNPCsTalkedToChanged;
    }

    private void Start()
    {
        UpdateProgressBar();
    }

    private void NextGoalChanged(int dialogueOptionsAvailable)
    {
        //Goals are 3, 6, 9
        //Dialogue options available are 1, 2, 3, 4
        switch(dialogueOptionsAvailable)
        {
            case 1:
                nextGoal = 3;
                break;
            case 2:
                nextGoal = 6;
                break;
            case 3:
                nextGoal = 9;
                break;
            case 4:
                nextGoal = 9;
                break;
            default:
                nextGoal = 9;
                break;
        }
    }

    private void UniqueNPCsTalkedToChanged(int numNPCs)
    {
        uniqueNPCsTalkedTo = numNPCs;
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if(translationProgressBar == null)
            return;
        translationProgressBar.fillAmount = (float)uniqueNPCsTalkedTo/nextGoal;
        Debug.Log("Translation progress at " + (float)uniqueNPCsTalkedTo/nextGoal + "%");
    }
}