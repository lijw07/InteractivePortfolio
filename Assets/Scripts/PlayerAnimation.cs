using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float velocityThreshold = 0.3f;
    [SerializeField] private float inputThreshold = 0.1f;
    [SerializeField] private float animationDampTime = 0.1f;
    [SerializeField] private bool useVelocityBasedAnimation = true;
    [SerializeField] private float maxSpeed = 1f;
    
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 lastValidDirection = Vector2.down;
    private Vector2 smoothedDirection;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMovement(Vector2 inputMovement, Vector2 lastMoveDirection)
    {
        if (useVelocityBasedAnimation)
        {
            UpdateVelocityBasedAnimation(inputMovement, lastMoveDirection);
        }
        else
        {
            UpdateInputBasedAnimation(inputMovement, lastMoveDirection);
        }
    }
    
    private void UpdateVelocityBasedAnimation(Vector2 inputMovement, Vector2 lastMoveDirection)
    {
        Vector2 currentVelocity = rb.linearVelocity;
        bool isMoving = currentVelocity.magnitude > velocityThreshold;
        
        Vector2 targetDirection;
        
        if (inputMovement.magnitude > inputThreshold)
        {
            targetDirection = inputMovement.normalized;
            lastValidDirection = targetDirection;
        }
        else if (isMoving)
        {
            targetDirection = currentVelocity.normalized;
            
            if (currentVelocity.magnitude > velocityThreshold * 2f)
            {
                lastValidDirection = targetDirection;
            }
        }
        else
        {
            targetDirection = lastValidDirection;
        }
        
        smoothedDirection = Vector2.Lerp(smoothedDirection, targetDirection, Time.deltaTime * 10f);
        
        animator.SetFloat("moveX", smoothedDirection.x, animationDampTime, Time.deltaTime);
        animator.SetFloat("moveY", smoothedDirection.y, animationDampTime, Time.deltaTime);
        animator.SetBool("isMoving", isMoving);
    }
    
    private void UpdateInputBasedAnimation(Vector2 inputMovement, Vector2 lastMoveDirection)
    {

        bool isMoving = inputMovement.magnitude > inputThreshold;
        
        if (isMoving)
        {
            animator.SetFloat("moveX", inputMovement.x);
            animator.SetFloat("moveY", inputMovement.y);
        }
        else
        {
            animator.SetFloat("moveX", lastMoveDirection.x);
            animator.SetFloat("moveY", lastMoveDirection.y);
        }
        
        animator.SetBool("isMoving", isMoving);
    }
}