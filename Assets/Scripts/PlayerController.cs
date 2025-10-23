using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        //Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        ApplyGravity();
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

    //Called when WASD keys pressed
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //Called when mouse position moves
    public void OnRotation(InputValue value)
    {
        rotationInput = value.Get<Vector2>();
    }

    //Called when spacebar pressed
    public void OnJump(InputValue value)
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
