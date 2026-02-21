using System.Collections.Generic;
using UnityEngine;

public class HazardWarningUI : MonoBehaviour
{
    public static HazardWarningUI Instance { get; private set; }

    [Header("Red overlay")]
    [SerializeField] private HazardRedHueUI redHue;

    // Track which hazards we are inside
    private readonly HashSet<int> activeHazards = new HashSet<int>();

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        redHue?.StopBreathing();
    }

    public void EnterHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        int before = activeHazards.Count;
        activeHazards.Add(hazard.GetInstanceID());

        // 0 -> 1 : start red hue
        if (before == 0 && activeHazards.Count == 1)
            redHue?.StartBreathing();
    }

    public void ExitHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        activeHazards.Remove(hazard.GetInstanceID());

        // 1 -> 0 : stop red hue
        if (activeHazards.Count == 0)
            redHue?.StopBreathing();
    }

    public void ForceClearAll()
    {
        activeHazards.Clear();
        redHue?.StopBreathing();
    }
}
