using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	private AudioSource audioSource;

	private float pitch = 1f;
	private float minPitch = 0.8f;
	private float maxPitch = 1.2f;
	private float volume = 1f;
	private float hitTreshold = 0.1f; // How hard item collision velocty has to at least be in order to produce sound effect

	public bool leftCol = false;
	public bool rightCol = false;

	private GameObject player;
	private Rigidbody myRigidbody;

	private ItemHoldingPosition itemHoldingPositionScript;
	private Vector3 itemHoldingPosition;
	private Vector3 itemMovingDirection;

	public float movingForce = 1000f;
	public float rotatingForce = 1f;

	public bool handsAttached = false;
	private Vector3 zeroVelocity = Vector3.zero;

	private HandFollowing leftShoudlerState;
	private HandFollowing rightShoulderState;

	private Renderer renderer;
	private Color myColor;
	private Color transparencyOn = new Color (0, 0, 0, 0);
	private Color transparencyOff = new Color (0, 0, 0, 1);
	private float transparencyAmount = 0.5f;


//	#####################################################################################################
//  # Items need the following to be able to be grabbed properly: 										#			~ Ultimate ASCII adventure ~
//  #	1. Layer needs to be set to "Item"																#				~ Legacy of the king~
//	#	2. Rigidbody																					#			
//	#	3. Rigidbody's Interpolate mode needs to be set to "Extrapolate" from default "None"			#	- Can you find the hidden gold and retrieve it back safely?	
//	#	4. Collider																						#####################################################################	
//	#	5. Grabbable script (this)																		#     You are here -->	@...........................................#
//	#	(Optional) 6. If one wants an item to have sounds, it needs to have an Audio Source component   ##############################################################+####.#
//	#	(Optional) 7. Set audio you want to AudioClip variable and set "Play On Awake" to false.  		#.$.......................................$..............^.....#D.#.#
//	#################################################################################################################################################+#######################
//																																					#.......................#
//																																					#########################
	void Start () 
	{		
		// For adjusting shader
		renderer = GetComponent<Renderer> ();
		if (renderer != null) {
			myColor = renderer.material.GetColor("_Color");
		}

		myRigidbody = this.GetComponent<Rigidbody>();
		//itemHoldingPositionScript = GameObject.FindGameObjectWithTag("ItemHoldingPositionChecker").GetComponent<ItemHoldingPosition>();

		if (audioSource != null) {	
			audioSource.spatialBlend = 1; // Needs to be 1 to make 3D space volume to work
			audioSource.minDistance = 2f;
			audioSource.maxDistance = 15f;
		}
	}

	void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter(Collision collision)
	{
//		Debug.Log ("Collision name: " + collision.transform.name + " with velocity of: " + collision.relativeVelocity.magnitude);

		float collisionSpeed = collision.relativeVelocity.magnitude;

		// Sound from this
		if (collision.transform.name != "RightShoulder" && collision.transform.tag != "LeftShoulder") // Prevent sound spamming from hands
		{
			if (audioSource != null) 
			{
//				if (collisionSpeed > hitTreshold) 
//				{
					audioSource.volume = getVolumeFromVelocity (collisionSpeed);
					audioSource.pitch = getRandomPitch ();
					audioSource.Play ();
//				}
			}
		}

		// Sound from the target
		if (collision.gameObject.transform.tag != "Item")
		{
			if (collision.gameObject.GetComponent<AudioSource> () != null) 
			{
				AudioSource targetAudioSource = collision.gameObject.GetComponent<AudioSource> ();

				targetAudioSource.volume = getVolumeFromVelocity (collisionSpeed);
				targetAudioSource.pitch = getRandomPitch ();
				targetAudioSource.Play ();
			}
		}
	}

	public float getRandomPitch()
	{
		pitch = Random.Range (minPitch, maxPitch);
		return pitch;
	}

	public float getVolumeFromVelocity(float collisionSpeed)
	{
		volume = 0.3f + (collisionSpeed / 5);
		return volume;
	}

	public void setTransparencyOn()
	{
		renderer.material.SetColor ("_OutlineColor", transparencyOn); 
		myColor.a = transparencyAmount;
		renderer.material.SetColor ("_Color", myColor);
	}

	public void setTransparencyOff()
	{
		renderer.material.SetColor ("_OutlineColor", transparencyOff); 
		myColor.a = 1f;
		renderer.material.SetColor ("_Color", myColor); 
	}

	void OnEnable()
	{
		leftCol = false;
		rightCol = false;
	}
	
	void FixedUpdate ()
	{
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag ("Player");
		}	

		if (leftCol && rightCol) 
		{

			Vector3 targetDelta = this.transform.position - player.transform.position;
			float angleDifference = Vector3.Angle (transform.forward, targetDelta);
			Vector3 cross = Vector3.Cross (transform.forward, targetDelta);
			myRigidbody.AddTorque (cross * angleDifference * rotatingForce);

			// Get item holding position from another script
			itemHoldingPosition = itemHoldingPositionScript.itemHoldingPosition;

			// Calculate direction that items needs to move towards
			itemMovingDirection = (itemHoldingPosition - this.transform.position);

			// Gravity stabilisation things
			if (handsAttached == false)
			{
				handsAttached = true;
				setTransparencyOn ();
			}

			// Macgyver fix to force detach hands with certain distance
			if (itemHoldingPositionScript.distanceBetweenHands > 3f) // Distance that needs to be between palms
			{
				removeItemFromHold ();
			}

			myRigidbody.velocity = zeroVelocity;
			myRigidbody.mass = 3f;
			myRigidbody.drag = 1f;
			myRigidbody.angularDrag = 3f;

			// Moving the item
			myRigidbody.AddForce(itemMovingDirection * movingForce, ForceMode.Acceleration);

		} else { //If neither of the colliders were found

			if (handsAttached)
			{
				myRigidbody.mass = 1f;
				myRigidbody.drag = 0.1f;
				myRigidbody.angularDrag = 0.1f;
				myRigidbody.velocity = zeroVelocity;
				handsAttached = false;
				setTransparencyOff ();
			}
		}
	}
		
	void OnTriggerEnter(Collider collider)
	{
		Debug.Log ("You shouldn't see this from: " + this.transform.name);

	}

	void OnTriggerStay(Collider collider)
	{
		/*
		switch (collider.gameObject.name) {
		case "LeftHandTrigger":
			if (leftShoudlerState == null)
				leftShoudlerState = collider.GetComponentInParent<HandFollowing> ();
			leftCol = true;
			break;
		case "RightHandTrigger":
			// Gets player right shoulder's state when touching for the first time
			if (rightShoulderState == null)
				rightShoulderState = collider.GetComponentInParent<HandFollowing> ();
			rightCol = true;
			break;
		}
		*/
	}


	// Method for external usage to remove item from player's hold. Not currently used anywhere
	public void removeItemFromHold()
	{
		if (myRigidbody != null) 
		{
			// Set physics thingys to default
			myRigidbody.mass = 1f;
			myRigidbody.drag = 0.1f;
			myRigidbody.angularDrag = 0.1f;
			myRigidbody.velocity = zeroVelocity;
			handsAttached = false;
			setTransparencyOff ();
		}

		leftCol = false;
		rightCol = false;
	}

	void OnTriggerExit(Collider collider)
	{
		// Could add player's rigidbody's velocity to the object before detaching so it would not just stop
		switch (collider.name) {
		case "LeftHandTrigger":
			if (leftShoudlerState.handRotationEnabled == false) { // If hand is being moved (most likely to drop off the item)
				leftCol = false;
//				rightCol = false;
			}

			break;
		case "RightHandTrigger":
			if (rightShoulderState.handRotationEnabled == false) { // If hand is being moved (most likely to drop off the item)			
				rightCol = false;
//				leftCol = false;
			}

			break;
		}
	}

    public bool GetHandsAttached()
    {
        return handsAttached;
    }
		
}
