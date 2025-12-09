using System.Collections;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float extraGraceAfterWarning = 1.5f;

    // How many player colliders are overlapping this hazard right now.
    private int playerOverlapCount = 0;

    private bool canDamage = false;
    private Coroutine enableDamageCo;
    private PlayerHealth currentPlayer;

    private void OnTriggerEnter(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // If this is the first overlap with the player, treat as "entered hazard"
        playerOverlapCount++;

        // Always keep reference to the player (single-player game)
        currentPlayer = ph;

        if (playerOverlapCount == 1)
        {
            // Start the hazard warning UI sequence
            HazardWarningUI.Instance?.EnterHazard(this);

            // Reset damage gating
            canDamage = false;

            // Start the countdown until damage becomes active
            if (enableDamageCo != null)
                StopCoroutine(enableDamageCo);

            enableDamageCo = StartCoroutine(EnableDamageAfterWarning());
        }
    }

    //Waits for the warning sequence time, then allows damage.
    // If player leaves early, damage never turns on.
    private IEnumerator EnableDamageAfterWarning()
    {
        float wait = 3f; // fallback

        if (HazardWarningUI.Instance != null)
            wait = HazardWarningUI.Instance.TotalWarningTime; // usually 3 seconds

        yield return new WaitForSeconds(wait + extraGraceAfterWarning);

        // Only enable damage if player is still inside (overlap count > 0)
        if (playerOverlapCount > 0)
            canDamage = true;

        enableDamageCo = null;
    }

    private void OnTriggerStay(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // TELEPORT SAFETY:
        // If the player teleported into this trigger, OnTriggerEnter may never fire.
        // So if we are effectively "inside" but overlap count is 0, treat this as entry.
        if (playerOverlapCount == 0)
        {
            playerOverlapCount = 1;
            currentPlayer = ph;

            HazardWarningUI.Instance?.EnterHazard(this);

            canDamage = false;

            if (enableDamageCo != null)
                StopCoroutine(enableDamageCo);

            enableDamageCo = StartCoroutine(EnableDamageAfterWarning());
        }

        if (!canDamage) return;
        if (currentPlayer == null) return;
        if (ph != currentPlayer) return;

        currentPlayer.TakeDamage(damagePerSecond * Time.deltaTime);
    }


    private void OnTriggerExit(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // Player overlap decreased (could be multiple colliders)
        playerOverlapCount = Mathf.Max(0, playerOverlapCount - 1);

        // Only treat as "fully exited" when overlap count reaches 0
        if (playerOverlapCount == 0)
        {
            canDamage = false;
            currentPlayer = null;

            // Stop pending countdown coroutine if still running
            if (enableDamageCo != null)
            {
                StopCoroutine(enableDamageCo);
                enableDamageCo = null;
            }

            // Tell HUD we exited this hazard zone
            HazardWarningUI.Instance?.ExitHazard(this);
        }
    }

     //HARD RESET for respawn/teleport cases where triggers don't cleanly exit.
    //Call this when the player dies (before/after loading).
    public void ForceReset()
    {
        playerOverlapCount = 0;
        canDamage = false;
        currentPlayer = null;

        if (enableDamageCo != null)
        {
            StopCoroutine(enableDamageCo);
            enableDamageCo = null;
        }
    }

}
