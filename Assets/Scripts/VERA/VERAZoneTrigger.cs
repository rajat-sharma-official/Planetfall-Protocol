using UnityEngine;

public class VERAZoneTrigger : MonoBehaviour
{
    [SerializeField] private string zoneName = "none";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        VERARaycast vera = other.GetComponentInChildren<VERARaycast>();
        if (vera == null)
            vera = FindFirstObjectByType<VERARaycast>();

        if (vera != null)
        {
            vera.setZone(zoneName);
            Debug.Log("(VERA Zone Trigger) Entered zone: " + zoneName);
        }
    }
}