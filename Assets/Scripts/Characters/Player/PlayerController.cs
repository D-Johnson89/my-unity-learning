using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("Player Variables")]
    [SerializeField] private float moveSpeed = 3f;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 lastPosition;
    private Vector2 currentDirection;

    [Header("Combat Variables")]
    [SerializeField] private float minDamage = 6f;
    [SerializeField] private float maxDamage = 12f;
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

    private void Start()
    {
        lastPosition = transform.position;
        currentDirection = Vector2.down; // Default facing down
        attackHitbox.enabled = false;
    }

    // Handles player input for movement and attacks, determines attack direction based on movement direction, and manages attack cooldowns
    private void Update()
    {
        // Get directional input
        Vector2 currentPosition = transform.position;
        Vector2 movement = currentPosition - lastPosition;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
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

            lastPosition = transform.position;
        }

        // Handle attack input
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            StartCoroutine(Attack());
        }
        
    }

    // Handle movement in FixedUpdate for consistent physics updates
    private void FixedUpdate()
    {
        Move(horizontalInput, verticalInput, moveSpeed);
    }

    // Coroutine to manage attack timing, enables hitbox for duration of attack then disables it
    private IEnumerator Attack()
    {
        // Enable hitbox and set offset based on current direction, then disable after attack duration
        attackHitbox.enabled = true;
        attackHitbox.offset = currentDirection * hitboxOffset;
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.enabled = false;
    }

    // Draw the attack hitbox in the editor for tuning and debuging purposes, offset in the direction the player is facing
    private void OnDrawGizmos()
    {
        if (attackHitbox != null)
        {
            Vector3 gizmoOffset = new Vector3(currentDirection.x * hitboxOffset, currentDirection.y * hitboxOffset, 0);
            Vector3 gizmoSize = new Vector3(attackHitbox.size.x, attackHitbox.size.y, 0);
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position + gizmoOffset, gizmoSize);
        }
    }

    // Handle collision with enemy hitboxes, checks for miss and critical hit chances, applies damage to enemy if attack hits, sets next attack time based on cooldown
    private void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.CompareTag("Enemy"))
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

            CharacterBase enemyCharacter = other.GetComponent<CharacterBase>();
            if (enemyCharacter != null)
            {
                enemyCharacter.TakeDamage(damage);
            }
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Attacking enemy, next attack time: " + nextAttackTime);
            
        }
    }

    // Override the Die method from CharacterBase to handle player death, currently just destroys the player game object but can be expanded to include death animations, game over screens, etc.
    protected override void Die()
    {
        Destroy(gameObject);
    }

}