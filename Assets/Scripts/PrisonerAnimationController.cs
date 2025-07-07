using UnityEngine;

public class PrisonerAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [Header("Animation Parameters")]
    [SerializeField] private string horizontalParam = "Horizontal";
    [SerializeField] private string verticalParam = "Vertical";
    [SerializeField] private string isRunningParam = "IsRunning";
    
    private Vector2 lastDirection = Vector2.down;
    
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateRunningAnimation(Vector2 direction, bool isRunning)
    {
        if (direction != Vector2.zero)
        {
            lastDirection = direction.normalized;
        }

        if (animator != null)
        {
            animator.SetFloat(horizontalParam, lastDirection.x);
            animator.SetFloat(verticalParam, lastDirection.y);
            animator.SetBool(isRunningParam, isRunning);
        }
    }
    
    public void SetRunning(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool(isRunningParam, isRunning);
        }
    }
}