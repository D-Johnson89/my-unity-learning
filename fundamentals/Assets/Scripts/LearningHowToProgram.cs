using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningHowToProgram : MonoBehaviour
{

    // Declare player objects here to use globally
    Player warrior;
    Player archer;

    private void Start()
    {

        // Create Warrior class 
        Warrior warrior = new Warrior(100, 50, "Kalis");
        warrior.Attack(20);

        // Initialize player objects
        archer = new Player(75, 100, "Lavits");

        // Start Coroutine
        StartCoroutine(ExecuteSomething(3));
        /*
            Can also call Coroutines in string value to start and stop as such.

            StartCoroutine("ExecuteSomething"());
            StopCoroutine("ExecuteSomething"());
        */


        // Use getter to retrieve private health variable
        int health = archer.Health;
        Debug.Log($"Health is {health}.");

        // Use setter to change health (private variable in player class file)
        archer.Health = 83;
        health = archer.Health;
        Debug.Log($"Health is now {health}.");

        // Use getter to retrieve private name variable
        // string name = archer.GetName();
        string name = archer.Name;
        Debug.Log($"Name is {name}.");

        // Use setter to change health (private variable in player class file)
        // archer.SetName("Kornbred");
        archer.Name = "Kornbred";
        // name = archer.GetName();
        name = archer.Name;
        Debug.Log($"Name is now {name}.");


    }

    // Delay Executing (Coroutines)
    IEnumerator ExecuteSomething(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log($"{time} second delay");
    }


} // class
