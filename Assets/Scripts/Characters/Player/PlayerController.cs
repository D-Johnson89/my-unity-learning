using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("Player Specific")]
    public int maxMana = 50;
    public int currentMana;
    public float moveSpeed = 3f;
    public float horizontalInput;
    public float verticalInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        Move(horizontalInput, verticalInput, moveSpeed);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

}