using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class HandFollowing : MonoBehaviour
{
	private GameObject targetHandPosition;
//	private GameObject palmOfTheHand;
	public GameObject handTarget;


	public bool handRotationEnabled = true;

	private Rigidbody myRigidbody;

	private float xAngle;
	private float yAngle;
	private float zAngle;
	public float followingForce = 200f;
	public float handTargetFollowingSpeed = 1f;
	public float handRotationSpeed = 2f;

	private Quaternion deltaRotation;

	private Vector3 eularVelocity;
	private Vector3 localTargetPosition;

	void Start ()
	{
		myRigidbody = this.GetComponent<Rigidbody> ();

		if (this.name == "RightShoulder") 
		{
			targetHandPosition = GameObject.FindGameObjectWithTag ("RightHandPosition");
//			palmOfTheHand = GameObject.FindGameObjectWithTag ("RightHandPalm");
			handTarget = GameObject.FindGameObjectWithTag ("RightHandTarget");
		}
		else if (this.name == "LeftShoulder")
		{
			targetHandPosition = GameObject.FindGameObjectWithTag ("LeftHandPosition");
//			palmOfTheHand = GameObject.FindGameObjectWithTag ("LeftHandPalm");
			handTarget = GameObject.FindGameObjectWithTag ("LeftHandTarget");
		}
	}

	// Physics
	public void FixedUpdate()
	{
		// Move hands towards their targetHandPositions next to the player body
		myRigidbody.AddForce ((targetHandPosition.transform.position-transform.position) * followingForce, ForceMode.Acceleration);
//		this.transform.position = Vector3.Lerp (this.transform.position, targetHandPosition.transform.position, followingForce); // Without physics. Hands stay next to the body but break physics

		Debug.DrawLine (this.transform.position, targetHandPosition.transform.position, Color.cyan);
		Debug.DrawLine (this.transform.position, handTarget.transform.position, Color.yellow);
		Debug.DrawLine (targetHandPosition.transform.position, handTarget.transform.position, Color.red);


		// If triggers are not pressed. So this is basically run almost all the time
		if (handRotationEnabled)
		{
			// Calculate horizontal rotation to handtarget
			localTargetPosition = transform.InverseTransformPoint(handTarget.transform.position);
			xAngle = Mathf.Atan2(localTargetPosition.x, localTargetPosition.z) * Mathf.Rad2Deg;
		
			// Calculate vertical rotation to hand target
			localTargetPosition = transform.InverseTransformPoint (handTarget.transform.position);
			yAngle =  Mathf.Atan2(localTargetPosition.y, localTargetPosition.z) * Mathf.Rad2Deg;

			// Make a velocity vector out of these
			eularVelocity = new Vector3 (-yAngle,xAngle, 0);

			// Make a deltarotaion of the velocity vector which is modifiable my rotation speed
			deltaRotation = Quaternion.Euler (eularVelocity * handTargetFollowingSpeed * Time.deltaTime);

			// Rotate hand towards hand target
//			myRigidbody.MoveRotation (myRigidbody.rotation * deltaRotation);
			myRigidbody.AddTorque (myRigidbody.rotation * eularVelocity * handRotationSpeed, ForceMode.Acceleration); // Would rotate smoother but has some reverse Y problem
		} 
	}
			
	public void enableHandFollowing()
	{
		if (handRotationEnabled != true)
			handRotationEnabled = true;
	}

	public void disableHandFollowing()
	{
		if (handRotationEnabled != false)
			handRotationEnabled = false;
	}
}
