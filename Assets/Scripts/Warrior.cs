using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can only access public variables and functions/methods of Player
public class Warrior : Player
{

    // Warrior constructor
    public Warrior(int health, int mana, string name)
    {
        // Gain access to Player Health, Mana, and Name via passed arguments
        Health = health;
        Mana = mana;
        Name = name;
    }

    public override void Attack(int damage)
    {
        Debug.Log($"{Name} attacked for {damage} damage with axe-.");
    }

} // class
