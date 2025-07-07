using UnityEngine;
using System.Collections;

public class PrisonerWaypointRunner : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [SerializeField] private Transform[] runningPoints;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private bool loopPath = true;
    [SerializeField] private bool reverseOnEnd = false;
    
    private int currentWaypointIndex = 0;
    private bool isReversing = false;
    private PrisonerAnimationController animationController;
    
    void Start()
    {
        animationController = GetComponent<PrisonerAnimationController>();
        
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<PrisonerAnimationController>();
        }
        
        if (runningPoints.Length == 0)
        {
            Debug.LogError("No waypoints set for " + gameObject.name);
            enabled = false;
        }
    }
    
    void Update()
    {
        if (runningPoints.Length == 0)
            return;
            
        RunToWaypoint();
    }
    
    void RunToWaypoint()
    {
        Transform targetWaypoint = runningPoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        
        if (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, runSpeed * Time.deltaTime);
            
            if (animationController != null)
            {
                animationController.UpdateRunningAnimation(direction, true);
            }
        }
        else
        {
            OnReachWaypoint();
        }
    }
    
    void OnReachWaypoint()
    {
        if (!isReversing)
        {
            if (currentWaypointIndex >= runningPoints.Length - 1)
            {
                if (loopPath)
                {
                    if (reverseOnEnd)
                    {
                        isReversing = true;
                        currentWaypointIndex--;
                    }
                    else
                    {
                        currentWaypointIndex = 0;
                    }
                }
            }
            else
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            if (currentWaypointIndex <= 0)
            {
                isReversing = false;
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex--;
            }
        }
    }
    
    public void ResetPath()
    {
        currentWaypointIndex = 0;
        isReversing = false;
        
        if (runningPoints.Length > 0)
        {
            transform.position = runningPoints[0].position;
        }
        
        if (animationController != null)
        {
            animationController.SetRunning(false);
        }
    }
    
    public void SetRunSpeed(float newSpeed)
    {
        runSpeed = newSpeed;
    }
}