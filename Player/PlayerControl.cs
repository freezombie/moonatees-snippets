using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class PlayerControl : MonoBehaviour {

	private float moveSensitivity = 2F;
	//	private float mouseSensitivity = 100F;
	public float handSensitivity = 100F;
    private float movementForce = 1200;

	public float rotationSensitivity = 10f;

	public bool inverted = false; //inverted or normal camera/hand control
	public int invertState = 1;

	// Needs to be here for InputOptionsMessage but we are not using it
	public bool handPrimary = false; //hand or mmove/look as primary
	public bool seatedVR = false;
	public bool motionControl = false;
	public bool toggleControl = false;

	public bool toggleState = false;

	public bool allowMovement = true;
	public bool allowLooking = true;

	public bool allowRightHand = false;
	public bool allowLeftHand = false;

	private Vector3 rightHandDeltaRotationX;
	private Vector3 rightHandDeltaRotationY;

	private Vector3 leftHandDeltaRotationX;
	private Vector3 leftHandDeltaRotationY;

	private Vector3 rightJoysticVerticalRotation;
	private Vector3 rightJoysticHorizontalRotation;

	private Vector3 newRightHandTargetPosition;
	private Vector3 newLeftHandTargetPosition;

	private Rigidbody leftHand;
	private Rigidbody rightHand;
	private Rigidbody playerBody;
	private Rigidbody myRigidBody;

	private Vector3 desiredMovementDirection;
	private Vector3 movementDirection = Vector3.zero;

	private HandFollowing rightHandFollowingScript;
	private HandFollowing leftHandFollowingScript;

	private GameObject leftHandTarget;
	private GameObject rightHandTarget;

	private GameObject leftHandOffsetTarget;
	private GameObject rightHandOffsetTarget;

	public bool enableStrafeingUp = false;
	public bool enableStrafeingDown = false;

	private GameObject rightShoulder;
	private GameObject leftShoulder;


	public bool strafeEnabled = false;

	// Use this for initialization
	void Start () 
	{
		myRigidBody = GetComponent<Rigidbody>();

		rightShoulder = GameObject.FindGameObjectWithTag ("RightShoulder");	
		leftShoulder = GameObject.FindGameObjectWithTag ("LeftShoulder");	

		rightHand = rightShoulder.GetComponent<Rigidbody>();	
		leftHand = leftShoulder.GetComponent<Rigidbody>();

//		rightHandFollowingScript = GameObject.GetComponent<HandFollowing>();	
//		leftHandFollowingScript = GameObject.GetComponent<HandFollowing>();	

		rightHandFollowingScript = GameObject.FindGameObjectWithTag ("RightShoulder").GetComponent<HandFollowing>();	
		leftHandFollowingScript = GameObject.FindGameObjectWithTag ("LeftShoulder").GetComponent<HandFollowing>();	

		rightHandTarget = GameObject.FindGameObjectWithTag ("RightHandTarget");
		leftHandTarget = GameObject.FindGameObjectWithTag ("LeftHandTarget");

		rightHandOffsetTarget = GameObject.FindGameObjectWithTag ("RightHandTargetOffset");
		leftHandOffsetTarget = GameObject.FindGameObjectWithTag ("LeftHandTargetOffset");

		rightHandOffsetTarget.transform.position = rightHand.transform.position + rightHand.transform.forward * 1.5f;
		leftHandOffsetTarget.transform.position = leftHand.transform.position + leftHand.transform.forward * 1.5f;
	}
		
	void Update ()
	{

		// Check if player is not moving hands so moving is allowed
		if (allowMovement)
		{
			movementDirection = desiredMovementDirection;
		}

		if (inverted) 
			invertState = -1;
		else
			invertState = 1;			

		allowMovement = true;
		allowLooking = true;
		allowRightHand = false;
		allowLeftHand = false;

        // Toggle hand movement controls when either the left or right joysticks are pressed
//		if (toggleControl) {
//			if (InputManagerX.GetButtonDown("RS") || InputManagerX.GetButtonDown("LS") || Input.GetKeyUp(KeyCode.Space)) {
//				toggleState = !toggleState;
//			}
//		}

		// Movement
		// Left trigger is NOT pressed
//		if (!toggleState && !InputManagerX.GetButton("Move") )


		// Both left trigger and left bumper
//		if (!toggleState && !InputManagerX.GetButton("Move") && (InputManagerX.GetAxis("LeftTrigger") <= 0 ))

//		 Just left bumper
		if (!toggleState && !InputManagerX.GetButton ("Move")) {
			strafeEnabled = false;
		} else
			strafeEnabled = true;


		// While left trigger is not pressed
//		if (InputManagerX.GetAxis("LeftTrigger") <= 0 )		
		if (!toggleState && InputManagerX.GetAxis("LeftTrigger") <= 0 )				
			
		{
			float moveHorizonal = InputManagerX.GetAxis("Horizontal");
			float moveVertical = InputManagerX.GetAxis("Vertical");

			Vector2 movementDirection = new Vector2 (moveHorizonal, moveVertical);

			// Normalize vector from input
			if (movementDirection.sqrMagnitude > 1)
				movementDirection.Normalize ();

			// If strafe is not enabled, move normally 
			if (!strafeEnabled) {
				desiredMovementDirection = Camera.main.transform.forward * movementDirection.y + Camera.main.transform.right * movementDirection.x;
				desiredMovementDirection = desiredMovementDirection * movementForce / 2;

			// If strafe is enabled by holding left bumper, forward direction is changed to up
			} else {
				desiredMovementDirection = Camera.main.transform.up * movementDirection.y + Camera.main.transform.right * movementDirection.x;
				desiredMovementDirection = desiredMovementDirection * movementForce / 2;
			}

			leftHandFollowingScript.enableHandFollowing ();


		} else // Left trigger is pressed
		{

			leftHandDeltaRotationX = handSensitivity * InputManagerX.GetAxis("Horizontal") * Vector3.up;
			leftHandDeltaRotationY = handSensitivity * InputManagerX.GetAxis("Vertical") * Vector3.left * invertState;

			leftHandFollowingScript.disableHandFollowing ();

			// Disable movement while moving a hand
			allowMovement = false;
			allowLooking = false;
			allowLeftHand = true;
		}

		// Look rotation
		// If right trigger is NOT pressed	
		if (!toggleState && !InputManagerX.GetButton("Look") && (InputManagerX.GetAxis("RightTrigger") <= 0 )) 
		{

			rightJoysticHorizontalRotation = rotationSensitivity * InputManagerX.GetAxis("LookHorizontal") * Vector3.up;

			rightHandFollowingScript.enableHandFollowing ();

		} else // Trigger pressed now I guess?
		{ 
			rightHandDeltaRotationX = handSensitivity * InputManagerX.GetAxis("LookHorizontal") * Vector3.up;
			rightHandDeltaRotationY = handSensitivity * InputManagerX.GetAxis("LookVertical") * Vector3.left * invertState;

			rightHandFollowingScript.disableHandFollowing  ();

			// Disable movement while moving a hand
			allowMovement = false;
			allowLooking = false;
			allowRightHand = true;
		}

		// Moving up with Y
		if (InputManagerX.GetButton ("Y") && !InputManagerX.GetButton ("X") || Input.GetKey(KeyCode.E)) {
			enableStrafeingUp = true;
			enableStrafeingDown = false;
		} else {
			enableStrafeingUp = false;
		}

		// Moving down with X
		if (InputManagerX.GetButton ("X") && !InputManagerX.GetButton ("Y") || Input.GetKey(KeyCode.Q)) {
			enableStrafeingDown = true;
			enableStrafeingUp = false;
		} else {
			enableStrafeingDown = false;
		}
	}

	// All physic related stuff should be ran here
	void FixedUpdate()
	{
		// Movement with joystic
		if (allowMovement)
		{
            myRigidBody.AddForce (movementDirection * Time.deltaTime, ForceMode.Acceleration);

			rightHandTarget.transform.position = rightHandOffsetTarget.transform.position;
			leftHandTarget.transform.position = leftHandOffsetTarget.transform.position;
		}

		// Forward movement with Y button
		if (enableStrafeingUp)
		{
//			myRigidBody.AddForce (Camera.main.transform.forward * 10, ForceMode.Acceleration); // Forward
            myRigidBody.AddForce (Camera.main.transform.up * movementForce / 2 * Time.deltaTime, ForceMode.Acceleration); // Up

		}

		// Backwards movement with X button
		if (enableStrafeingDown)
		{
//			myRigidBody.AddForce (Camera.main.transform.forward * -10, ForceMode.Acceleration); //Backward
            myRigidBody.AddForce (Camera.main.transform.up * -movementForce / 2 * Time.deltaTime, ForceMode.Acceleration); // Down

		}

		// Horizontal rotating / looking around. (Protip: Vertical rotation is located in CameraRotation)
		if (allowLooking) {		
			if (InputManagerX.IsSteamVREnabled () && !seatedVR) {
				myRigidBody.MoveRotation (Quaternion.Euler (new Vector3 (0f, InputManagerX.GetRotation ("Hmd").eulerAngles.y, 0f)));
			} else {
				myRigidBody.AddTorque (this.transform.up * rightJoysticHorizontalRotation.y, ForceMode.Acceleration);
			}
		}
		// Right hand moving
		if (allowRightHand) {
			
			// Moving the right hand when trigger is held
			if (motionControl && (InputManagerX.IsSteamVRControllerEnabled())) { // Razer Hydra / VR stuff
				rightHand.transform.rotation = Quaternion.RotateTowards (rightHand.transform.rotation, myRigidBody.transform.rotation * InputManagerX.GetRotation("Right"), 5f);
			} else {
				rightHand.AddTorque (this.transform.up * rightHandDeltaRotationX.y, ForceMode.Acceleration);
				rightHand.AddTorque (this.transform.right * rightHandDeltaRotationY.x, ForceMode.Acceleration);
			}

			newRightHandTargetPosition = (rightHand.transform.position + rightHand.transform.forward * 1.5f);
			rightHandOffsetTarget.transform.position = newRightHandTargetPosition;
		}

		// Left hand moving
		if (allowLeftHand){

			// Moving the right hand when trigger is held
			if (motionControl && (InputManagerX.IsSteamVRControllerEnabled())) { // Razer Hydra / VR stuff
				leftHand.transform.rotation = Quaternion.RotateTowards (leftHand.transform.rotation, myRigidBody.transform.rotation * InputManagerX.GetRotation("Left"), 5f);
			} else {
				leftHand.AddTorque (this.transform.up * leftHandDeltaRotationX.y, ForceMode.Acceleration);
				leftHand.AddTorque (this.transform.right * leftHandDeltaRotationY.x, ForceMode.Acceleration);
			}

			newLeftHandTargetPosition = leftHand.transform.position + leftHand.transform.forward * 1.5f;
			leftHandOffsetTarget.transform.position = newLeftHandTargetPosition;
		}
	}

	public void setMaxMovementForce()
	{
        movementForce = 1200;
        //		movementForce = forceAmount;
	}

    public void setZeroMovementForce()
    {
        movementForce = 0;
        //      movementForce = forceAmount;
    }

    public float getMovementForce()
    {
         return movementForce;
    }

	// Disable some functionality from hands when going to the cinema mode so their original rotation is kept when going back to the gameplay
	public void disableHands()
	{
		// Settings velocities to 0
		rightShoulder.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		rightShoulder.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		leftShoulder.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		leftShoulder.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

		// Disabling hand movements. They still persist in gameplay but won't move or rotate
		rightShoulder.GetComponent<HandFollowing> ().enabled = false;
		leftShoulder.GetComponent<HandFollowing> ().enabled = false;
		rightShoulder.GetComponent<Rigidbody> ().isKinematic = true;
		leftShoulder.GetComponent<Rigidbody> ().isKinematic = true;
	}

	// Enable all hand fuctionality back to normal gameplay
	public void enabledHands()
	{
		rightShoulder.SetActive (true);
		leftShoulder.SetActive (true);
		rightHandTarget.SetActive (true);
		leftHandTarget.SetActive (true);

		rightShoulder.GetComponent<HandFollowing> ().enabled = true;
		leftShoulder.GetComponent<HandFollowing> ().enabled = true;
		rightShoulder.GetComponent<Rigidbody> ().isKinematic = false;
		leftShoulder.GetComponent<Rigidbody> ().isKinematic = false;
	}
}
