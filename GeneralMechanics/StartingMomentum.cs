using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMomentum : MonoBehaviour
{
    private Rigidbody myRigidbody;
    private GameManager gameManager;
    private bool momentumGiven = false;
    public float initialMovingForce = 0.2f;
    public float initialRotatingForce = 0.01f;

	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
        if (myRigidbody == null)
            Debug.Log("Couldn't find rigidbody for: " + this.transform.name + " which is required in StartingMomentum sript!");

        // Find game manager so we can see when the game has started
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<GameManager>();
	}
	
	void Update () 
    {
        if (!momentumGiven && gameManager.getGameState() == "InGameplay")
        {   
            // Give initial forces only once
            myRigidbody.AddForce(myRigidbody.transform.forward * initialMovingForce, ForceMode.Impulse);
            myRigidbody.AddTorque(myRigidbody.transform.forward * initialRotatingForce, ForceMode.Impulse);

            momentumGiven = true;
        }
	}
}
