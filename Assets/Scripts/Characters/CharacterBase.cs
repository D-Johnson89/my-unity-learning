using UnityEngine;
using System.Collections.Generic;

public abstract class CharacterBase : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("UI Element")]
    [SerializeField] protected UIHandler uiHandler;
    
    [Header("Base Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    private float healthFillAmount;
    

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }


    // Method to apply damage to the character, takes single damage value as parameter
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthFillAmount = currentHealth / (float)maxHealth;
        if (uiHandler != null)
        {
            uiHandler.SetFill(healthFillAmount, UIHandler.BarType.Health);
        }
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
