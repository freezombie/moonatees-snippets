using UnityEngine;
using System.Collections;
//using System.Collections.Generic;
using TeamUtility.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // For graphical raycaster

public class InputOptionsMessage : MonoBehaviour {

	public GameObject startingScreenComponent;
	public GameObject controllerText;
	public GameObject player;
	public GameObject vrText;
	public GameObject invertedText;
	public GameObject toggleText;
	public GameObject motionText;
	private List<string> controllers = new List<string>();
	private string controller;
	public int controllerIndex;

	public GameObject gamepadInstructionImage;
	public GameObject keyboardAndMouseInstructionImage;
	public GameObject dailyMissionImage;

	private AudioSource myAudioSource;
	public AudioClip audioSelect;
	public AudioClip audioDisable;

	public MusicPlayer mainCameraMusicPlayer;

	public bool gameCanBegin = false;
	public bool allButStartControlsDisabled = false;


	public RawImage blackScreenImage;
	public RawImage[] allUIImages;
	public List <RawImage> allInstructionImages;
	public List <RawImage> allDailyMissionImages;
	public List <RawImage> allEndingScreenImages;

	public bool StartButtonEnabled = true;

	private bool beginingStarted = false;
	private bool creditsStarted = false;
	private bool endScreenStarted = false;

	public string gameState = "";

	public bool musicEnabled = true;

	public Text creditText;

	// Testing variables
	public float timeSpeed;

	public Camera cinemaCamera;
	public Camera uiCamera;
	public Camera blacknessCamera;

	private PlayerControl playerControl;

	//	private Transform backupTransform;
	private Vector3 backupPosition;
	private Quaternion backupRotation;

	private Quaternion backupCameraRotationX;

	private GameObject cinemaRoomPivotPoint;

	private float globalFadingDuration = 0.3f;

	// temporary variables
	public GameObject randomTrailerObject;

	public LayerMask speechLayerMasks;


	public List<GameObject> hits = new List<GameObject> ();
	public SpeechBubble closestSpeech;

	public GameObject randomSpeechLocationPointer;



	public RaycastHit closestHit;

	public RaycastHit hitInfo;

	public SpeechBank speechBank;
	public bool scanGoing = false;

	// How often a boxcast is made to scan for speech boxes
	public float speechScanInterval = 0.5f;

    // Contains all the stuff in ending screen. Images and texts
    public GameObject endingScreen;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		cinemaCamera = GameObject.FindGameObjectWithTag ("CinemaCamera").GetComponent<Camera>();
		uiCamera = GameObject.FindGameObjectWithTag ("UICamera").GetComponent<Camera>();

		cinemaRoomPivotPoint = GameObject.FindGameObjectWithTag ("CinemaRoomPivot");

		alignCinemaRoomToPlayer(); // position, check

		speechBank = this.GetComponentInChildren<SpeechBank>();


		//add list controller
		//		controllers.Add("Gamepad");

		controllers.Add("Keyboard And Mouse");
		//        controllers.Add("Keyboard");
		//controllers.Add("Gamepad");

		//get current controller
		controller = InputManagerX.PlayerOneConfiguration.name;
		controllerIndex = controllers.IndexOf(controller);
		//		controllerIndex = 0;

		Time.timeScale = 1;

		//get current config
		playerControl = player.GetComponent<PlayerControl> ();

		if (playerControl.inverted) {
			invertedText.GetComponent<Text>().text = "Inverted Control";
		} else {
			invertedText.GetComponent<Text>().text = "Normal Control";
		}

        playerControl.setZeroMovementForce();
//				StartCoroutine(delayedDisablePlayerMovement (0)); // Sets backup rot/pos of the player but screws init rot
		backupPosition = playerControl.transform.position;
		backupRotation = playerControl.transform.rotation;
		backupCameraRotationX = Camera.main.transform.rotation;

		/*
        if (player.GetComponent<PlayerControl>().seatedVR) {
            vrText.GetComponent<Text>().text = "SeatedVR";
        }
        else {
            vrText.GetComponent<Text>().text = "StandingVR";
        }
       

        if (player.GetComponent<PlayerControl>().toggleControl) {
            toggleText.GetComponent<Text>().text = "Toggle On";
        }
        else {
            toggleText.GetComponent<Text>().text = "Toggle Off";
        }

        if (player.GetComponent<PlayerControl>().motionControl) {
            motionText.GetComponent<Text>().text = "Motion On";
        }
        else {
            motionText.GetComponent<Text>().text = "Motion Off";
        }
        */

		//		allUIImages = gameObject.GetComponentsInChildren<RawImage> (true);
		allUIImages = GameObject.Find("UICanvas").GetComponentsInChildren<RawImage> (true);


		getAllDailyMissionImages();
		getAllInstructionImages ();
		getAllEndingScreenImages ();

		// We set volume to 0 on start to prevent hearing all sounds that are played on start up (for some reason even if "Play on awake" is set off)
		//		AudioListener.volume = 0f;

		myAudioSource = GetComponent<AudioSource> ();

		mainCameraMusicPlayer = Camera.main.GetComponentInChildren<MusicPlayer> ();

		creditText = GameObject.Find ("UICanvas/Credits/Text").GetComponent<Text> ();

		timeSpeed = Time.timeScale;
	
		Camera.main.transform.rotation = Quaternion.LookRotation (player.transform.forward);

        endingScreen = GameObject.Find("UICanvas/EndingScreen");
        endingScreen.SetActive(false);

	}

	public void alignCinemaRoomToPlayer()
	{
		this.transform.position = cinemaRoomPivotPoint.transform.position;
		this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
	}

	public void setTime()
	{
		Time.timeScale = timeSpeed;
	}

	public void setLevelCompleted()
	{
		gameState = "inEnding";
	}

	public string getGameState()
	{
		return gameState;
	}

	void getAllDailyMissionImages()
	{
		foreach (RawImage ri in allUIImages) {
			if (ri.transform.parent.name == "DailyMissionImage" || ri.transform.name == "DailyMissionImage")
				allDailyMissionImages.Add (ri);
		}
	}

	void getAllInstructionImages()
	{
		foreach (RawImage ri in allUIImages) {
			if (ri.transform.parent.name == "GamepadInstructionImage" || ri.transform.name == "KeyboardAndMouseInstructionImage")
				allInstructionImages.Add (ri);
		}
	}

	void getAllEndingScreenImages()
	{
		foreach (RawImage ri in allUIImages) {
			if (ri.transform.parent.name == "EndingScreen" || ri.transform.name == "EndingScreen")
				allEndingScreenImages.Add (ri);
		}
	}

	void playSelectSound()
	{
		//		AudioListener.volume = 0.2f;
		//myAudioSource.clip = audioSelect;
		//myAudioSource.Play ();
	}

	void changeInstructionImage()
	{
		// Switches the states of controllers. Doesn't really work when we have more than 2 controlling styles
		gamepadInstructionImage.SetActive (!gamepadInstructionImage.activeInHierarchy);
		keyboardAndMouseInstructionImage.SetActive (!keyboardAndMouseInstructionImage.activeInHierarchy); 
	}

	void disableInstuctionImages()
	{
		gamepadInstructionImage.SetActive (false);
		keyboardAndMouseInstructionImage.SetActive (false); 
	}

	IEnumerator delayedDisableInstuctionImages(float delay)
	{
		yield return new WaitForSeconds (delay);

		gamepadInstructionImage.SetActive (false);
		keyboardAndMouseInstructionImage.SetActive (false); 
	}

	IEnumerator delayedDisableDailyMissionImages(float delay)
	{
		yield return new WaitForSeconds (delay);

		foreach (RawImage ri in allDailyMissionImages)
		{
			StartCoroutine (fadeImage (ri, 0, globalFadingDuration, 0));
		}
	}

	void enableInstuctionImages()
	{
		foreach (RawImage ri in allInstructionImages) {
			ri.canvasRenderer.SetAlpha (1f);		
		}

		if (controllerIndex == 1) // If gamepad 
			gamepadInstructionImage.SetActive (true);
		else // If keyboard and mouse
			keyboardAndMouseInstructionImage.SetActive (true); 
	}

	void showDailyMission()
	{
		dailyMissionImage.SetActive (true);
	}

    void showEnding()
    {
        endingScreen.SetActive(true);
    }

	IEnumerator delayedShowDailyMission(float delay)
	{
		yield return new WaitForSeconds (delay);
		dailyMissionImage.SetActive (true);
	}

	// Fades alpha of a given image to given alpha with given duration and given delay till start
	IEnumerator fadeImage(RawImage ri, float alpha, float fadeDuration, float delayTillStart)
	{
		StartButtonEnabled = false;
		yield return new WaitForSeconds (delayTillStart);

		ri.CrossFadeAlpha (alpha, fadeDuration, false);
		StartButtonEnabled = true;
	}

	// Fades alpha of a given text to given value with given duration and delay till start
	IEnumerator fadeText(Text t, float alpha, float fadeDuration, float delayTillStart)
	{
		StartButtonEnabled = false;
		yield return new WaitForSeconds (delayTillStart);

		t.CrossFadeAlpha (alpha, fadeDuration, false);
		StartButtonEnabled = true;
	}

	IEnumerator delayedDisableImage(RawImage ri, float delayTillStart)
	{
		yield return new WaitForSeconds (delayTillStart);

		ri.enabled = false;
	}

	IEnumerator enableStartButton(float delay)
	{
		yield return new WaitForSeconds (delay);
		StartButtonEnabled = true;
	}

	IEnumerator delayedDisableCinemaRoomStuff(float delay)
	{
		yield return new WaitForSeconds (delay);
		cinemaCamera.enabled = false;
	}

	IEnumerator delayedEnableUICamera(float delay)
	{
		yield return new WaitForSeconds (delay);
		uiCamera.enabled = true;
	}

	IEnumerator delayedEnableCinemaRoomStuff(float delay)
	{
		yield return new WaitForSeconds (delay);

		enableInstuctionImages ();
		alignCinemaRoomToPlayer ();
		cinemaCamera.enabled = true;

		if (!playerControl.inverted)
			GameObject.Find("InvertModeState").GetComponent<Text>().text = "Off";
		else
			GameObject.Find("InvertModeState").GetComponent<Text>().text = "On";
	}

	IEnumerator delayedDisablePlayerMovement(float delay)
	{
		yield return new WaitForSeconds (delay);

        playerControl.setZeroMovementForce();
		backupPosition = playerControl.transform.position;
		backupRotation = playerControl.transform.rotation;
		backupCameraRotationX = Camera.main.transform.rotation;

		Camera.main.transform.rotation = Quaternion.identity;

		playerControl.disableHands ();
		player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		player.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		uiCamera.enabled = false;
	}

	IEnumerator delayedEnablePlayerMovement(float delay)
	{

		yield return new WaitForSeconds (delay);
		uiCamera.enabled = true;
        playerControl.setMaxMovementForce();
		playerControl.transform.position = backupPosition;
		playerControl.transform.rotation = backupRotation;
		Camera.main.transform.rotation = backupCameraRotationX;
		playerControl.enabledHands ();
	}

	IEnumerator scanForSpeechBubbles(float delay)
	{
		if (Physics.BoxCast (uiCamera.transform.position, new Vector3 (2, 2, 2), uiCamera.transform.forward, out hitInfo, uiCamera.transform.rotation, 15f, speechLayerMasks))
		{
			if (hitInfo.transform.GetComponentInChildren<SpeechBubble> () != null)
			{
				closestHit = hitInfo;
				closestSpeech = hitInfo.transform.GetComponentInChildren<SpeechBubble> ();
				speechBank.setFocusedSpriteForOne(closestSpeech);
			}
		}

		yield return new WaitForSeconds (delay);
		scanGoing = false;
	}

	void Update ()
	{
		// If game hasn't started yet
		if (gameState == "") {

			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled)
			{
				controller = controllers [controllerIndex];
				InputManagerX.SetInputConfiguration (controller, PlayerID.One);

				gameState = "inBeginning";
			}
		}

		// If moving to daily mission screen from start
		if (gameState == "inBeginning") 
		{
			if (!beginingStarted) 
			{
				// To make this run this only once
				beginingStarted = true;
				StartButtonEnabled = false;

//				disableInstuctionImages ();
//				showDailyMission ();
//				delayed
				//				allButStartControlsDisabled = true;

				// These fadings are started here because starting them below would trigger them from previous enter/start press.
				// Thus these don't happen until enter/start is pressed below

				// Fade away all instruction images immediately during 1 second
				foreach (RawImage ri in allInstructionImages)
				{
					StartCoroutine (fadeImage (ri, 0, globalFadingDuration, 0));
				}

				StartCoroutine (delayedShowDailyMission (globalFadingDuration));

				StartCoroutine (fadeImage (blackScreenImage, 255, globalFadingDuration, 0));	
				StartCoroutine (enableStartButton (globalFadingDuration));
				StartCoroutine (fadeImage (blackScreenImage, 1, globalFadingDuration, globalFadingDuration));	
			}

			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled) 
			{
				// Volume to 1
				AudioListener.volume = 1f;

				// Fade away text components under DailyMissionImage
				foreach (Text t in GameObject.Find("DailyMissionImage").GetComponentsInChildren<Text>())
					t.CrossFadeAlpha (0, globalFadingDuration, false);				

				// Start playing background music from camera. Should work with both normal and VR player
				if (musicEnabled)
					mainCameraMusicPlayer.PlayBackgroundMusic ();

				allButStartControlsDisabled = false;

				StartButtonEnabled = false;
				StartCoroutine (enableStartButton (globalFadingDuration));

				StartCoroutine (delayedDisableDailyMissionImages (globalFadingDuration));

				StartCoroutine (delayedDisableCinemaRoomStuff (globalFadingDuration));

				gameState = "inGameplay";
                playerControl.setMaxMovementForce();

				StartCoroutine (fadeImage (blackScreenImage, 255, globalFadingDuration, 0));	
				StartCoroutine (delayedEnableUICamera(globalFadingDuration));
				StartCoroutine (fadeImage (blackScreenImage, 1, globalFadingDuration, globalFadingDuration));	
			}
		}

		// When returning to gameplay from options
		if (gameState == "inControlOptions") 
		{

			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled && gameState != " inGameplay"|| Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled)
			{

				// Options stuff
				controller = controllers [controllerIndex];
				InputManagerX.SetInputConfiguration (controller, PlayerID.One);
				//				disableInstuctionImages ();
				StartCoroutine(delayedDisableInstuctionImages(globalFadingDuration));

				gameState = "inGameplay"; // This seems to work better when it's before the delays

				// Disabling cinema room stuff
				StartCoroutine (fadeImage (blackScreenImage, 255, globalFadingDuration, 0));	
				StartCoroutine(delayedDisableCinemaRoomStuff (globalFadingDuration));
				StartCoroutine (fadeImage (blackScreenImage, 0, globalFadingDuration, globalFadingDuration));

				// Enabling player movement stuff
				StartCoroutine(delayedEnablePlayerMovement(globalFadingDuration));
			}			
		}

		// When going to options
		else if (gameState == "inGameplay") 
		{

			// If Enter or Start is pressed
			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled) 
			{
				// Disabling player movement stuff
				StartCoroutine(delayedDisablePlayerMovement(globalFadingDuration));

				gameState = "inControlOptions"; // This seems to work better when it's before the delays

				// Delayed images
				StartCoroutine (fadeImage (blackScreenImage, 255, globalFadingDuration, 0));	
				StartCoroutine (fadeImage (blackScreenImage, 0, globalFadingDuration, globalFadingDuration));

				// Enabling cinema room stuff
				StartCoroutine(delayedEnableCinemaRoomStuff(globalFadingDuration));

				// Options stuff

				/*
				// Update invert mode text each time options is opened. Otherwise it can show old state from different input modes
				if (!playerControl.inverted)
					GameObject.Find("InvertModeState").GetComponent<Text>().text = "Off";
				else
					GameObject.Find("InvertModeState").GetComponent<Text>().text = "On";
				*/
			} 

			// Scan every speechScanInverval for possible speechbubbles
			if (!scanGoing)
			{
				StartCoroutine(scanForSpeechBubbles(speechScanInterval));
				scanGoing = true;
			}

			// If A on game pad or F on keyboard is pressed
			if (InputManagerX.GetButtonDown ("A") || Input.GetKeyDown(KeyCode.F) && allButStartControlsDisabled == false) 
			{

				// If there's a focused speech, force fade it away
				if (closestSpeech != null) 
				{
//                  closestSpeech.setBeFullyTransparentOff();
                    closestSpeech.callFadeAlphaDown();
                    closestSpeech.enableForceFadingAway();
				}
			}				
		}

		// If the level was completed
		if (gameState == "inEnding")
		{
			// Check that this is done only once
			if (!endScreenStarted) 
			{
				endScreenStarted = true;

                // Show everythign that EndingScreen contains
                showEnding();

				// Disable movement inputs
				player.GetComponent<PlayerControl> ().enabled = false;
				// Disable camera rotation
				player.GetComponentInChildren<CameraRotation> ().enabled = false;
				// Disable all inputs elsewhere
				allButStartControlsDisabled = true;

				StartButtonEnabled = false;
            
				StartCoroutine (fadeImage (blackScreenImage, 255, 2, 0));	
				StartCoroutine(delayedEnableCinemaRoomStuff(2));
				StartCoroutine (delayedDisableInstuctionImages (2));

				StartCoroutine (fadeImage (blackScreenImage, 0, 2, 2));	

				// Fade in the ending screen pictures
				foreach (RawImage ri in allEndingScreenImages) 
				{
					ri.enabled = true; // The image has to have alpha of 1 as starting value or fading in doesn't work for reason. That's why it's disabled to begin with
					StartCoroutine (fadeImage (ri, 255, 0, 2)); 
				}

				// Fade ending texts visible
				foreach (Text t in GameObject.Find("EndingScreen").GetComponentsInChildren<Text>())
				{
					StartCoroutine(fadeText(t, 255, 2, 2));
				}
			}

			// If enter or start is pressed
			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled)
			{
				// Fade ending texts visible
                foreach (Text t in GameObject.Find("EndingScreen").GetComponentsInChildren<Text>())
				{
					StartCoroutine(fadeText(t, 0, 1, 0));
				}
				gameState = "inCredits";
			}
		}

		// If moving from ending to credits
		if (gameState == "inCredits") 
		{
			// Run these only once
			if (!creditsStarted)
			{
				creditsStarted = true;	

				foreach (RawImage ri in allUIImages)
				{
					if (ri != blackScreenImage)
						StartCoroutine (fadeImage (ri, 0, 1, 0));
				}

				creditText.enabled = true;

				// Disable buttons for 3 seconds before restarting is possible
				StartButtonEnabled = false;
				StartCoroutine (enableStartButton (5));
			}

			// Scroll the credits up
			creditText.transform.Translate (Vector3.up * 1 * Time.deltaTime);

			// Enter, backspace, start or back restarts the game
			if (Input.GetKeyUp (KeyCode.Return) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button7) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Joystick1Button6) && StartButtonEnabled || Input.GetKeyUp (KeyCode.Backspace) && StartButtonEnabled) {
				SceneManager.LoadScene ("CockpitScene");
			}
		}

		// If in options
		//		if (Time.timeScale == 0 && gameState == "inControlOptions" ||  gameState == "")  {

		if (gameState == "inControlOptions" ||  gameState == "")  
		{

			// If Back/Backspace pressed, restart the scene
			if (Input.GetKeyUp (KeyCode.Joystick1Button6) || Input.GetKeyUp (KeyCode.Backspace))
			{
				SceneManager.LoadScene ("CockpitScene");
			}

			// Changing input style
			if ((Input.GetKeyUp (KeyCode.Mouse0) || Input.GetKeyUp (KeyCode.Joystick1Button4)) && allButStartControlsDisabled == false) 
			{
				if (controllerIndex >= 1)
				{
					controllerIndex = controllerIndex - 1;
					changeInstructionImage ();
					playSelectSound ();
				} else
				{
					controllerIndex = controllers.Count - 1;
					changeInstructionImage ();
					playSelectSound ();
				}
			}

			if ((Input.GetKeyUp (KeyCode.Mouse1) || Input.GetKeyUp (KeyCode.Joystick1Button5)) && allButStartControlsDisabled == false)
			{
				if (controllerIndex <= controllers.Count - 2) 
				{
					controllerIndex = controllerIndex + 1;
					changeInstructionImage ();
					playSelectSound ();
				} else
				{
					controllerIndex = 0;
					changeInstructionImage ();
					playSelectSound ();
				}
			}

			// Enable toggle hand movement controls for gamepad. Disable otherwise
			//if (controllerIndex == 1)
				//playerControl.toggleControl = true;
			//else
				//playerControl.toggleControl = false;



			controllerText.GetComponent<Text> ().text = controllers [controllerIndex];

			// Extra input options

			// VR modes 
			/*
			if (InputManagerX.GetButtonDown ("X") && allButStartControlsDisabled == false) {
				if (player.GetComponent<PlayerControl> ().seatedVR) {
					player.GetComponent<PlayerControl> ().seatedVR = false;
					vrText.GetComponent<Text> ().text = "StandingVR";

					playSelectSound ();
				} else {
					player.GetComponent<PlayerControl> ().seatedVR = true;
					vrText.GetComponent<Text> ().text = "SeatedVR";

					playSelectSound ();
				}
			}
			*/

			// Inverted hand movement
			if (InputManagerX.GetButtonDown ("Y") || Input.GetKeyUp (KeyCode.I) && allButStartControlsDisabled == false) 
			{
				if (player.GetComponent<PlayerControl> ().inverted)
				{
					player.GetComponent<PlayerControl> ().inverted = false;
					GameObject.Find("InvertModeState").GetComponent<Text>().text = "Off";
					playSelectSound ();
				} else
				{
					player.GetComponent<PlayerControl> ().inverted = true;
					GameObject.Find("InvertModeState").GetComponent<Text>().text = "On";
					playSelectSound ();
				}
			}

			// Toggled hand movement
			// NOW ALWAYS ON BY DEFAULT IN PLAYER
			/*
			if (InputManagerX.GetButtonDown ("A") || Input.GetKeyUp (KeyCode.T) && allButStartControlsDisabled == false)
			{
				if (player.GetComponent<PlayerControl> ().toggleControl)
				{
					player.GetComponent<PlayerControl> ().toggleControl = false;
					GameObject.Find("ToggledHandMovementState").GetComponent<Text>().text = "Off";
					playSelectSound ();
				} else
				{
					player.GetComponent<PlayerControl> ().toggleControl = true;
					GameObject.Find("ToggledHandMovementState").GetComponent<Text>().text = "On";
					playSelectSound ();
				}
			}
			*/

			// Motion controls
			/*
			if (InputManagerX.GetButtonDown ("B") && allButStartControlsDisabled == false)
			{
				if (player.GetComponent<PlayerControl> ().motionControl) {
					player.GetComponent<PlayerControl> ().motionControl = false;
					motionText.GetComponent<Text> ().text = "Motion Off";
				} else {
					player.GetComponent<PlayerControl> ().motionControl = true;
					motionText.GetComponent<Text> ().text = "Motion On";
				}
			}
			*/
		}

		// Somewhat global hotkeys

		// If Esc is pressed, quit the game. Works always
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Debug.Log ("Esc pressed");
			if (Application.isPlaying) {
				Debug.Log ("Shutting down the game");

				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#endif

				Application.Quit ();
			}
		}

		// If F1 is pressed, toggle backgroundmusic. Works always
		if (Input.GetKeyUp (KeyCode.F1))
		{
			// Flip music playing state
			musicEnabled = !musicEnabled;

			AudioSource tempAudioSource = player.GetComponentInChildren<AudioSource>();

			if (musicEnabled)
			{	
				tempAudioSource.volume = 0.1f;
				tempAudioSource.Play ();
			} else
			{
				tempAudioSource.volume = 0.1f;
				tempAudioSource.Stop ();
			}
		}

		// Super secret code
//		if (Input.GetKeyDown (KeyCode.F2) && Input.GetKeyDown (KeyCode.F4))
        if (Input.GetKeyDown (KeyCode.F2))
            
		{
			setLevelCompleted ();
			Debug.Log ("Level completed");
		}

		if (Input.GetKeyDown (KeyCode.F5))
		{
			if (randomTrailerObject != null)
			{
				randomTrailerObject.GetComponent<RandomTrailerObjectSettings>().resetPosition();
			}
		}

		if (Input.GetKeyDown (KeyCode.F6))
		{
			if (randomTrailerObject != null)
			{
				randomTrailerObject.GetComponent<RandomTrailerObjectSettings>().resetRotation();
			}
		}

	}
}

