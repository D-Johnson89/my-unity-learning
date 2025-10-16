using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    // Variables (without specifically declaring as public, variables default to private)
    private int _health;

    // Accessibility modifier for accessing _health variable
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    private int _mana;
    public int Mana
    {
        get
        {
            return _mana;
        }
        set
        {
            _mana = value;
        }
    }
    private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    // Getter/Setter below to show other option
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    // Player constructor
    public Player() {} 
    public Player(int health, int mana, string name)
    {
        Health = health;
        Mana = mana;
        Name = name;

    }

    // Must use 'virtual' keywork here for 'override' to work in Warrior class
    public virtual void Attack(int damage)
    {
        Debug.Log($"{Name} attacked for {damage} damage with sword.");
    }

    public void Info()
    {
        Debug.Log($"Health is : {Health}");
        Debug.Log($"Mana is: {Mana}");
        Debug.Log($"Name is: {Name}");
    }

    
    //Getter and setter for accessing private name variable
    /*
    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }
     */

}
