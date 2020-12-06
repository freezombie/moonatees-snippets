using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class CameraRotation : MonoBehaviour {

	private Camera mainCamera;
	public float inputSensitivy = 2;
	public PlayerControl playerControl;
	public Quaternion deltaRotationX;

	public float lowerLimit = 90;
	public float upperLimit = 270;
	private float xRotation = 0;
	private float offset = 5; // Degrees that camera can rotate over allowed limits

	private float cameraRotationSpeed = 3000;

	private float roundedXRotation; 

	// Use this for initialization
	void Start () 
	{
		playerControl = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		mainCamera = Camera.main;
		xRotation = transform.localRotation.eulerAngles.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!InputManagerX.GetButton("Look"))	
		{
			// Check if left trigger is not pressed
			if (playerControl.allowLooking)
			{
				if (InputManagerX.IsSteamVREnabled ()) {
					if (playerControl.seatedVR) {
						Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(InputManagerX.GetRotation("Hmd").eulerAngles.x, InputManagerX.GetRotation("Hmd").eulerAngles.y, 0f));
					} else {
						Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(InputManagerX.GetRotation("Hmd").eulerAngles.x, 0f, 0f));
					}
				} else {
                    deltaRotationX = Quaternion.Euler(inputSensitivy * InputManagerX.GetAxis("LookVertical") * Vector3.left);
                    xRotation = transform.localRotation.eulerAngles.x;

                    // When rotating up
                    if (playerControl.invertState * deltaRotationX.x < 0)
                    {
                        roundedXRotation = Mathf.Round(xRotation * 1000) / 1000;
                        if (xRotation > upperLimit + offset && xRotation < 360 ||
                            xRotation < lowerLimit && xRotation > -1)
                        {
                            mainCamera.transform.Rotate(Vector3.right * deltaRotationX.x * playerControl.invertState * cameraRotationSpeed * Time.deltaTime);
                        }
                    }

                    // When rotating down
                    else if (playerControl.invertState * deltaRotationX.x > 0)
                    {
                        roundedXRotation = Mathf.Round(xRotation * 1000) / 1000;
                        if (xRotation < lowerLimit - offset && xRotation > -1 ||
                            xRotation > upperLimit && xRotation < 360)
                        {
                            mainCamera.transform.Rotate(Vector3.right * deltaRotationX.x * playerControl.invertState * cameraRotationSpeed * Time.deltaTime);
                        }
                    }
                }
			}
		}	
	}
}
		
