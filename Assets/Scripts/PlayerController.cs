using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    //Movement vars
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    
    //Camera vars
    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float maxLookAngle = 80f;
    private Vector2 rotationInput;
    private float cameraPitch = 0f;
    public static bool isPaused = false; 

    //Interaction vars
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("HUD")]
    [SerializeField] private HUDMgr hud; // <-- drag your HUDMgr here (on your Canvas)

    //cache target for Interact()
    private IInteractable current;

    private bool interactPressed = false;
    public static event Action OnScrapReset;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
        ApplyGravity();
        ShowInteractionPrompt();
        HandleInteraction();
    }

    public void LoadData(GameData data)
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        // Safely teleport the player using the CharacterController
        if (controller != null)
        {
            controller.enabled = false;                 // turn off to avoid interference
            transform.position = data.playerPosition;   // set saved position
            velocity = Vector3.zero;                    // clear any falling / movement
            controller.enabled = true;                  // turn it back on
        }
        else
        {
            // Fallback if for some reason there's no controller
            transform.position = data.playerPosition;
            velocity = Vector3.zero;
        }
    }
    
    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Horizontal rotation (left/right) - rotate player body
        transform.Rotate(Vector3.up, rotationInput.x * lookSensitivity);

        // Vertical rotation (up/down) - rotate camera only
        cameraPitch -= rotationInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleInteraction()
    {
        if (interactPressed)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

            if (hits.Length > 0)
            {
                Collider closest = GetClosestInteractable(hits);
                IInteractable interactable;
                if (!TryResolveInteractable(closest, out interactable)) interactable = null;

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }

            interactPressed = false;
        }
    }

    private void ShowInteractionPrompt()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        if (hits.Length > 0)
        {
            Collider closest = GetClosestInteractable(hits);

            if (closest != null && TryResolveInteractable(closest, out var interactable))
            {
                current = interactable;
                string prompt = interactable.GetInteractionPrompt();
                // TODO: Display prompt on UI
                Debug.Log(prompt);
                //show on HUD
                hud?.ShowInteractPrompt(prompt);
                return;
            }
        }
        current = null;
        hud?.HideInteractPrompt();
    }

    private Collider GetClosestInteractable(Collider[] colliders)
    {
        Collider closest = null;
        float minDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col;
            }
        }

        return closest;
    }
    
    //Robustly find IInteractable no matter where it sits relative to the collider
    private static bool TryResolveInteractable(Collider c, out IInteractable interactable)
    {
        interactable = null;
        if (c == null) return false;

        // Same object
        if (c.TryGetComponent<IInteractable>(out interactable))
            return true;

        // Parent (common: collider on child, script on root)
        interactable = c.GetComponentInParent<IInteractable>();
        if (interactable != null) return true;

        // Rigidbody owner (compound colliders)
        var rb = c.attachedRigidbody;
        if (rb != null)
        {
            if (rb.TryGetComponent<IInteractable>(out interactable)) return true;
            interactable = rb.GetComponentInParent<IInteractable>();
            if (interactable != null) return true;
        }

        // Children (less common)
        interactable = c.GetComponentInChildren<IInteractable>();
        return interactable != null;
    }  

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRotation(InputValue value)
    {
        rotationInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnInteract(InputValue value)
    {
        // Button actions return 1 on press, 0 on release
        bool isPressed = value.Get<float>() >= 0.5f;
        // Crosshair scale change
        hud?.SetCrosshairPressed(isPressed); 
         // Only run the interact once on press
        if (isPressed)
        interactPressed = true;
    }

    public void OnDEBUGResetScrap(InputValue value)
    {
        OnScrapReset?.Invoke();
    }
}
