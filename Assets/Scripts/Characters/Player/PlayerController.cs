using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("Player Variables")]
    public float moveSpeed = 3f;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 lastPosition;
    private Vector2 currentDirection;
    private Vector2 movement;

    [Header("Combat Variables")]
    /*[SerializeField] private int maxMana = 50;
    [SerializeField] private int currentMana;
    [SerializeField] private int manaCost;
    [SerializeField] private float manaRegenRate = 5f;*/
    [SerializeField] private float minDamage = 6f;
    [SerializeField] private float maxDamage = 12f;
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private float missChance = 0.1f;
    [SerializeField] private float criticalChance = 0.15f;
    [SerializeField] private float criticalMultiplier = 2.25f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private BoxCollider2D attackHitbox;
    [SerializeField] private float hitboxOffset = 0.5f;
    [SerializeField] private Vector2 verticalHitbox = new Vector2(1.5f, 2f);
    [SerializeField] private Vector2 horizontalHitbox = new Vector2(2f, 1.5f);
    [SerializeField] private float attackDuration = 0.5f;
    
    private float nextAttackTime;

    void Start()
    {
        lastPosition = transform.position;
        currentDirection = Vector2.down; // Default facing down
        attackHitbox.enabled = false;
    }
    void Update()
    {
        // Get directional input
        Vector2 currentPosition = transform.position;
        movement = currentPosition - lastPosition;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //
        if (movement.sqrMagnitude > 0.01f)
        {
            float absHorizontal = Mathf.Abs(movement.x);
            float absVertical = Mathf.Abs(movement.y);

            if (absHorizontal > absVertical)
            {
                currentDirection = (movement.x > 0) ? Vector2.right : Vector2.left;
                attackHitbox.size = horizontalHitbox;
            }
            else
            {
                currentDirection = (movement.y > 0) ? Vector2.up : Vector2.down;
                attackHitbox.size = verticalHitbox;
            }

            Debug.Log("Current direction: " + currentDirection);
            lastPosition = transform.position;
        }

        // Handle attack input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            Debug.Log("Player attacking, time: " + Time.time);
            StartCoroutine(Attack());
        }
        
    }
    void FixedUpdate()
    {
        Move(horizontalInput, verticalInput, moveSpeed);
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attack started, time: " + Time.time);
        // Detect enemies in range
        attackHitbox.enabled = true;
        attackHitbox.offset = currentDirection * hitboxOffset;
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.enabled = false;
    }

    void OnDrawGizmos()
    {
        if (attackHitbox != null)
        {
            Vector3 gizmoOffset = new Vector3(currentDirection.x * hitboxOffset, currentDirection.y * hitboxOffset, 0);
            Vector3 gizmoSize = new Vector3(attackHitbox.size.x, attackHitbox.size.y, 0);
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position + gizmoOffset, gizmoSize);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {  
        Debug.Log("Trigger fired");
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit detected, time: " + Time.time);
            Debug.Log("other tag: " + other.tag);
            // Check within attack range
            if (Vector2.Distance(transform.position, other.transform.position) <= attackRange)
            {
            // Calculate hit/miss
                if (Random.value < missChance)
                    {
                        Debug.Log("attack missed!");
                        return;
                    }
                    
                // Calculate damage
                float damage = Random.Range(minDamage, maxDamage);
                if (Random.value < criticalChance)
                {
                    damage *= criticalMultiplier;
                    Debug.Log("Critical hit! Damage to enemy: " + damage);
                }  
                else
                {
                    Debug.Log("Hit! Damage to enemy: " + damage);
                }
                    // Apply damage to enemy (assuming enemy has a TakeDamage method)
                    other.GetComponent<CharacterBase>().TakeDamage(damage);
                    nextAttackTime = Time.time + attackCooldown;
            }
            Debug.Log("Attacking enemy, next attack time: " + nextAttackTime);
            
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

}