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
    public float maxDistance = 50f;
    public float detectionRadius = 12f;
    [Range(0f, 180f)]
    public float detectionAngle = 35f;

    public LayerMask layerMask = Physics.DefaultRaycastLayers;
    public QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

    private string currentZone = "None";

    public string GetContext()
    {
        Collider[] targets = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            layerMask,
            queryTriggerInteraction);

        Collider bestTarget = null;
        float bestScore = float.MinValue;
        float bestDistance = 0f;

        foreach (var col in targets)
        {
            if (col.transform == transform)
                continue;

            Vector3 targetPoint = col.bounds.center;
            Vector3 toTarget = targetPoint - transform.position;
            float distance = toTarget.magnitude;

            if (distance > maxDistance || distance <= 0.01f)
                continue;

            Vector3 dir = toTarget / distance;
            float angle = Vector3.Angle(transform.forward, dir);

            if (angle > detectionAngle)
                continue;

            if (!HasLineOfSight(col, targetPoint, distance))
                continue;

            float alignmentScore = 1f - (angle / detectionAngle);   // 1 when centered
            float distanceScore = 1f - (distance / maxDistance);    // 1 when close
            float priorityScore = GetPriority(col.tag) / 10f;       // normalize

            float totalScore =
                alignmentScore * 0.5f +
                distanceScore * 0.2f +
                priorityScore * 0.3f;

            if (totalScore > bestScore)
            {
                bestScore = totalScore;
                bestTarget = col;
                bestDistance = distance;
            }
        }

        if (bestTarget != null)
        {
            return JsonUtility.ToJson(new VERAContext
            {
                objectName = bestTarget.name.Replace("(Clone)", "").Trim(),
                objectTag = bestTarget.tag,
                distance = (float)Math.Round(bestDistance, 1),
                priority = GetPriority(bestTarget.tag),
                zone = currentZone
            });
        }

        return JsonUtility.ToJson(new VERAContext
        {
            objectName = "None",
            objectTag = "None",
            distance = 0,
            priority = -1,
            zone = currentZone
        });
    }

    private bool HasLineOfSight(Collider target, Vector3 targetPoint, float distance)
    {
        Vector3 origin = transform.position;

        if (Physics.Raycast(
            origin,
            (targetPoint - origin).normalized,
            out RaycastHit hit,
            distance,
            layerMask,
            queryTriggerInteraction))
        {
            return hit.collider == target;
        }

        return false;
    }

    private float GetPriority(string objectTag)
    {
        switch (objectTag)
        {
            case "NPC": return 10;
            case "Hazard": return 8;
            case "Safety": return 8;
            case "ScrapMetal": return 6;
            case "Ground": return 0;
            default: return 1;
        }
    }

    public void setZone(string zoneName)
    {
        currentZone = zoneName;
    }
}