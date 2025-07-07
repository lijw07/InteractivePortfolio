using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointWalker : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [SerializeField] private Transform[] walkingPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Wait Settings")]
    [SerializeField] private float waitTimeAtPoint1 = 12.5f;
    [SerializeField] private float waitTimeAtPoint7 = 12.5f;
    
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private bool hasReachedEnd = false;
    public PierAnimationController animationController;
    
    void Start()
    {
        animationController = GetComponent<PierAnimationController>();
        
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<PierAnimationController>();
        }
        
        if (walkingPoints.Length == 0)
        {
            Debug.LogError("No waypoints set for " + gameObject.name);
            enabled = false;
        }
    }
    
    void Update()
    {
        if (hasReachedEnd || isWaiting || walkingPoints.Length == 0)
            return;
            
        MoveToWaypoint();
    }
    
    void MoveToWaypoint()
    {
        Transform targetWaypoint = walkingPoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        
        if (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
            
            if (animationController != null)
            {
                animationController.UpdateMovementAnimation(direction, true);
            }
        }
        else
        {
            OnReachWaypoint();
        }
    }
    
    
    void OnReachWaypoint()
    {
        if (currentWaypointIndex == 1)
        {
            StartCoroutine(WaitAtWaypoint(waitTimeAtPoint1));
        }
        else if (currentWaypointIndex == 7)
        {
            StartCoroutine(WaitAtWaypoint(waitTimeAtPoint7));
        }
        else if (currentWaypointIndex == walkingPoints.Length - 1)
        {
            hasReachedEnd = true;
            if (animationController != null)
            {
                animationController.SetIdle();
            }
            Debug.Log(gameObject.name + " has reached the final waypoint.");
        }
        else
        {
            currentWaypointIndex++;
        }
    }
    
    IEnumerator WaitAtWaypoint(float waitTime)
    {
        isWaiting = true;
        
        if (animationController != null)
        {
            animationController.SetIdle();
        }
        
        Debug.Log(gameObject.name + " is waiting at waypoint " + (currentWaypointIndex + 1) + " for " + waitTime + " seconds.");
        
        yield return new WaitForSeconds(waitTime);
        
        isWaiting = false;
        currentWaypointIndex++;
        
        if (currentWaypointIndex >= walkingPoints.Length)
        {
            hasReachedEnd = true;
            Debug.Log(gameObject.name + " has completed the path.");
        }
    }
    
    public void ResetWalk()
    {
        currentWaypointIndex = 0;
        hasReachedEnd = false;
        isWaiting = false;
        
        if (walkingPoints.Length > 0)
        {
            transform.position = walkingPoints[0].position;
        }
    }
}