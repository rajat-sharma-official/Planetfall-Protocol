using UnityEngine;

// <summary>
//RestingZone heals the player gradually while they stand inside the trigger
public class RestingZone : MonoBehaviour
{
    [SerializeField] private float healPerSecond = 10f;

    // Overlap counter to handle multiple colliders on the player
    private int playerOverlapCount = 0;

    // Cached reference to the player health while inside
    private PlayerHealth currentPlayer;

    private void OnTriggerEnter(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // Track overlaps in case the player has multiple colliders
        playerOverlapCount++;
        currentPlayer = ph;
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerOverlapCount <= 0) return;
        if (currentPlayer == null) return;

        // Makes sure it is player
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null || ph != currentPlayer) return;

        // Heal smoothly over time
        currentPlayer.Heal(healPerSecond * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        playerOverlapCount = Mathf.Max(0, playerOverlapCount - 1);

        // Fully exited
        if (playerOverlapCount == 0)
        {
            currentPlayer = null;
        }
    }
}