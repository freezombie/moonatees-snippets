using UnityEngine;
using System.Collections;

public class RandomTrailerObjectSettings : MonoBehaviour 
{
	public float rotatingSpeed = 0.1f;

	public float xRotationAmount = 0;
	public float yRotationAmount = 0;
	public float zRotationAmount = 0;


	public float movingspeed = 0.1f;

	public float xMovementAmount = 0;
	public float yMovementAmount = 0;
	public float zMovementAmount = 0;

	private Vector3 startingPosition;
	private Quaternion startingRotation;

    public GameObject coffeeCup;
    private Vector3 coffeeCupStartingPosition;
    public  Vector3 coffeeCupNearCaptainPosition = new Vector3 (-13.7f, 33.5f, 70.9f);
    private bool coffeeMoved = false;

	void Start()
	{
		startingPosition = this.transform.position;
	}

	void Update ()
	{
		this.transform.RotateAround (new Vector3 (xRotationAmount, yRotationAmount, zRotationAmount), rotatingSpeed * Time.deltaTime);
		this.transform.Translate (new Vector3 (xMovementAmount, yMovementAmount, zMovementAmount) * movingspeed * Time.deltaTime);
	}

	public void resetPosition()
	{
		this.transform.position = startingPosition;
	}

	public void resetRotation()
	{
		this.transform.rotation = startingRotation;
	}

}
