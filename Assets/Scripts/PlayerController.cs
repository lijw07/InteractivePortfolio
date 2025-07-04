using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float velPower = 0.96f;
    
    private Rigidbody2D rb;
    private PlayerInputController inputController;
    private PlayerAnimation playerAnimation;
    private Vector2 movement;
    private Vector2 lastMoveDirection = Vector2.down;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputController = GetComponent<PlayerInputController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.linearDamping = 0f;
    }

    void Update()
    {
        movement = inputController.MoveInput;
        
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement.normalized;
        }
        
        if (playerAnimation != null)
        {
            playerAnimation.SetMovement(movement, lastMoveDirection);
        }
    }
    
    void FixedUpdate()
    {
        Vector2 targetVelocity = movement * moveSpeed;
        Vector2 velocityDiff = targetVelocity - rb.linearVelocity;
        
        float accelRate = (Mathf.Abs(targetVelocity.magnitude) > 0.01f) ? acceleration : deceleration;
        
        Vector2 moveForce = Mathf.Pow(Mathf.Abs(velocityDiff.magnitude) * accelRate, velPower) * velocityDiff.normalized;
        
        rb.AddForce(moveForce, ForceMode2D.Force);
        
        if (movement.magnitude < 0.01f && rb.linearVelocity.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }
}