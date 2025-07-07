using UnityEngine;

public class PierAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [Header("Animation Parameters")]
    [SerializeField] private string horizontalParam = "Horizontal";
    [SerializeField] private string verticalParam = "Vertical";
    [SerializeField] private string isMovingParam = "IsMoving";
    
    private Vector2 lastDirection = Vector2.down;
    
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void UpdateMovementAnimation(Vector2 direction, bool isMoving)
    {
        if (direction != Vector2.zero)
        {
            lastDirection = direction.normalized;
        }
        
        if (animator != null)
        {
            animator.SetFloat(horizontalParam, lastDirection.x);
            animator.SetFloat(verticalParam, lastDirection.y);
            animator.SetBool(isMovingParam, isMoving);
        }
    }
    
    public void SetIdle()
    {
        if (animator != null)
        {
            animator.SetBool(isMovingParam, false);
        }
    }
}