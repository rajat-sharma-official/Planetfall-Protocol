using UnityEngine;
using UnityEngine.UI;

public class TranslationProgressBar : MonoBehaviour
{
    [SerializeField] private Image translationProgressBar;
    private int uniqueNPCsTalkedTo = 0;
    private int goalNPCsTalkedTo = 5;

    private void OnEnable()
    {
        PlayerController.UniqueNPCsTalkedToChanged += UniqueNPCsTalkedToChanged;
    }

    private void OnDisable()
    {
        PlayerController.UniqueNPCsTalkedToChanged -= UniqueNPCsTalkedToChanged;
    }

    private void Start()
    {
        UpdateProgressBar();
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
        translationProgressBar.fillAmount = (float)uniqueNPCsTalkedTo/goalNPCsTalkedTo;
        Debug.Log("Translation progress at " + (float)uniqueNPCsTalkedTo/goalNPCsTalkedTo + "%");
    }
}