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
    private Vector3 spawnPoint;
    public Vector3 targetWayPoint;

    // Enemy variables
    public float positionTolerance = 0.2f;
    public float detectionRadius = 2f;
    public float leashDistance = 4.5f;
    public float stoppingDistance = 0.8f;
    public float waitTime = 1f;
    public float moveSpeed = 1f;
    public float chaseSpeed = 2f;
    private bool isWaiting = false;
    
    // State variables
    enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    void Start()
    {
        spawnPoint = transform.position;
        wayPoint1 = new Vector3(spawnPoint.x, spawnPoint.y + 3f);
        wayPoint2 = new Vector3(spawnPoint.x, spawnPoint.y - 0.3f);
        wayPoint3 = new Vector3(spawnPoint.x + 2f, spawnPoint.y);
        wayPoint4 = new Vector3(spawnPoint.x - 0.3f, spawnPoint.y);
        waypoints = new Vector3[4];
        waypoints[0] = wayPoint1;
        waypoints[1] = wayPoint2;
        waypoints[2] = wayPoint3;
        waypoints[3] = wayPoint4;
        targetWayPoint = waypoints[currentWayPointIndex];
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                ReturnToPatrol();
                break;
        }
    }

    void FixedUpdate()
    {
    
        if (!isWaiting)
        {
            if (currentState == State.Patrol)
            {
                MoveToTarget(moveSpeed, positionTolerance, targetWayPoint);
            }
            else if (currentState == State.Chase)
            {
                MoveToTarget(chaseSpeed, stoppingDistance, player.position);
            } else if (currentState == State.Return)
            {
                MoveToTarget(moveSpeed, positionTolerance, targetWayPoint);
            }
        }
    }

    void Patrol()
    {
        if (!isWaiting)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetWayPoint);
            if (distanceToTarget <= positionTolerance)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }
    }

    void CheckForPlayer()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        if (distanceToTarget <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    void ChasePlayer()
    {
        float distanceToTarget = Vector3.Distance(transform.position, spawnPoint);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToTarget > leashDistance)
        {
            currentState = State.Return;
        } else if (distanceToPlayer > detectionRadius)
        {
            currentState = State.Return;
        }
    }

    void CheckLeashDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, spawnPoint);
        if (distanceToTarget > leashDistance)
        {
            currentState = State.Return;
        }
    }

    void ReturnToPatrol()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetWayPoint);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius && distanceToTarget <= leashDistance)
        {
            Debug.Log("Player detected during return, switching to chase" + distanceToTarget);
            currentState = State.Chase;
        } else if (distanceToTarget <= positionTolerance)
         {
            currentState = State.Patrol;
         }
    }

    void MoveToTarget(float speed, float stoppingDistance, Vector3 target)
    {
    
        if (Vector3.Distance(transform.position, target) > stoppingDistance)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime));
        } else
        {
            rb.MovePosition(transform.position); // Stop moving
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
