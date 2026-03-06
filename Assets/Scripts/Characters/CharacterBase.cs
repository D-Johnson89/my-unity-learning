using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 100;
    public int currentHealth = 10;
    public float moveSpeed = 5f;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage = 10)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    protected abstract void Die();

    protected void Move(float x, float y)
    {
        Vector2 movement = new Vector2(x, y) * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

}
