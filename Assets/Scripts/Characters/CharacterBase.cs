using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    
    [Header("Base Stats")]
    public float maxHealth = 100;
    public float currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Method to apply damage to the character, takes single damage value as parameter
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    // Abstract method to be implemented by derived classes for character death behavior
    protected abstract void Die();

    // Method for character movement, takes x and y input for direction and moveSpeed for speed of movement
    protected void Move(float x, float y, float moveSpeed)
    {
        Vector2 movement = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

}
