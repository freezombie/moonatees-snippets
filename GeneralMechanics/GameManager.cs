using UnityEngine;
using System.Collections;
using Valve.VR; // For listening Vive controls
using NewtonVR; // To access for example NCRHand which is hidden inside a namespace
using UnityEngine.SceneManagement; // For scene reloading
using UnityEngine.UI; // For Text and other UI elements
using UnityEngine.Audio;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour
{
	public Camera cinemaCamera;
	public GameObject player;
	public AlignToPlayerView alignToPlayerView;

	public string gameState = "InOptions";
	public bool inGameplay = false;
	public bool inOptions = true;
	public bool inEnding = false;

    public bool levelLoadDone = false;
    public AsyncOperation async;

	private Valve.VR.EVRButtonId TriggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId MenuButton = EVRButtonId.k_EButton_ApplicationMenu;

	public bool triggerPressed = false;

	public SteamVR_Controller.Device Controller1;
	private bool controller1Enabled = true;
	public SteamVR_Controller.Device Controller2;
	private bool controller2Enabled = true;

	public GameObject physicalLeftController;
	public GameObject physicalRightController;

	public NVRHand leftController;
	public NVRHand rightController;

	public BoxCollider[] physicalLeftControllerColliders;
	public BoxCollider[] physicalRightControllerColliders;

    // Dirty handles for end screen stuff
	public GameObject optionsScreen;
	public GameObject endingScreen;
	public GameObject endingCredits;
	public bool creditsCanRoll = false;

	public bool controllerScanGoing = false;
	public bool controllerCheckGoing = false;

	// How often a boxcast is made to scan for speech boxes
	public float speechScanInterval = 0.5f;

	public RaycastHit closestHit;
	public SpeechBubble closestSpeech;
	public SpeechBank speechBank;
	public RaycastHit hitInfo;
	public LayerMask speechLayerMasks;

	public AudioClip dayCompleteAudio;
	public AudioClip gameplayAudio;

	private GameObject missingLeftFlipper;
	private GameObject missingRightFlipper;

    public MeshRenderer leftFlipperRenderer;
    public MeshRenderer rightFlipperRenderer;
    public bool presentFlippersFound = false;

	public SteamVR_TrackedObject steamVRLeftController;
	public SteamVR_TrackedObject steamVRRightController;

	public bool leftControllerValidState = false;
	public bool rightControllerValidState = false;

	public bool movementControlsEnabled = false;

	public bool deviceIndexesSet = false;
    public bool loadingStateOnCooldown = false;

    // Settings stuff
    public LayerMask settingsLayerMask;
    private RaycastHit hit;
    public bool touchRotationModeState, rotationMomentumModeState, rotationMomentumDeaccelerationState = false;
    private bool pointingParticlesSpawned = false;
    private GameObject rightUberRay, leftUberRay;
    private ParticleSystem rightSystem, leftSystem;
    private float defaultMovementSpeed = 1f;
    private float defaultRotationSpeed = 1f;
    private float currentVolume, currentMovementSpeed = 4f, currentRotationSpeed = 7f;
    public Text masterVolumeText, movementSpeedText, rotationSpeedText;
    public AudioMixer mixer;
    private Transform chosenController;

    public Color notHittingColor, hittingColor;

    // Loading screen stuff
    public Text loadingText;
    public GameObject loadingManatee1, loadingManatee2;
    public bool loading = false; // Used to check when loading text should be updated


    // Public gameobjects for checkboxes
    public GameObject exitPrompt;
    public GameObject touchRotationModeCheck;
    public GameObject rotationMomentumModeCheck;
    public GameObject rotationMomentumDeacceleration;
    public GameObject rotationMomentumDeaccelerationModeCheck;

    public GameObject currentMissionImage;
    public GameObject loadingImage;

    public bool creditsRolling = false;
    public bool menuButtonsEnabled = true;
    public bool menuButtonsDisableCooldown = false;


    // Fading related things
    public Tonemapping headCameraTone;
    public Tonemapping cinemaCameraTone;
    private float fadeTimer = 0;
    private bool faderGoing = false;
    private bool fadeInGoing = false;
    private bool fadeOutGoing = false;
    private float newValue = 0;
    private float fadeOneWayDuration = 0.1f;
    private bool changeCinemaCameraState = false;

    public GameObject day1button;
    public GameObject day2button;

//    private bool menuButtonsEnabled = true;

    private bool allDoneInStart = false;

	// Use this for initialization
	void Start () 
	{
        // Reset this in case the scene gets loaded again and it doesn't get set to default state
        creditsRolling = false;

        // Set correct mission image depending on the scene
        Scene scene = SceneManager.GetActiveScene();

        loadingImage = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/LoadingImage").gameObject;

        if (scene.name == "Tutorial")
        {
            currentMissionImage = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/FirstMission").gameObject;
            GameObject temp = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/SecondMission").gameObject;
            temp.SetActive(false);
            loadingImage.SetActive(false);
        }

        else if (scene.name == "CockpitScene")
        {
            currentMissionImage = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/SecondMission").gameObject;
            GameObject temp = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/FirstMission").gameObject;
            temp.SetActive(false);
            loadingImage.SetActive(false);
        }


		cinemaCamera = GameObject.FindGameObjectWithTag ("CinemaCamera").GetComponent<Camera>();

        if (gameState == "InGameplay")
            cinemaCamera.gameObject.SetActive(false);
        
		player = GameObject.FindGameObjectWithTag ("Player");

		speechBank = this.GetComponentInChildren<SpeechBank>();

		alignToPlayerView = player.GetComponentInChildren<AlignToPlayerView> ();
		alignToPlayerView.alignToPlayerView ();

		physicalLeftController = this.transform.parent.FindChild ("Controller (left)").gameObject;
		physicalRightController = this.transform.parent.FindChild ("Controller (right)").gameObject;

		missingLeftFlipper = Camera.main.transform.FindChild ("MissingLeftFlipper").gameObject;
		missingRightFlipper = Camera.main.transform.FindChild ("MissingRightFlipper").gameObject;



        movementSpeedText = GameObject.FindGameObjectWithTag("Settings").transform.FindChild("MovementSpeed/MovementSpeedText").GetComponent<Text>();
        rotationSpeedText = GameObject.FindGameObjectWithTag("Settings").transform.FindChild("RotationSpeed/RotationSpeedText").GetComponent<Text>();
        masterVolumeText = GameObject.FindGameObjectWithTag("Settings").transform.FindChild("MasterVolume/VolumeText").GetComponent<Text>();

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume",PlayerPrefs.GetFloat("MasterVolume"));
        }

        bool result = mixer.GetFloat("MasterVolume", out currentVolume);
        if (result)
        {
            if (currentVolume > -20)
                masterVolumeText.text = (100 - 10 * (int)((currentVolume / - 5 ))).ToString();
            else
                masterVolumeText.text = (100 - 10 * (int)((currentVolume / - 10 ) + 2)).ToString();            
        }
        else
            masterVolumeText.text = "-";

        movementSpeedText.text = currentMovementSpeed.ToString();
        rotationSpeedText.text = currentRotationSpeed.ToString();

        if(PlayerPrefs.HasKey("MovementSpeed")) // could be anything really. If movementspeed has been saved, everything else should be saved as well
        {
           currentMovementSpeed = PlayerPrefs.GetFloat("MovementSpeed");
           movementSpeedText.text = PlayerPrefs.GetFloat("MovementSpeed").ToString(); // Just modify these to match the real ones at some point.           
        }
        if (PlayerPrefs.HasKey("RotationSpeed"))
        {
            currentRotationSpeed = PlayerPrefs.GetFloat("RotationSpeed");
            rotationSpeedText.text = PlayerPrefs.GetFloat("RotationSpeed").ToString();
        }
        findControllers(); // this sets the default moving speed
        day1button = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/LoadDay1").gameObject;
        day2button = this.transform.parent.FindChild("CinemaRoomPivot/CinemaRoom/UICanvas/LoadDay2").gameObject;

        day1button.SetActive(false);
        day2button.SetActive(false);

        if (PlayerPrefs.HasKey("TutorialPassed"))
        {
            day1button.SetActive(true);
            day2button.SetActive(true);
        }

        allDoneInStart = true;
	}

	public void setMovementControlState(bool state)
	{
		movementControlsEnabled = state;
	}

	public bool getMovementControlState()
	{
		return movementControlsEnabled;
	}

	IEnumerator scanForSpeechBubbles(float delay)
	{
		if (Physics.BoxCast (Camera.main.transform.position, new Vector3 (2, 2, 2), Camera.main.transform.forward, out hitInfo, Camera.main.transform.rotation, 15f, speechLayerMasks))
		{
			if (hitInfo.transform.GetComponentInChildren<SpeechBubble> () != null)
			{
				closestHit = hitInfo;
				closestSpeech = hitInfo.transform.GetComponentInChildren<SpeechBubble> ();
				speechBank.setFocusedSpriteForOne(closestSpeech);
			}
		}

		yield return new WaitForSeconds (delay);
		controllerScanGoing = false;
	}

	IEnumerator scanForVRControllers(float delay)
	{
		yield return new WaitForSeconds (delay);

//		Debug.Log ("Scanning...");
	
		findControllers ();
		setDeviceIndexes ();
		resetHandColliders ();

		controllerScanGoing = false;
	}

	IEnumerator checkForMissingControllers(float delay)
	{
		yield return new WaitForSeconds (delay);

		if (!deviceIndexesSet)
			setDeviceIndexes();

		if (physicalLeftController == null)
			physicalLeftController = this.transform.parent.FindChild ("Controller (left)").gameObject;
		if (physicalRightController == null)
			physicalRightController = this.transform.parent.FindChild ("Controller (right)").gameObject; 

		if (steamVRLeftController == null)
			steamVRLeftController = physicalLeftController.GetComponent<SteamVR_TrackedObject> ();
		if (steamVRRightController == null)				
			steamVRRightController = physicalRightController.GetComponent<SteamVR_TrackedObject> ();

        if (!presentFlippersFound && player != null)
        {
            if (player.transform.Find("Controller (left) [Physical]/CustomColliders/VRFlipper") != null)
            {
                leftFlipperRenderer = player.transform.Find("Controller (left) [Physical]/CustomColliders/VRFlipper").gameObject.GetComponentInChildren<MeshRenderer>();
            }

            if (player.transform.Find("Controller (right) [Physical]/CustomColliders/VRFlipper") != null)
            {
                rightFlipperRenderer = player.transform.Find("Controller (right) [Physical]/CustomColliders/VRFlipper").gameObject.GetComponentInChildren<MeshRenderer>();
            }

            if (leftFlipperRenderer != null && rightFlipperRenderer != null)
            {
                presentFlippersFound = true;
            }
        }

		leftControllerValidState = steamVRLeftController.isValid;
		rightControllerValidState = steamVRRightController.isValid;

        findControllers();

		if (!leftControllerValidState)
		{
			StartCoroutine(scanForVRControllers(0f));

//            Debug.Log ("Left controller OR Vive headset not visible for beacons!");

			missingLeftFlipper.SetActive(true);
            if (leftFlipperRenderer != null)
                leftFlipperRenderer.enabled = false;
		}
		else
		{
			missingLeftFlipper.SetActive(false);
            if (leftFlipperRenderer != null)
                leftFlipperRenderer.enabled = true;
		}


		if (!rightControllerValidState)
		{
			StartCoroutine (scanForVRControllers (0f));

			missingRightFlipper.SetActive(true);
            if (rightFlipperRenderer != null)
                rightFlipperRenderer.enabled = false;
		}
		else
		{
			missingRightFlipper.SetActive(false);
            if (rightFlipperRenderer != null)
                rightFlipperRenderer.enabled = true;
		}

		controllerCheckGoing = false;

	}

	public void findControllers()
	{
        if (leftController == null)
        {
            if (GameObject.FindGameObjectWithTag("LeftHand"))
            {
                leftController = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<NVRHand>();
//                float curSpeed = float.Parse(movementSpeedText.text);
//                leftController.ChangeMovementSpeed(curSpeed);

            }
        }
        else
        {
            float curSpeed = float.Parse(movementSpeedText.text);
            leftController.ChangeMovementSpeed(curSpeed);
        }

        if (rightController == null)
        {
            if (GameObject.FindGameObjectWithTag("RightHand"))
            {
                rightController = GameObject.FindGameObjectWithTag("RightHand").GetComponent<NVRHand>();
//                float curRotSpeed = float.Parse(rotationSpeedText.text);
//                rightController.ChangeRotationSpeed(curRotSpeed);
            }
            
        }
        else
        {
            float curRotSpeed = float.Parse(rotationSpeedText.text);
            rightController.ChangeRotationSpeed(curRotSpeed);
        }
	}

	public void setDeviceIndexes()
	{
		SteamVR_ControllerManager steamVRControllerManager = player.GetComponent<SteamVR_ControllerManager> ();

		steamVRControllerManager.Refresh ();

		if ((int)steamVRControllerManager.leftIndex != -1 && (int)steamVRControllerManager.rightIndex != -1) 
		{
			Controller1 = SteamVR_Controller.Input ((int)steamVRControllerManager.leftIndex);
			Controller2 = SteamVR_Controller.Input ((int)steamVRControllerManager.rightIndex);	

			deviceIndexesSet = true;
		}
	}

	public string getGameState()
	{
		return gameState;
	}

	public void setGameState(string gameState)
	{
		this.gameState = gameState;
	}

	public void toggleCinemaCameraOnOff()
	{
		alignToPlayerView.alignToPlayerView ();

		string currentGameState = gameState;

        if (currentGameState == "InOptions" && !loading && leftControllerValidState && rightControllerValidState && allDoneInStart && !faderGoing)
		{
			gameState = "InGameplay";

			// #purkkaviritys. Doesn't even seem to replay itself when spammed
			this.transform.parent.GetComponentInChildren<MusicPlayer> ().PlayBackgroundMusic ();

            if (exitPrompt.activeInHierarchy)
                exitPrompt.SetActive(false);

			inOptions = false;
			inGameplay = true;

            // Set this on so we toggle cinema camera state during black screen
            changeCinemaCameraState = true;
            StartCoroutine(fadeBlackIn());

			setVRControlsOn();
			setMovementControlState(true);
            PlayerPrefs.SetFloat("MovementSpeed", currentMovementSpeed);   // MODIFY THESE TO MATCH THE REAL ONES
            PlayerPrefs.SetFloat("RotationSpeed", currentRotationSpeed);

            float tempVolume;
            mixer.GetFloat("MasterVolume", out tempVolume);
            PlayerPrefs.SetFloat("MasterVolume", tempVolume);
            return;
		}

        else if (currentGameState == "InGameplay"  && allDoneInStart && !loading && !faderGoing)
		{
			gameState = "InOptions";
			inOptions = true;
			inGameplay = false;

            changeCinemaCameraState = true;
            StartCoroutine(fadeBlackIn());

			setVRControlsOff();
			setMovementControlState(false);

            // These are to reset controller1 and controller2 indexes in case controllers were turned off and on again and they switch sides
            findControllers ();
            setDeviceIndexes ();
            resetHandColliders ();

			return;
		}

		else if (currentGameState == "InEnding")
		{
			inOptions = true;
			inGameplay = false;
//			inEnding = true;
			cinemaCamera.gameObject.SetActive (true);

			return;
		}
	}

	public void setVRControlsOff()
	{
		findControllers ();

		if (leftController != null && leftController.enabled == true)
		{
			leftController.enabled = false;
			if (GameObject.Find ("Controller (left) [Physical]").GetComponentsInChildren<BoxCollider> () != null)
			{
				physicalLeftControllerColliders = GameObject.Find ("Controller (left) [Physical]").GetComponentsInChildren<BoxCollider> ();

				foreach (BoxCollider bc in physicalLeftControllerColliders)
				{
					bc.enabled = false;
				}
			}
			else
				Debug.Log ("Couldn't find left physical hand!");
		}

		if (rightController != null && rightController.enabled == true)
		{
			rightController.enabled = false;
			if (GameObject.Find ("Controller (right) [Physical]").GetComponentsInChildren<BoxCollider> () != null)
			{
				physicalRightControllerColliders = GameObject.Find ("Controller (right) [Physical]").GetComponentsInChildren<BoxCollider> ();

				foreach (BoxCollider bc in physicalRightControllerColliders)
				{
					bc.enabled = false;
				}
			}
			else
				Debug.Log ("Couldn't find right physical hand!");
		}
	}

	public void setVRControlsOn()
	{
		findControllers ();

		if (leftController != null && leftController.enabled == false)
		{
			leftController.enabled = true;
			physicalLeftControllerColliders = GameObject.Find ("Controller (left) [Physical]").GetComponentsInChildren<BoxCollider> ();



			foreach (BoxCollider bc in physicalRightControllerColliders)
			{
                if (bc != null) // bug catch? Sometimes it still tries to access destoryed ones
				    bc.enabled = true;
			}
		}

		if (rightController != null && rightController.enabled == false)
		{
			rightController.enabled = true;
			physicalRightControllerColliders = GameObject.Find ("Controller (right) [Physical]").GetComponentsInChildren<BoxCollider> ();

			foreach (BoxCollider bc in physicalRightControllerColliders)
			{
				bc.enabled = true;
			}
		}
	}



	public void setDayComplete()
	{
		gameState = "InEnding";
		toggleCinemaCameraOnOff ();
	}

	public void resetHandColliders()
	{
		if (leftController != null)
		{
			BoxCollider[] tempLeftBoxColliders = leftController.GetComponentsInChildren<BoxCollider> ();
			foreach (BoxCollider bc in tempLeftBoxColliders) 
			{
				bc.enabled = false;
				bc.enabled = true;
			}

		}

		if (rightController != null)
		{
			BoxCollider[] tempRightBoxColliders = rightController.GetComponentsInChildren<BoxCollider> ();
			foreach (BoxCollider bc in tempRightBoxColliders) 
			{
				bc.enabled = false;
				bc.enabled = true;
			}
		}
	}

    void FixedUpdate()
    {
        if (faderGoing)
        {
            if (fadeOutGoing)
                newValue = 0 + fadeTimer / fadeOneWayDuration;
            else if (fadeInGoing)
                newValue = 1.2f - fadeTimer / fadeOneWayDuration;

            headCameraTone.exposureAdjustment = newValue;
            cinemaCameraTone.exposureAdjustment = newValue;

            fadeTimer += Time.deltaTime;
        }

        if (!faderGoing && newValue != 1.2f)
        {
            headCameraTone.exposureAdjustment = 1.2f;
            cinemaCameraTone.exposureAdjustment = 1.2f;     
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
		// Game state management

        if (loading)
        {
            StartCoroutine(getLoadingState(0.1f));
            loadingStateOnCooldown = true;
        }


		if (!controllerCheckGoing) 
		{
			StartCoroutine (checkForMissingControllers (2f));
			controllerCheckGoing = true;
		}


		if (gameState == "InOptions")
		{
			if (physicalLeftController != null && physicalRightController != null)
			{
				if (physicalLeftController.activeSelf && physicalRightController.activeSelf && Controller1 != null)
				{
                    // Summon pointing particle rays
                    if (!pointingParticlesSpawned)
                    {
                        if (leftController != null && rightController != null)
                        {
                            rightUberRay = Instantiate(Resources.Load("RaycastHit"), physicalRightController.transform.position, physicalRightController.transform.rotation) as GameObject;
                            leftUberRay = Instantiate(Resources.Load("RaycastHit"), physicalLeftController.transform.position, physicalLeftController.transform.rotation) as GameObject;

                            rightSystem = rightUberRay.GetComponent<ParticleSystem>();
                            leftSystem = leftUberRay.GetComponent<ParticleSystem>();

                            pointingParticlesSpawned = true;
                        }
                    }
                    else
                    {
                        // Update ray loc and rots
                        rightUberRay.transform.position = physicalRightController.transform.position;
                        rightUberRay.transform.rotation = physicalRightController.transform.rotation;
                        leftUberRay.transform.position = physicalLeftController.transform.position;
                        leftUberRay.transform.rotation = physicalLeftController.transform.rotation;
                    

                        // Check when hitting UI elements for right controller
                        ParticleSystem.TrailModule rightTrails = rightSystem.trails;
                        if (Physics.Raycast(physicalRightController.transform.position, physicalRightController.transform.forward, out hit, 50f, settingsLayerMask))
                        {
                            if (hit.collider.CompareTag("SettingsElement"))
                                rightTrails.colorOverLifetime = hittingColor;                       
                        }
                        else
                            rightTrails.colorOverLifetime = notHittingColor;
                    
                        // Check when hitting UI elements for left controller
                        ParticleSystem.TrailModule LeftTrails = leftSystem.trails;
                        if (Physics.Raycast(physicalLeftController.transform.position, physicalLeftController.transform.forward, out hit, 50f, settingsLayerMask))
                        {
                            if (hit.collider.CompareTag("SettingsElement"))
                                LeftTrails.colorOverLifetime = hittingColor;                       
                        }
                        else
                            LeftTrails.colorOverLifetime = notHittingColor;
                    
                        bool rightTrigger = false;
                        bool leftTrigger = false;

                        if (Controller2.GetPressDown(TriggerButton))
                        {
                            rightTrigger = true;
                            chosenController = physicalRightController.transform;
                        }

                        if (Controller1.GetPressDown(TriggerButton))
                        {
                            leftTrigger = true;
                            chosenController = physicalLeftController.transform;
                        }

                        // Button listening	
                        if (rightTrigger || leftTrigger)
                        {
                            if (Physics.Raycast(chosenController.transform.position, chosenController.transform.forward, out hit, 50f, settingsLayerMask))
                            {

                                float curVolume;
                                switch (hit.collider.name)
                                {
                                    case "TouchRotationCheckBox":
                                        touchRotationModeState = !touchRotationModeState;
                                        touchRotationModeCheck.SetActive(touchRotationModeState);
                                        rightController.setTouchRotationEnabled(touchRotationModeState);
                                        break;

                                    case "DecreaseVolume":
                                        float min = -80f;

                                        mixer.GetFloat("MasterVolume", out curVolume);

                                        if (curVolume > min && curVolume > -20)
                                        {
                                            mixer.SetFloat("MasterVolume", curVolume - 5f);
                                            currentVolume = (100 - 10 * (int)((curVolume / -5) + 1));
                                            masterVolumeText.text = currentVolume.ToString();
                                        }
                                        else if (curVolume > min && curVolume <= -20)
                                        {
                                            mixer.SetFloat("MasterVolume", curVolume - 10f);
                                            currentVolume = (100 - 10 * (int)((curVolume / -10) + 3));
                                            masterVolumeText.text = currentVolume.ToString();
                                        }
                                        break;

                                    case "IncreaseVolume":
                                        float max = 0f;
                                        mixer.GetFloat("MasterVolume", out curVolume);

                                        if (curVolume < max - 1 && curVolume >= -20)
                                        {
                                            mixer.SetFloat("MasterVolume", curVolume + 5f);
                                            currentVolume = (100 - 10 * (int)((curVolume / -5) - 1));
                                            masterVolumeText.text = currentVolume.ToString();
                                        }
                                        else if (curVolume < max && curVolume < -20)
                                        {
                                            mixer.SetFloat("MasterVolume", curVolume + 10f);
                                            currentVolume = (90 - 10 * (int)((curVolume / -10)));
                                            masterVolumeText.text = currentVolume.ToString();
                                        }
                                        break;

                                    case "DecreaseMovement":
                                        if (currentMovementSpeed > 1)
                                        {
                                            currentMovementSpeed = currentMovementSpeed - 1;
                                            movementSpeedText.text = currentMovementSpeed.ToString();
                                            float newSpeed = defaultMovementSpeed + (0.5f * currentMovementSpeed);
                                            leftController.ChangeMovementSpeed(newSpeed);
                                        }
                                        break;

                                    case "IncreaseMovement":
                                        if (currentMovementSpeed < 10)
                                        {
                                            currentMovementSpeed = currentMovementSpeed + 1;
                                            movementSpeedText.text = currentMovementSpeed.ToString();
                                            float newSpeed = defaultMovementSpeed + (0.5f * currentMovementSpeed);
                                            leftController.ChangeMovementSpeed(newSpeed);
                                        }
                                        break;

                                    case "DecreaseRotation":
                                        if (currentRotationSpeed > 4)
                                        {
                                            currentRotationSpeed = currentRotationSpeed - 1;
                                            rotationSpeedText.text = currentRotationSpeed.ToString();
                                            float newRotationSpeed = currentRotationSpeed;
                                            rightController.ChangeRotationSpeed(newRotationSpeed);
//                                            Debug.Log("new rot was: " + newRotationSpeed);
                                        }
                                        break;

                                    case "IncreaseRotation":
                                        if (currentRotationSpeed < 12)
                                        {
                                            currentRotationSpeed = currentRotationSpeed + 1;
                                            rotationSpeedText.text = currentRotationSpeed.ToString();
                                            float newRotationSpeed = currentRotationSpeed;
                                            rightController.ChangeRotationSpeed(newRotationSpeed);
                                        }
                                        break;

                                    case "ExitDoor":
                                        if (!exitPrompt.activeInHierarchy)
                                            exitPrompt.SetActive(true);
                                        else
                                            exitPrompt.SetActive(false);
                                        break;

                                    case "RotationMomentumCheckBox":
                                        rotationMomentumModeState = !rotationMomentumModeState;
                                        rotationMomentumModeCheck.SetActive(rotationMomentumModeState);
                                        rotationMomentumDeacceleration.SetActive(rotationMomentumModeState);
                                        rightController.setRotationMomentum(rotationMomentumModeState);
                                        break;

                                    case "RotationMomentumDeaccelerationCheckBox":
                                        rotationMomentumDeaccelerationState = !rotationMomentumDeaccelerationState;
                                        rotationMomentumDeaccelerationModeCheck.SetActive(rotationMomentumDeaccelerationState);
                                        rightController.setRotationMomentumDeacceleration(rotationMomentumDeaccelerationState);
                                        break;

                                    case "YesBox":
                                        Debug.Log("Shutting down the game");
                                        #if UNITY_EDITOR
                                        UnityEditor.EditorApplication.isPlaying = false;
                                        #endif
                                        Application.Quit();
                                        break;

                                    case "NoBox":
                                        if (!exitPrompt.activeInHierarchy)
                                            exitPrompt.SetActive(true);
                                        else
                                            exitPrompt.SetActive(false);
                                        break;

                                    case "LoadDay1":
                                        if (!loading && !faderGoing)
                                            loadTutorialScene();
                                        break;

                                    case "LoadDay2":
                                        if (!loading && !faderGoing)
                                            loadCockpitScene(false);
                                        break;
                                }
                            }
                            return;
                        }
                    }

                    if (Controller1.GetPressDown(MenuButton))
					{
						toggleCinemaCameraOnOff();
						return;
					}						

					if (Controller2.GetPressDown(MenuButton))
					{
						toggleCinemaCameraOnOff();
						return;
					}	
				}
			}
		}

		if (gameState == "InGameplay")
		{
			if (physicalLeftController != null && physicalRightController != null)
			{
				if (physicalLeftController.activeSelf && physicalRightController.activeSelf)
				{	
					if (Controller1.GetPressDown (MenuButton))
					{
						toggleCinemaCameraOnOff ();
						return;
					}	

					if (Controller2.GetPressDown (MenuButton))
					{
						toggleCinemaCameraOnOff ();
						return;
					}	
				}


				// Scan every speechScanInverval for possible speechbubbles
				if (!controllerScanGoing)
				{
					StartCoroutine (scanForSpeechBubbles (speechScanInterval));
					controllerScanGoing = true;
				}

				if (Controller1 != null)
				{
					if (Controller1.GetPressDown (TriggerButton))
					{
						// If there's a focused speech, force fade it away
						if (closestSpeech != null)
						{
							closestSpeech.callFadeAlphaDown ();
							closestSpeech.enableForceFadingAway ();
						}
					}
				}

				if (Controller2 != null)
				{
					if (Controller2.GetPressDown (TriggerButton))
					{
						// If there's a focused speech, force fade it away
						if (closestSpeech != null)
						{
							closestSpeech.callFadeAlphaDown ();
							closestSpeech.enableForceFadingAway ();
						}
					}
				}
			}
		}

		if (gameState == "InEnding")
		{
			if (!inEnding)
			{
                // Disable hand pointing particle thingys. They wouldn't work outside options state anyway
                Destroy(rightUberRay);
                Destroy(leftUberRay);
                pointingParticlesSpawned = false;

				inEnding = true;
				toggleCinemaCameraOnOff();
				optionsScreen.SetActive (false);
				endingScreen.SetActive (true);
                StartCoroutine(disableMenuButtons(1f));
			}
                
            if (Controller2.GetPressDown (TriggerButton) || Controller2.GetPressDown(MenuButton) || Controller1.GetPressDown (TriggerButton) || Controller1.GetPressDown(MenuButton) && menuButtonsEnabled)
			{
                if (!creditsRolling)
                {
                    currentMissionImage.SetActive(false);
                    endingScreen.SetActive (false);
                    endingCredits.SetActive (true);
                    creditsCanRoll = true;
                    StartCoroutine(disableMenuButtons(1f));
                    return;
                }

                if (creditsRolling)
                {
                    endingCredits.SetActive (false);
                    currentMissionImage.SetActive(false);
                    loadCockpitScene(false);
                    StartCoroutine(disableMenuButtons(1f));
                    return;
                }				
                return;
			}

            if (creditsCanRoll)
            {
                currentMissionImage.SetActive(false);
                endingCredits.transform.Translate(Vector3.up * 1 * Time.deltaTime);
               
                creditsRolling = true;
            }

            if (Controller2.GetPressDown (TriggerButton) || Controller2.GetPressDown(MenuButton) || Controller1.GetPressDown (TriggerButton) || Controller1.GetPressDown(MenuButton))
            {
                endingCredits.SetActive (false);
                loadCockpitScene(false);
            }
		}
        #if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.F1))
		{
			toggleCinemaCameraOnOff ();
		}
        #endif

       #if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.F2))
		{
			GameObject.FindGameObjectWithTag ("GameWorld").GetComponentInChildren<WakingScript> ().WakeUp(WakingScript.WakeType.Coffee,3);
		}
        #endif

        #if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.F3))
		{
            StartCoroutine(fadeBlackIn());
		}
        #endif

//
//		if (Input.GetKeyDown (KeyCode.F4))
//		{
//			// Something here
//
//		}

        // Exit prompt
		if (Input.GetKeyDown (KeyCode.Escape))
		{
            if (gameState == "InGameplay")
            {
                toggleCinemaCameraOnOff();
                return;
            }

            if (gameState == "InOptions")
            {
                if (!exitPrompt.activeInHierarchy)
                {
                    exitPrompt.SetActive(true);
                }
                else
                {
                    exitPrompt.SetActive(false);
                }
            }
		}

        // Confirming exit prompt
        if (Input.GetKeyDown(KeyCode.Return) && exitPrompt.activeInHierarchy)
        {
            if (Application.isPlaying)
            {
                // Exit game in editor
                Debug.Log("Shutting down the game");
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                // Exit game as a client
                Application.Quit();
            }
        }

//        if (Input.GetKeyDown (KeyCode.F5) && gameState == "InOptions")
//		{
//            loadTutorialScene();
//		}
//
//        if (Input.GetKeyDown (KeyCode.F6)  && gameState == "InOptions")
//        {
//            loadCockpitScene();
//        }

//		if (Input.GetKeyDown (KeyCode.F9))
//		{
//			leftController.ChangeMovementSpeed (-5f);
//			Debug.Log ("Speed down..");
//		}
//
//		if (Input.GetKeyDown (KeyCode.F10))
//		{
//			leftController.ChangeMovementSpeed (5f);
//			Debug.Log ("Speed up!");
//		}       
//
//        // Restart game from the tutorial
//		if (Input.GetKeyDown (KeyCode.F11))
//		{
//            Application.LoadLevel("Tutorial");
//		}

	}

    IEnumerator fadeBlackIn() 
    {        
        faderGoing = true;
        fadeInGoing = true;

        yield return new WaitForSeconds (fadeOneWayDuration);

        fadeTimer = 0;
        faderGoing = false;
        fadeInGoing = false;

        // Set cinema camera to opposite state if so requested
        if (changeCinemaCameraState)
            cinemaCamera.gameObject.SetActive(!cinemaCamera.gameObject.activeInHierarchy);

        // Start fading black out
        StartCoroutine(fadeBlackOut());
    }

    IEnumerator fadeBlackOut() 
    {        
        faderGoing = true;
        fadeOutGoing = true;

        yield return new WaitForSeconds (fadeOneWayDuration);

        fadeTimer = 0;
        faderGoing = false;
        fadeOutGoing = false;

        changeCinemaCameraState = false;
    }


    IEnumerator disableMenuButtons(float delay) 
    {
        menuButtonsEnabled = false;
        menuButtonsDisableCooldown = true;
        yield return new WaitForSeconds (delay);
        menuButtonsDisableCooldown = false;
        menuButtonsEnabled = true;
    }

    public void loadTutorialScene()
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadTutorialScene());
        async.allowSceneActivation = false;
        loadingManatee1.SetActive(true);
        loadingManatee2.SetActive(true);
        //        toggleCinemaCameraOnOff();
    }

    public void loadCockpitScene(bool toggleCinema)
    {
        loadingImage.SetActive(true);

        if (toggleCinema)
            toggleCinemaCameraOnOff();
//
//        if (!cinemaCamera.gameObject.activeInHierarchy)
//            cinemaCamera.gameObject.SetActive(true);

        StartCoroutine(LoadCockpitScene());
        async.allowSceneActivation = false;
        loadingManatee1.SetActive(true);
        loadingManatee2.SetActive(true);
        //        toggleCinemaCameraOnOff();
    }

    IEnumerator LoadCockpitScene() 
    {
//        Debug.Log("Starting to load next level..");
        loading = true;
        async = Application.LoadLevelAsync("CockpitScene");
        yield return async;
    }

    IEnumerator LoadTutorialScene() 
    {
//        Debug.Log("Starting to load next level..");
        loading = true;
        async = Application.LoadLevelAsync("Tutorial");
        yield return async;
    }


    IEnumerator getLoadingState(float delay)
    {
        if (async.progress < 0.9f)
        {
            loadingText.text = "Loading next level (" + Mathf.Round(async.progress * 100) + "%)";
        }
        else
        {
//            loadingText.text = "Done. Press F6";
//            loading = false;
//            loadingManatee1.SetActive(false);
//            loadingManatee2.SetActive(false);

            async.allowSceneActivation = true; // This changes the scene when it's loaded
        }

        yield return new WaitForSeconds(delay);
        loadingStateOnCooldown = false;
    }

}
