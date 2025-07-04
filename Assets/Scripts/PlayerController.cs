using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private bool smoothMovement = true;
    [SerializeField] private float movementSmoothing = 0.1f;
    
    [Header("Animation")]
    [SerializeField] private bool flipSpriteHorizontally = true;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Vector2 smoothedMovement;
    private Vector2 movementVelocity;
    private bool isSprinting;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        HandleSpriteFlipping();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }
    
    private void HandleMovement()
    {
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        
        if (smoothMovement)
        {
            smoothedMovement = Vector2.SmoothDamp(smoothedMovement, moveInput, ref movementVelocity, movementSmoothing);
            rb.linearVelocity = smoothedMovement * currentSpeed;
        }
        else
        {
            rb.linearVelocity = moveInput * currentSpeed;
        }
    }
    
    private void HandleSpriteFlipping()
    {
        if (flipSpriteHorizontally && spriteRenderer != null && moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }
    
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
    
    public bool IsMoving()
    {
        return moveInput.magnitude > 0.1f;
    }
    
    public bool IsSprinting()
    {
        return isSprinting;
    }
}