using UnityEngine;
using System.Collections;
using System.Reflection;

public class BoardOfInstructions : MonoBehaviour 
{
	public GameObject hatch;
    public AudioClip[] hatchAudioClips;
    AudioSource hatchAS;
    AudioSource boiAS;
	public Vector3 target;
	public float marginWaitTime;
	Vector3 originalPos;
	public float movementSpeed;
	public float idleWaitTime;
	float waitTimeHelper;
	bool playerNear;
	bool active;
	bool moving = false;
	bool doneMoving = false;
    bool instructionsDone = false;
	bool playingAnim = false;
    bool deactivating = false;
    public bool startActivated = false;
	Animator hatchAnimator;
	Animator boiAnimator;
	public AnimationClip openHatch;
	public AnimationClip activateBOI;
	public AnimationClip idle1;
	public AnimationClip idle2;
	public AnimationClip wave;
	RuntimeAnimatorController ac;
	GameObject screen;
	public Texture[] screenStates;
    bool[] stateBooleans;
	int screenState=-1; // we start the screen being off basically.
    public enum Board {Default,OnePicture,Animated}
    public Board boardType;
    public AudioClip changeTextureAudioClip;
	// Use this for initialization
	void Start () 
	{       
		boiAnimator = GetComponent<Animator> ();
		ac = boiAnimator.runtimeAnimatorController;
		originalPos = transform.localPosition;
		//boiAnimator.Play("Deactivate/Activate",0f,ac.animationClips[GetClipIndex("Deactivate/Activate")].length);
        if (hatch != null)
        {
            hatchAnimator = hatch.GetComponent<Animator> ();
            hatchAS = hatch.GetComponent<AudioSource>();
        }
        if (!startActivated)
        {
            boiAnimator.Play("Deactivate/Activate",0,activateBOI.length);
            boiAnimator.SetFloat ("Speed", 0f);
        }
        else
        {
            active = true;
        }		
		screen = transform.FindChild("Screen").gameObject;
		waitTimeHelper = idleWaitTime;
        stateBooleans = new bool[screenStates.Length];
        for(int i=0;i<stateBooleans.Length;i++)
        {
            stateBooleans[i] = false;
        }
        if (startActivated)
        {
            screenState = 0; // we don't start as 'off'
        }
        boiAS = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () 
	{
        /*if(Input.GetKeyDown(KeyCode.F7))
        {
            Deactivate();
        }
        if(Input.GetKeyDown(KeyCode.F8))
        {
            NextTextureStage();
        }*/
        if(instructionsDone && !doneMoving)
        {
            if (!startActivated)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPos, movementSpeed * Time.deltaTime);
            }
            // tähän jos tarvii toinen deaktivointi tapa johon tarvii määrittää positio
            if (transform.localPosition.y == originalPos.y)
            {
                if (hatch != null)
                {
                    hatchAnimator.Play("Open",0,openHatch.length);
                    hatchAS.clip = hatchAudioClips[1];
                    hatchAS.Play();
                    hatchAnimator.SetFloat("Speed", -1);
                }
                doneMoving = true;
            }
        }

        if (moving)
		{
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, target, movementSpeed * Time.deltaTime);
			if (transform.localPosition.y == target.y)
			{
				doneMoving = true;
				moving = false;
				playingAnim = true;
				boiAnimator.Play("Deactivate/Activate",0,activateBOI.length);
				StartCoroutine(WaitAndExecute("SetPlayingAnimFalse", activateBOI.length));
				boiAnimator.SetFloat ("Speed", -1);
                if (boardType == Board.Default)
                {
                    StartCoroutine(WaitAndExecute ("NextTextureStage", activateBOI.length));
                }
                if (boardType == Board.Animated)
                {
                    StartCoroutine(WaitAndExecute ("StartScreenAnimation", activateBOI.length));
                }
			}
		}
		if (!playingAnim && screenState != -1)
		{
			waitTimeHelper -= Time.deltaTime;
			if(waitTimeHelper<=0)
			{
				if (playerNear)
				{
					playingAnim = true;
					boiAnimator.Play ("Idle1");
					StartCoroutine (WaitAndExecute ("SetPlayingAnimFalse", idle1.length));
					waitTimeHelper = idleWaitTime;
				}
				else
				{
					playingAnim = true;
					boiAnimator.Play ("Wave");
					StartCoroutine (WaitAndExecute ("SetPlayingAnimFalse", wave.length));
					waitTimeHelper = idleWaitTime;
				}
			}
		}
	}

	public int GetClipIndex(string name)
	{
		for (int i = 0; i < ac.animationClips.Length; i++)
		{
			if(ac.animationClips[i].name == name)
			{
				return i;
			}
		}
		return 666;
	}

	public void SetPlayerNear(bool boolean)
	{
		if (!active && boolean && !instructionsDone)
		{
			active = true;
			playerNear = true;
            if(!startActivated)
			    StartCoroutine (Activate ());
		}
		playerNear = boolean;
	}

	IEnumerator Activate()
	{
        if (hatch != null)
        {
            hatchAnimator.Play ("Open");
            hatchAS.clip = hatchAudioClips[0];
            hatchAS.Play();
            yield return new WaitForSeconds(openHatch.length);
        }
		moving = true;
	}

	IEnumerator WaitAndExecute (string methodName, float timetowait) // used to wait and then execute another method. Could be improved with actually getting and settign parameters. Using System.reflections.
	{
		//Get the method information using the method info class
		MethodInfo mi = this.GetType().GetMethod(methodName);

		yield return new WaitForSeconds (timetowait + marginWaitTime);
//        Debug.Log("Waited for " + (timetowait + marginWaitTime));
        //Invoke the method
        // (null- no parameter for the method call
        // or you can pass the array of parameters...)
		mi.Invoke(this, null);
	}

	public void SetPlayingAnimFalse ()
	{
		playingAnim = false;
	}

	public void ChangeTexture(Texture newTexture) // if we want to change the texture into something that has nothing to do with the stages of the screen (not really used right now)
	{
		playingAnim = true;
		boiAnimator.Play ("Idle2");
        boiAS.clip = changeTextureAudioClip;
        boiAS.Play();
		StartCoroutine(WaitAndExecute("SetPlayingAnimFalse",idle2.length));
		screen.GetComponent<Renderer> ().material.mainTexture = newTexture;
	}

    public void StartScreenAnimation()
    {
        screen.GetComponent<TutorialAnimatedBOI>().StartAnimation();
    }

	public void ChangeTexture(int stage) // if we wanna jump back and forth between said stages
	{
//        Debug.Log("Current state: " + screenState + " changing to " + stage);
//        Debug.Log("Current state boolean is " + stateBooleans[screenState]);
        if (stateBooleans[screenState]) // if the current screen instruction has been done, we can show the next one.
        {
//            Debug.Log("So we changed the state to the next one");
            screenState = stage;
            for(int i=screenState;i<screenStates.Length;i++) // this should find the first one where the stateboolean is false and do that.
            {                
                if(!stateBooleans[i])
                {
//                    Debug.Log("Found a false one at index: " + i);
                    ChangeTexture(screenStates[i]);
                    screenState = i;
                    break;
                }
                else if(i==screenStates.Length-1 && stateBooleans[i]) // if we checked the last oen and that was true then we skip to deactivating the BOI
                {
                    Deactivate();
                    break;
                }
            }
            
        }
        
	}

	public void NextTextureStage()
	{
		screenState++;
		ChangeTexture(screenStates [screenState]);
	}

    public void SetStateBoolean(bool boolean,int index)
    {
        stateBooleans[index] = boolean;
    }
    public void SetInstructionsDone()
    {
//        Debug.Log("instructions done");
        instructionsDone = true;
        doneMoving = false;
        deactivating = false;
    }

	public void Deactivate()
	{
        if(!deactivating)
        {
            deactivating = true;
//            Debug.Log("Trying to deactivate");
            if (playingAnim)
            {
                AnimatorStateInfo stateInfo = boiAnimator.GetCurrentAnimatorStateInfo(0);
//                Debug.Log("Gotta wait for " + (stateInfo.length - stateInfo.normalizedTime) + " for a " + stateInfo.length + " long animation");
                StartCoroutine(WaitAndExecute("Deactivate", stateInfo.length - stateInfo.normalizedTime + marginWaitTime));
            }
            else
            {
//                Debug.Log("Deactivating");
                playingAnim = true;
                boiAnimator.Play("Deactivate/Activate");
                boiAnimator.SetFloat("Speed", 1);
                screen.GetComponent<Renderer>().material.mainTexture = null;
                StartCoroutine(WaitAndExecute("SetInstructionsDone", activateBOI.length));
            }
        }        
	}
}
