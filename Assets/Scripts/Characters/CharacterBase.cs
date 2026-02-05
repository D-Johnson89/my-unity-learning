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
        Debug.Log($"Health: {currentHealth}, Damage Taken: {damage}");
        if (currentHealth <= 0) Die();
    }

    protected abstract void Die();

    protected void Move(float x, float y)
    {
        rb.linearVelocity = new Vector2(x * moveSpeed, y * moveSpeed);
    }

}
