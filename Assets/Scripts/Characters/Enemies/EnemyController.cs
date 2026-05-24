using System.Collections;
using UnityEngine;

// EnemyController handles all behavior for enemy characters, including patrolling between waypoints, chasing the player when detected, returning to patrol route if player escapes, and attacking the player when in range. Inherits from CharacterBase which provides basic health and damage functionality.
public class EnemyController : CharacterBase
{

    [Header("Patrol Variables")]
    private Transform player;
    private Vector3[] waypoints;
    private int currentWayPointIndex = 0;
    private Vector3 spawnPoint;
    private Vector3 targetWayPoint;

    [Header("Movement Variables")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float leashDistance = 4.5f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float stoppingDistance = 0.8f;
    private float positionTolerance = 0.2f;
    private bool isWaiting;
    
    [Header("Combat Variables")]
    [SerializeField] private float minDamage = 5f;
    [SerializeField] private float maxDamage = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float missChance = 0.1f;
    [SerializeField] private float criticalChance = 0.15f;
    [SerializeField] private float criticalMultiplier = 2f;
    [SerializeField] private float attackCooldown = 2f;
    private float nextAttackTime;

    [Header("State Management Variables")]
    private State currentState = State.Patrol;
    private enum State { Patrol, Chase, Return, Attack }

    private void Start()
    {
        spawnPoint = transform.position;
        Vector3 wayPoint1 = new Vector3(spawnPoint.x, spawnPoint.y + 3f);
        Vector3 wayPoint2 = new Vector3(spawnPoint.x, spawnPoint.y - 0.3f);
        Vector3 wayPoint3 = new Vector3(spawnPoint.x + 2f, spawnPoint.y);
        Vector3 wayPoint4 = new Vector3(spawnPoint.x - 0.3f, spawnPoint.y);
        waypoints = new Vector3[4];
        waypoints[0] = wayPoint1;
        waypoints[1] = wayPoint2;
        waypoints[2] = wayPoint3;
        waypoints[3] = wayPoint4;
        targetWayPoint = waypoints[currentWayPointIndex];
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
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
            case State.Attack:
                AttackPlayer();
                break;
        }
    }

    // Handles all movement towards a target, used for patrol, chase, and return states
    private void FixedUpdate()
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

    // Handles patrolling between waypoints, if within tolerance of target waypoint start wait coroutine to pause before moving to next waypoint
    private void Patrol()
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

    // Checks for player within detection radius, if found switch to chase state
    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    // Handles chasing the player, if player moves out of detection radius switch to return state, if player moves within attack range switch to attack state
    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRadius)
        {
            currentState = State.Return;
        } else if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
    }

    // Checks if player has moved beyond leash distance from spawn point, if so switch to return state
    private void CheckLeashDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, spawnPoint);
        if (distanceToTarget > leashDistance)
        {
            currentState = State.Return;
        }
    }

    // Handles returning to patrol route, if player moves back within detection radius and leash distance switch to chase state, if reaches target waypoint switch to patrol state
    private void ReturnToPatrol()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetWayPoint);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius && distanceToTarget <= leashDistance)
        {
            currentState = State.Chase;
        } else if (distanceToTarget <= positionTolerance)
         {
            currentState = State.Patrol;
         }
    }

    // Handles actual movement function called from FixedUpdate, moves towards target position at given speed, if within stopping distance of target will stop moving
    private void MoveToTarget(float speed, float stoppingDistance, Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) > stoppingDistance)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime));
        } else
        {
            rb.MovePosition(transform.position);
        }
    }

    // Handles attacking the player, checks for miss and critical hit chances, applies damage to player if attack hits, switches back to chase state after attack
    private void AttackPlayer()
    {
        float distanceToTarget = Vector3.Distance(transform.position, spawnPoint);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange && distanceToPlayer <= detectionRadius)
        {
            currentState = State.Chase;
        } else if (distanceToPlayer > detectionRadius || distanceToTarget > leashDistance)
        {
            currentState = State.Return;
        } else if (Time.time >= nextAttackTime)
        {
            // Calculate hit/miss
            if (Random.value < missChance)
            {
                Debug.Log("Enemy attack missed!");
                return;
            }
            // Calculate damage
            float damage = Random.Range(minDamage, maxDamage);
            if (Random.value < criticalChance)
            {
                damage *= criticalMultiplier;
                Debug.Log("Critical hit! Damage to player: " + damage);
            }  
            else
            {
                Debug.Log("Hit! Damage to player: " + damage);
            }
            CharacterBase playerCharacter = player.GetComponent<CharacterBase>();
            if (playerCharacter != null)
            {
                playerCharacter.TakeDamage(damage);
            }
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Attacking player, next attack time: " + nextAttackTime);
        }
    }

    // Coroutine to handle waiting at waypoints, sets isWaiting to true to prevent movement, waits for specified time, then updates target waypoint and sets isWaiting to false to resume movement
    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWayPointIndex = (currentWayPointIndex + 1) % waypoints.Length;
        targetWayPoint = waypoints[currentWayPointIndex];
        isWaiting = false;
    }
    
    // Handles enemy death, currently just destroys the game object, can be expanded to include death animation, loot drops, etc.
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
