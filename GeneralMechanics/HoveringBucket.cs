using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringBucket : MonoBehaviour 
{

    private Vector3 startPosition;
    private Vector3 newPosition;
    public float effectMultiplier = 0.5f;
    public float speedMultiplier = 2f;


	// Use this for initialization
	void Start ()
    {
        startPosition = this.transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
//        this.transform.position = startPosition + new Vector3(Mathf.Sin(Time.time, 0f, 0f));

        newPosition = transform.position;
        newPosition.y += (Mathf.Sin(Time.time * speedMultiplier) * effectMultiplier) * Time.deltaTime ;
        transform.position = newPosition ;
	}
}
