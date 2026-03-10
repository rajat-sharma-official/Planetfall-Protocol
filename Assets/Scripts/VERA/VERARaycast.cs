using UnityEngine;
using System;

[Serializable]
public class VERAContext
{
    public string objectName;
    public string objectTag;
    public float distance;
    public float priority;
    public string zone;

}
public class VERARaycast : MonoBehaviour
{
    public float maxDistance = 50;
    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

    private string currentZone = "None";

    public string GetContext()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, maxDistance, layerMask);
        Collider bestTarget = null;
        float bestDot = 0.5f; 

        foreach (var col in targets)
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dir);

            if (dot > bestDot)
            {
                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxDistance))
                {
                    if (hit.collider == col)
                    {
                        bestDot = dot;
                        bestTarget = col;
                    }
                }
            }
        }

        if (bestTarget != null)
        {
            return JsonUtility.ToJson(new VERAContext
            {
                objectName = bestTarget.name.Replace("(Clone)", "").Trim(),
                objectTag = bestTarget.tag,
                distance = (float)System.Math.Round(Vector3.Distance(transform.position, bestTarget.transform.position), 1),
                priority = GetPriority(bestTarget.tag),
                zone = currentZone
            });
        }

        return JsonUtility.ToJson(new VERAContext { objectName = "None", objectTag = "None", distance = 0, priority = -1, zone = currentZone });
    }

    private float GetPriority(string objectTag)
    {
        switch (objectTag)
        {
            case "NPC":
                return 10;
            case "Hazard":
                return 8;
            case "Safety":
                return 8;
            case "ScrapMetal":
                return 6;
            case "Ground":
                return 0;
            default:
                return 1;
        }
    }

    public void setZone(string zoneName)
    {
        currentZone = zoneName;
    }

    private void Update()
    {
        // Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
    }
}