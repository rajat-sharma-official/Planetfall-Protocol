using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VERAFogPathController : MonoBehaviour
{
    [Header("References")]
    // player transform for player related stuff
    [SerializeField] private Transform playerTransform;
    // line renderer that draws the visible path
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Settings")]
    // small height offset so the line does not clip into the ground
    [SerializeField] private float lineHeightOffset = 0.1f;
    // how long the path stays visible after it shows up
    [SerializeField] private float visibleTime = 30f;
    // delay before the path starts showing
    [SerializeField] private float initialDelay = 10f;
    // how often the path gets updated while it is active
    [SerializeField] private float refreshInterval = 0.05f;
    // distance check for when the player is close enough to the target
    [SerializeField] private float targetReachedDistance = 1.0f;

    [Header("Flash")]
    // flashing interval to show up when the line is first added
    [SerializeField] private float flashInterval = 0.15f;
    // how many times the line flashes at the beginning
    [SerializeField] private int flashCount = 10;

    // keeps track of the current coroutine so it can be stopped safely
    private Coroutine activeRoutine;
    // stores the current target the path is pointing to
    private Transform currentTarget;

    private void Awake()
    {
        // auto find the player if it was not assigned in the inspector
        if (playerTransform == null)
        {
            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
            if (inventory != null)
                playerTransform = inventory.transform;
        }

        // auto grab the line renderer from this object if needed
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        // start with the line hidden so nothing shows on scene load
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.enabled = false;
        }
    }

    public void IlluminatePathTo(Transform target)
    {
        // stop early if the main references are missing
        if (playerTransform == null || target == null || lineRenderer == null)
            return;

        // ignore targets that are not active in the scene
        if (!target.gameObject.activeInHierarchy)
            return;

        // do not draw a path to scrap that was already collected
        if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(target.gameObject))
            return;

        currentTarget = target;

        // stop the old path routine before starting a new one
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        // clear any old line data before drawing the new path
        HidePath();
        activeRoutine = StartCoroutine(DrawPathRoutine(target));
    }

    private IEnumerator DrawPathRoutine(Transform target)
    {
        // wait a bit before the path appears
        yield return new WaitForSeconds(initialDelay);

        // cancel if the target changed or disappeared before drawing
        if (target == null || currentTarget != target)
        {
            HidePath();
            activeRoutine = null;
            yield break;
        }

        // stop if the target got disabled
        if (!target.gameObject.activeInHierarchy)
        {
            HidePath();
            activeRoutine = null;
            currentTarget = null;
            yield break;
        }

        // stop if the target was scavenged during the delay
        if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(target.gameObject))
        {
            HidePath();
            activeRoutine = null;
            currentTarget = null;
            yield break;
        }

        // try getting the first valid path before doing any flashing
        bool gotInitialPath = UpdatePath(target);
        // exit if a usable path could not be found
        if (!gotInitialPath)
        {
            activeRoutine = null;
            yield break;
        }

        // flash the line a few times so the player notices it
        for (int i = 0; i < flashCount; i++)
        {
            // stop flashing if the target changed
            if (target == null || currentTarget != target)
            {
                HidePath();
                activeRoutine = null;
                yield break;
            }

            // stop flashing if the target is no longer active
            if (!target.gameObject.activeInHierarchy)
            {
                HidePath();
                activeRoutine = null;
                currentTarget = null;
                yield break;
            }

            // stop flashing if the item was already picked up
            if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(target.gameObject))
            {
                HidePath();
                activeRoutine = null;
                currentTarget = null;
                yield break;
            }

            // refresh the line positions in case something moved
            UpdatePath(target);
            // turn the line on for the flash effect
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(flashInterval);

            // turn the line off to complete the flash
            lineRenderer.enabled = false;
            yield return new WaitForSeconds(flashInterval);
        }

        // leave the line on once the flashing part is done
        lineRenderer.enabled = true;

        // timer for how long the path should stay visible
        float timer = 0f;
        // keep updating the path until time runs out or the target becomes invalid
        while (timer < visibleTime && target != null)
        {
            // stop if some other target replaced this one
            if (currentTarget != target)
            {
                HidePath();
                activeRoutine = null;
                yield break;
            }

            // stop if the target gets disabled while showing
            if (!target.gameObject.activeInHierarchy)
            {
                HidePath();
                activeRoutine = null;
                currentTarget = null;
                yield break;
            }

            // stop if the target gets scavenged while showing
            if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(target.gameObject))
            {
                HidePath();
                activeRoutine = null;
                currentTarget = null;
                yield break;
            }

            // hide the line when the player is close enough to the target
            if (Vector3.Distance(playerTransform.position, target.position) <= targetReachedDistance)
            {
                HidePath();
                activeRoutine = null;
                currentTarget = null;
                yield break;
            }

            // keep recalculating the path in case the player moves
            UpdatePath(target);

            // count up based on the refresh speed
            timer += refreshInterval;
            yield return new WaitForSeconds(refreshInterval);
        }

        // hide the path when the routine finishes
        HidePath();
        activeRoutine = null;
        currentTarget = null;
    }

    private bool UpdatePath(Transform target)
    {
        // return false if the target is not the current one anymore
        if (target == null || currentTarget != target)
            return false;

        // return false if the target is inactive
        if (!target.gameObject.activeInHierarchy)
            return false;

        // return false if the target was already scavenged
        if (ScrapManager.Instance != null && ScrapManager.Instance.IsScavenged(target.gameObject))
            return false;

        // return false if the player already reached the target
        if (Vector3.Distance(playerTransform.position, target.position) <= targetReachedDistance)
            return false;

        // create a temporary navmesh path object to fill with corners
        NavMeshPath path = new NavMeshPath();
        // ask unity navmesh for a path from the player to the target
        bool foundPath = NavMesh.CalculatePath(
            playerTransform.position,
            target.position,
            NavMesh.AllAreas,
            path
        );

        // fail if unity could not make a valid path with enough points
        if (!foundPath || path.corners == null || path.corners.Length < 2)
            return false;

        // set how many points the line renderer needs to draw
        lineRenderer.positionCount = path.corners.Length;

        // copy each corner into the line renderer positions
        for (int i = 0; i < path.corners.Length; i++)
        {
            // grab each path corner one at a time
            Vector3 point = path.corners[i];
            // raise the point a little so the line is easier to see
            point.y += lineHeightOffset;
            // assign the adjusted point into the line renderer
            lineRenderer.SetPosition(i, point);
        }

        return true;
    }

    public void MarkTargetAsGrabbed(Transform grabbedTarget)
    {
        // nothing to do if the grabbed target is missing
        if (grabbedTarget == null)
            return;

        // only stop the path if the grabbed item is the current target
        if (currentTarget == grabbedTarget)
        {
            // stop the running coroutine so it does not keep updating
            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
                activeRoutine = null;
            }

            // clear the current target after it gets grabbed
            currentTarget = null;
            HidePath();
        }
    }

    public void HidePath()
    {
        // safety check in case the line renderer is missing
        if (lineRenderer == null)
            return;

        // remove all points and disable the line completely
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }
}