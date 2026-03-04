using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour, IDataPersistence
{
    //Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool jumpPressed = false;
    
    //Camera
    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float maxLookAngle = 80f;
    private Vector2 rotationInput;
    private float cameraPitch = 0f;
    
    //Pause/stop movement
    public bool pauseMenuOpen = false; 
    public bool veraMenuOpen = false;
    public bool dialoguePanelOpen = false;

    //Interaction
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    public static event Action<string?> OnInteractionPromptChanged;
    private IInteractable current;
    private bool interactPressed = false;

    //Translation Mechanic 
    private int uniqueNPCsTalkedTo = 0;
    private readonly int[] translationUnlockThresholds = {3, 6, 9}; //CHANGE BASED ON TOTAL # NPCS 
    private int dialogueOptionsAvailable = 1;
    public static event Action<int> DialogueOptionsAvailableChanged;

    //Debug
    public static event Action OnScrapReset;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueOptionsAvailableChanged?.Invoke(dialogueOptionsAvailable);
    }

    void OnEnable()
    {
        NPC_Base.OnConversationStart += DialoguePanelOpen;
        NPC_Base.OnConversationEnd += DialoguePanelClose;
        NPC_Base.FirstTalkedTo += NewNPCTalkedTo;
        PauseMenu.PauseMenuActive += PauseMenuOpen;
        PauseMenu.PauseMenuInactive += PauseMenuClose;
        VERAMenu.VERAMenuActive += VERAMenuOpen;
        VERAMenu.VERAMenuInactive += VERAMenuClose;
    }

    void OnDisable()
    {
        NPC_Base.OnConversationStart -= DialoguePanelOpen;
        NPC_Base.OnConversationEnd -= DialoguePanelClose;
        NPC_Base.FirstTalkedTo -= NewNPCTalkedTo;
        PauseMenu.PauseMenuActive -= PauseMenuOpen;
        PauseMenu.PauseMenuInactive -= PauseMenuClose;
        VERAMenu.VERAMenuActive -= VERAMenuOpen;
        VERAMenu.VERAMenuInactive -= VERAMenuClose;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
        if(pauseMenuOpen || veraMenuOpen || dialoguePanelOpen)
            return;
        HandleMovement();
        HandleRotation();
        HandleJump();
        ShowInteractionPrompt();
        HandleInteraction();
        HandleTranslationUnlock();

        if(moveInput.magnitude > 0.1f)
        {
            FindObjectOfType<AudioManager>().Play("Walking");
        } else
        {
            FindObjectOfType<AudioManager>().Stop("Walking");
        }
    }

    private void PauseMenuOpen()
    {
        pauseMenuOpen = true;
    }

    private void PauseMenuClose()
    {
        pauseMenuOpen = false;
    }

    private void VERAMenuOpen()
    {
        veraMenuOpen = true;
    }

    private void VERAMenuClose()
    {
        veraMenuOpen = false;
    }

    private void DialoguePanelOpen()
    {
        dialoguePanelOpen = true;
    }

    private void DialoguePanelClose()
    {
        dialoguePanelOpen = false;
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

    private void HandleJump()
    {
        if (controller.isGrounded && jumpPressed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }
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

    private void HandleTranslationUnlock()
    {
        int next = dialogueOptionsAvailable - 1;

        if(next < translationUnlockThresholds.Length && uniqueNPCsTalkedTo >= translationUnlockThresholds[next])
        {
            dialogueOptionsAvailable++;
            DialogueOptionsAvailableChanged?.Invoke(dialogueOptionsAvailable);
        }
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
                OnInteractionPromptChanged?.Invoke(prompt);
                return;
            }
        }
        current = null;
        OnInteractionPromptChanged?.Invoke(null);
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
        jumpPressed = true;
    }

    public void OnInteract(InputValue value)
    { 
        interactPressed = true;
    }

    public void OnDEBUGResetScrap(InputValue value)
    {
        OnScrapReset?.Invoke();
    }

    private void NewNPCTalkedTo()
    {
        uniqueNPCsTalkedTo++;
    }
}
