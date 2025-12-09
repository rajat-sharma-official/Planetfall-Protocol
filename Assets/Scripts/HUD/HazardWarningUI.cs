using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Controls the HazardPanel warning flashes (3 separate UI lights)
// and triggers the red breathing overlay AFTER the 3-second light sequence.
public class HazardWarningUI : MonoBehaviour
{
    public static HazardWarningUI Instance { get; private set; }

    [Header("Assign the 3 flashing UI GameObjects")]
    [SerializeField] private GameObject leftFlash;
    [SerializeField] private GameObject middleFlash;
    [SerializeField] private GameObject rightFlash;

    [Header("Red overlay (assign your RedHue root script)")]
    [SerializeField] private HazardRedHueUI redHue;

    [Header("Timing")]
    [SerializeField] private float flashesPerSecond = 3f; // 3 flashes/sec
    [SerializeField] private float stageSeconds = 1f;     // each stage duration (1 second)

    // Tracks which hazard objects the player is currently inside.
    // HashSet prevents double counting.
    private readonly HashSet<int> activeHazards = new HashSet<int>();

    private Coroutine sequenceCo;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Start clean
        SetFlashes(false, false, false);
        redHue?.StopBreathing();
    }

    /// Call when player enters a hazard zone.
    /// Pass the hazard MonoBehaviour (DamageZone uses 'this').
    public void EnterHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        int beforeCount = activeHazards.Count;
        activeHazards.Add(hazard.GetInstanceID());

        // If we were already inside at least one hazard, do nothing.
        // Only begin the sequence when transitioning 0 -> 1 hazards.
        if (beforeCount == 0 && activeHazards.Count == 1)
        {
            // Ensure red hue is off during the 3-second light sequence
            redHue?.StopBreathing();

            if (sequenceCo == null)
                sequenceCo = StartCoroutine(WarningSequence());
        }
    }

    // Call when player exits a hazard zone.
    // Pass the hazard MonoBehaviour (DamageZone uses 'this').
    public void ExitHazard(MonoBehaviour hazard)
    {
        if (hazard == null) return;

        activeHazards.Remove(hazard.GetInstanceID());

        // If no hazards remain, shut everything down.
        if (activeHazards.Count == 0)
        {
            if (sequenceCo != null)
            {
                StopCoroutine(sequenceCo);
                sequenceCo = null;
            }

            SetFlashes(false, false, false);
            redHue?.StopBreathing();
        }
    }

    // Used by DamageZone to delay damage until AFTER the 3-second light sequence.
    public float TotalWarningTime => stageSeconds * 3f;

    // Runs the 3-stage flash sequence, then starts red breathing if still in hazard.
    private IEnumerator WarningSequence()
    {
        // ON/OFF toggles happen twice per flash.
        // For 3 flashes/sec -> 6 toggles/sec -> halfPeriod = 1/(3*2)
        float halfPeriod = 1f / Mathf.Max(0.001f, flashesPerSecond * 2f);

        float t = 0f;
        bool on = false;

        bool leftActive = true;
        bool midActive = true;
        bool rightActive = true;

        SetFlashes(false, false, false);

        while (t < stageSeconds * 3f)
        {
            // If we left all hazards, stop immediately.
            if (activeHazards.Count == 0)
            {
                SetFlashes(false, false, false);
                redHue?.StopBreathing();
                sequenceCo = null;
                yield break;
            }

            // After 1s: Right permanently off
            if (t >= stageSeconds && rightActive)
            {
                rightActive = false;
                if (rightFlash != null) rightFlash.SetActive(false);
            }

            // After 2s: Middle permanently off
            if (t >= stageSeconds * 2f && midActive)
            {
                midActive = false;
                if (middleFlash != null) middleFlash.SetActive(false);
            }

            // Blink toggle
            on = !on;

            if (leftFlash != null)   leftFlash.SetActive(leftActive && on);
            if (middleFlash != null) middleFlash.SetActive(midActive && on);
            if (rightFlash != null)  rightFlash.SetActive(rightActive && on);

            yield return new WaitForSeconds(halfPeriod);
            t += halfPeriod;
        }

        // End of 3 seconds: all lights off
        SetFlashes(false, false, false);

        // Start breathing overlay ONLY if still inside at least one hazard
        if (activeHazards.Count > 0)
        {
            redHue?.StartBreathing();
        }

        sequenceCo = null;
    }

    //Force flash objects ON/OFF. Does NOT affect background seg objects.
    private void SetFlashes(bool left, bool mid, bool right)
    {
        if (leftFlash != null) leftFlash.SetActive(left);
        if (middleFlash != null) middleFlash.SetActive(mid);
        if (rightFlash != null) rightFlash.SetActive(right);
    }

    //clears redhue when no longer in hazard
    public void ForceClearAll()
    {
        // Clear all hazard tracking so we are "not in hazard"
        activeHazards.Clear();

        // Stop warning sequence if running
        if (sequenceCo != null)
        {
            StopCoroutine(sequenceCo);
            sequenceCo = null;
        }

        // Turn everything off
        SetFlashes(false, false, false);
        redHue?.StopBreathing();
    }
}
