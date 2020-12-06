using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaProjectorLightSpinner : MonoBehaviour
{
    public float rotationSpeed = 5f;
	
	// Update is called once per frame
	void Update ()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
	}
}
