using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterBase
{
    [Header("Player Specific")]
    public int maxMana = 50;
    public int currentMana;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Move(h, v);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

}