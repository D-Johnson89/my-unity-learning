using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBase
{

    public float detectionRadius = 5f;
    public float leashDistance = 10f;
    public float waitTime = 1f;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private Vector3 spawnPoint;
    private Transform player;

    enum State { Patrol, Chase, Return }
    private State currentState = State.Patrol;

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Patrol:
                Patrol();
                CheckForPlayer;
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
}
