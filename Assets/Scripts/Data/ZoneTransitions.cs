using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ZoneTrigger : MonoBehaviour
{
    [Header("Zone Info")]
    [SerializeField] private string zoneId = "Virelia";

    [SerializeField] private bool debugAutosaveOnEnter = true;

    [SerializeField] private UnityEvent onPlayerEnterZone;
    [SerializeField] private UnityEvent onPlayerExitZone;

    private PlayerHealth playerHealth;
    private int overlapCount;
    private bool playerInside;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private bool TryGetPlayer(Collider other, out PlayerHealth ph)
    {
        ph = other.GetComponentInParent<PlayerHealth>();
        return ph != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TryGetPlayer(other, out var ph)) return;

        playerHealth = ph;
        overlapCount++;

        if (!playerInside && overlapCount == 1)
        {
            EnterZone();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!TryGetPlayer(other, out var ph)) return;

        if (!playerInside)
        {
            playerHealth = ph;
            overlapCount = Mathf.Max(overlapCount, 1);
            EnterZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!TryGetPlayer(other, out var ph)) return;
        if (playerHealth == null || ph != playerHealth) return;

        overlapCount = Mathf.Max(0, overlapCount - 1);

        if (playerInside && overlapCount == 0)
        {
            playerInside = false;
            playerHealth = null;

            Debug.Log($"Player exited zone: {zoneId}");
            onPlayerExitZone?.Invoke();
        }
    }

    private void EnterZone()
    {
        playerInside = true;

        Debug.Log($"Player entered zone: {zoneId}");

        /*
        if (debugAutosaveOnEnter && DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
            Debug.Log($"Autosaved on entering zone: {zoneId}");
        }
        */

        onPlayerEnterZone?.Invoke();
    }

    private void OnDisable()
    {
        overlapCount = 0;
        playerInside = false;
        playerHealth = null;
    }
}