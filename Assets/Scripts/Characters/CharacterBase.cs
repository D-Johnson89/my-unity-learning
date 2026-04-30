using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    protected abstract void Die();

    protected void Move(float x, float y, float moveSpeed)
    {
        Vector2 movement = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

}
