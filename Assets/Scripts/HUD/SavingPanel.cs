using System.Collections;
using UnityEngine;

public class SavingPanel : MonoBehaviour
{
    [SerializeField] private GameObject savingPanel;
    [SerializeField] private float minimumVisibleTime = 1f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (savingPanel != null)
        {
            savingPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        DataPersistenceManager.onSaveStarted += ShowSaving;
        DataPersistenceManager.onSaveFinished += HideSaving;
    }

    private void OnDisable()
    {
        DataPersistenceManager.onSaveStarted -= ShowSaving;
        DataPersistenceManager.onSaveFinished -= HideSaving;
    }

    private void ShowSaving()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        if (savingPanel != null)
        {
            savingPanel.SetActive(true);
        }
    }

    private void HideSaving()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(minimumVisibleTime);

        if (savingPanel != null)
        {
            savingPanel.SetActive(false);
        }

        hideRoutine = null;
    }
}