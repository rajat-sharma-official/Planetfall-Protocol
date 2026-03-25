using System.Collections.Generic;
using UnityEngine;

public class HazardWarningUI : MonoBehaviour
{
    public static HazardWarningUI Instance { get; private set; }

    [Header("Red overlay")]
    [SerializeField] private HazardRedHueUI redHue;
    public HazardRedHueUI RedHue => redHue;

    [SerializeField] private GameObject warningBanner;

    // Track which hazards we are inside
    private readonly HashSet<int> activeHazards = new HashSet<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        RedHue?.StopBreathing();
        if(warningBanner != null)
        {
            warningBanner.SetActive(false);
        }
    }

    public void EnterHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        int before = activeHazards.Count;
        activeHazards.Add(hazard.GetInstanceID());

        // 0 -> 1 : start red hue
        if (before == 0 && activeHazards.Count == 1)
        {
            RedHue?.StartBreathing();
            if(warningBanner != null)
            {
                warningBanner.SetActive(true);
            }
        }
    }

    public void ExitHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        activeHazards.Remove(hazard.GetInstanceID());

        // 1 -> 0 : stop red hue
        if (activeHazards.Count == 0){

            RedHue?.StopBreathing();
            if(warningBanner != null)
            {
                warningBanner.SetActive(false);
            }
        }
    }

    public void ForceClearAll()
    {
        activeHazards.Clear();
        RedHue?.StopBreathing();
        if(warningBanner != null)
        {
            warningBanner.SetActive(false);
        }
    }
}
