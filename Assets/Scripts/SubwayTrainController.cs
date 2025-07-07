using UnityEngine;
using System.Collections;

public class SubwayTrainController : MonoBehaviour
{
    [Header("Train Movement Settings")]
    [SerializeField] private Transform waypoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float stoppingDistance = 5f;
    
    [Header("Station Settings")]
    [SerializeField] private float stationStopTime = 10f;
    [SerializeField] private float doorAnimationTime = 1f;
    
    [Header("Despawn Settings")]
    [SerializeField] private GameObject despawnTrigger;
    [SerializeField] private float respawnDelay = 5f;
    
    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string doorOpenTrigger = "OpenDoors";
    [SerializeField] private string doorCloseTrigger = "CloseDoors";
    
    private float currentSpeed = 0f;
    private float distanceFromWaypoint = 0f;
    private bool hasReachedWaypoint = false;
    
    public enum TrainState
    {
        ApproachingStation,
        Decelerating,
        AtStation,
        DoorsOpening,
        Waiting,
        DoorsClosing,
        LeavingStation,
        Accelerating
    }
    
    private TrainState currentState = TrainState.ApproachingStation;
    
    void Start()
    {
        if (waypoint == null)
        {
            Debug.LogError("No waypoint set for " + gameObject.name);
            enabled = false;
            return;
        }
        
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
        
        StartCoroutine(TrainCycle());
    }
    
    void Update()
    {
        if (waypoint == null) return;
        
        distanceFromWaypoint = Vector3.Distance(transform.position, waypoint.position);
        
        switch (currentState)
        {
            case TrainState.ApproachingStation:
            case TrainState.Decelerating:
                ApproachWaypoint();
                break;
            case TrainState.LeavingStation:
            case TrainState.Accelerating:
                LeaveStation();
                break;
        }
    }
    
    IEnumerator TrainCycle()
    {
        while (true)
        {
            yield return StartCoroutine(ApproachAndStop());
            yield return StartCoroutine(StationSequence());
            yield return StartCoroutine(LeaveAndRespawn());
        }
    }
    
    IEnumerator ApproachAndStop()
    {
        currentState = TrainState.ApproachingStation;
        hasReachedWaypoint = false;
        
        while (!hasReachedWaypoint)
        {
            yield return null;
        }
    }
    
    IEnumerator StationSequence()
    {
        currentState = TrainState.AtStation;
        
        yield return StartCoroutine(OpenDoors());
        yield return new WaitForSeconds(stationStopTime);
        yield return StartCoroutine(CloseDoors());
    }
    
    IEnumerator OpenDoors()
    {
        currentState = TrainState.DoorsOpening;
        
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(doorOpenTrigger);
        }
        
        yield return new WaitForSeconds(doorAnimationTime);
        currentState = TrainState.Waiting;
    }
    
    IEnumerator CloseDoors()
    {
        currentState = TrainState.DoorsClosing;
        
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(doorCloseTrigger);
        }
        
        yield return new WaitForSeconds(doorAnimationTime);
    }
    
    IEnumerator LeaveAndRespawn()
    {
        currentState = TrainState.LeavingStation;
        
        while (despawnTrigger != null && Vector3.Distance(transform.position, despawnTrigger.transform.position) > 0.5f)
        {
            yield return null;
        }
        
        StartCoroutine(DespawnAndRespawnSequence());
    }
    
    IEnumerator DespawnAndRespawnSequence()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        RespawnTrain();
    }
    
    void ApproachWaypoint()
    {
        if (distanceFromWaypoint <= 0.2f)
        {
            hasReachedWaypoint = true;
            currentSpeed = 0f;
            return;
        }
        
        if (distanceFromWaypoint <= stoppingDistance)
        {
            currentState = TrainState.Decelerating;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
        else
        {
            currentState = TrainState.ApproachingStation;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        
        Vector3 direction = (waypoint.position - transform.position).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }
    
    void LeaveStation()
    {
        currentState = TrainState.Accelerating;
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        
        Vector3 rightDirection = Vector3.right;
        transform.position += rightDirection * currentSpeed * Time.deltaTime;
    }
    
    void RespawnTrain()
    {
        currentSpeed = 0f;
        hasReachedWaypoint = false;
        currentState = TrainState.ApproachingStation;
        
        transform.position = spawnPoint.position;
    }
    
    void OnDrawGizmos()
    {
        if (waypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(waypoint.position, 1f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(waypoint.position, stoppingDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, waypoint.position);
        }
        
        if (spawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(spawnPoint.position, Vector3.one);
        }
        
        if (despawnTrigger != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(despawnTrigger.transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, despawnTrigger.transform.position);
        }
    }
}