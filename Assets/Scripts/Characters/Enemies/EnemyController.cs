using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Enemy AI/Movement
public class EnemyController : CharacterBase
{

    // Patrol variables
    public Transform player;
    public Vector3 wayPoint1;
    public Vector3 wayPoint2;
    public Vector3 wayPoint3;
    public Vector3 wayPoint4;
    public Vector3[] waypoints;
    private int currentWayPointIndex = 0;
    private Vector3 currentPoint;
    public Vector3 targetWayPoint;

    // Enemy variables
    public float positionTolerance = 0.7f;
    public float detectionRadius = 2f;
    public float leashDistance = 5f;
    public float stoppingDistance = 0.8f;
    public float waitTime = 1f;
    private bool isWaiting = false;
    
    // State variables
    enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;


    void Start()
    {
        currentPoint = transform.position;
        wayPoint1 = new Vector3(currentPoint.x, currentPoint.y + 3f);
        wayPoint2 = currentPoint;
        wayPoint3 = new Vector3(currentPoint.x + 2f, currentPoint.y);
        wayPoint4 = currentPoint;
        waypoints = new Vector3[4];
        waypoints[0] = wayPoint1;
        waypoints[1] = wayPoint2;
        waypoints[2] = wayPoint3;
        waypoints[3] = wayPoint4;
        targetWayPoint = waypoints[currentWayPointIndex];
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                CheckForPlayer();
                break;
            case State.Chase:
                ChasePlayer();
                CheckLeashDistance();
                break;
            case State.Return:
                ReturnToSpawn();
                break;
        }
    }

    void Patrol()
    {
        if (!isWaiting)
        {
            if (Vector3.Distance(transform.position, targetWayPoint) <= positionTolerance)
            {
                StartCoroutine(WaitAtWaypoint());
                currentPoint = targetWayPoint;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetWayPoint) <= positionTolerance)
                {
                    StartCoroutine(WaitAtWaypoint());
                    currentPoint = targetWayPoint;
                }
            }
        }
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            currentState = State.Chase;
            Vector3 direction = (player.position - transform.position).normalized;
        }
    }

    void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius && distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed/2 * Time.deltaTime);
            // rb.linearVelocity = direction * moveSpeed;
        } else if (Vector3.Distance(transform.position, targetWayPoint) > leashDistance)
        {
            currentState = State.Return;
        }
         else
        {
            if (distanceToPlayer <= stoppingDistance)
            {
                transform.position = transform.position; // Stop moving
            // Attack logic here
            }
        }
    }

    void CheckLeashDistance()
    {
        if (Vector3.Distance(transform.position, targetWayPoint) > leashDistance)
        {
            Vector3 direction = (targetWayPoint - transform.position).normalized;
            currentState = State.Return;
        }
    }

    void ReturnToSpawn()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetWayPoint) < positionTolerance)
        {
            currentState = State.Patrol;
        }

    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Length;
        targetWayPoint = waypoints[currentWayPointIndex];
        isWaiting = false;
    }
    
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
