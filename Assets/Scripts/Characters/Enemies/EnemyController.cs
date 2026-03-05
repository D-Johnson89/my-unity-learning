/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base Enemy AI/Movement
public class EnemyController : CharacterBase
{

    // Patrol variables
    public GameObject wayPoint1;
    public GameObject wayPoint2;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private Transform currentPoint;

    // Enemy variables
    private Rigidbody2D rb;
    public float detectionRadius = 5f;
    public float leashDistance = 10f;
    public float waitTime = 1f;  
    private bool isWaiting = false;
    private Vector3 spawnPoint;
    
    // State variables
    enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = wayPoint1.transfrom;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

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
        
    }
    
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
*/