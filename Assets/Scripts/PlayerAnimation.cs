using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovement(Vector2 movement, Vector2 lastMoveDirection)
    {
        bool isMoving = movement.magnitude > 0.1f;
        
        if (isMoving)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
        }
        else
        {
            animator.SetFloat("moveX", lastMoveDirection.x);
            animator.SetFloat("moveY", lastMoveDirection.y);
        }
        
        animator.SetBool("isMoving", isMoving);
    }
}