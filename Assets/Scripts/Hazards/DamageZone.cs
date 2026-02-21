using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 10f;

    private PlayerHealth player;
    private int overlapCount; // handles player having multiple colliders

    private void Reset()
    {
        // Safety: make sure the collider is set up as a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // single-player: keep the current player ref
        player = ph;
        overlapCount++;

        // First time we enter this hazard
        if (overlapCount == 1)
            HazardWarningUI.Instance?.EnterHazard(this);
    }

    private void OnTriggerStay(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // Teleport / spawn safety: if we appear inside without Enter firing
        if (player == null)
        {
            player = ph;
            overlapCount = 1;
            HazardWarningUI.Instance?.EnterHazard(this);
        }

        if (ph != player) return;

        player.TakeDamage(damagePerSecond * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        if (player == null || ph != player) return;

        overlapCount = Mathf.Max(0, overlapCount - 1);

        // Fully out of this hazard now
        if (overlapCount == 0)
        {
            HazardWarningUI.Instance?.ExitHazard(this);
            player = null;
        }
    }

    // Call this on respawn / scene load if you ever need a hard cleanup.
    public void ForceReset()
    {
        overlapCount = 0;
        player = null;
        HazardWarningUI.Instance?.ExitHazard(this);
    }


}