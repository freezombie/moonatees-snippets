using UnityEngine;
using System.Collections;

public class CoffeeWakeUp : MonoBehaviour 
{
    public GameObject fish;
    bool fishEntered = false;
    bool coffeeEntered = false;
    WakingScript ws;
    public GameObject wakeupBar;
    Animator barAnimator;
    AnimatorStateInfo stateInfo;
	bool currentlyReducing = false;
	int currentStage = 0;
	float currentAnimClipTime;
	float exitTime;
    AudioSource barAudioSource;

    void Start()
    {        
        ws = transform.parent.GetComponent<WakingScript>();
        barAnimator = wakeupBar.GetComponentInChildren<Animator>();
        barAudioSource = wakeupBar.GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider col)
    {
		if(col.transform.FindChild("Coffee")!=null && col.transform.FindChild("Coffee").gameObject.activeSelf && !coffeeEntered)			
        {
            coffeeEntered = true;
			if (currentlyReducing)
			{
				ResetBar ();
			}
            StartCoroutine(SendWakeUp(0));	
        }
        if (col.gameObject == fish && !fishEntered)
        {
            fishEntered = true;
			if (currentlyReducing)
			{
				ResetBar ();
			}
            StartCoroutine(SendWakeUp(1));
        }

        if (col.gameObject != fish && col.transform.FindChild("Coffee") == null)
        {
            StartCoroutine(SendWakeUp(3));
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.FindChild("Coffee") != null && col.transform.FindChild("Coffee").gameObject.activeSelf && coffeeEntered)
        {
            coffeeEntered = false;
			if (!ws.wakeUpDone)
			{
				ws.StartSnoring ();
				stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
				exitTime = stateInfo.normalizedTime;
				StartCoroutine (DecreaseBar (exitTime));
			}			    
        }
        if (col.gameObject == fish && fishEntered)
        {
            fishEntered = false;
			if (!ws.wakeUpDone)
			{
				ws.StartSnoring ();
				stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
				exitTime = stateInfo.normalizedTime;
				StartCoroutine (DecreaseBar (exitTime));
			}			    
        }
    }
    float GetBiggestFloat ( float a, float b)
    {
        if (a > b)
        {
            return a;
		}
        else
        {
            return b;
        }
    }
    IEnumerator SendWakeUp(int type) // 0 = coffee, 1 = fish
    {
        float audioClipLength = 0f;
        float animationClipLength = 0f;
		if (type == 0 || type == 1)
        {
            if(!wakeupBar.activeSelf)
            {
                wakeupBar.SetActive(true);
            }
            barAnimator.Play("Bar|0to4");
            barAudioSource.pitch = 0.7f;
            barAudioSource.Play();
			barAnimator.SetFloat ("Speed1", 1f);
			currentStage = 1;
            stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
            animationClipLength = stateInfo.length;
            if (type == 0)
            {
                ws.WakeUp(WakingScript.WakeType.Coffee, 0);
                audioClipLength = ws.coffeeWakeUpClips[0].length;
            }
            else if (type == 1)
            {
                ws.WakeUp(WakingScript.WakeType.Fish, 0);
                audioClipLength = ws.fishWakeUpClips[0].length;
            }
        }
        
        else if (type == 3)
        {
            ws.WakeUp(WakingScript.WakeType.WrongItem, 0);
            audioClipLength = ws.wrongItemClips[0].length;
//            yield return new WaitForSeconds(ws.delayTime);
            yield break;

        }

        yield return new WaitForSeconds(ws.delayTime + GetBiggestFloat(audioClipLength,animationClipLength));

        if((type == 0 && coffeeEntered) || (type == 1 && fishEntered))
        {
			//barAnimator.SetFloat ("Speed", 1f);
            barAnimator.Play("Bar|1to2");
            barAudioSource.pitch = 0.8f;
            barAudioSource.Play();
			barAnimator.SetFloat ("Speed2", 1f);
			currentStage = 2;
            stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
            animationClipLength = stateInfo.length;
            if (type == 0 && coffeeEntered)
            {
                ws.WakeUp(WakingScript.WakeType.Coffee, 1);
                audioClipLength = ws.coffeeWakeUpClips[1].length;
            }
            else if (type == 1 && fishEntered)
            {
                ws.WakeUp(WakingScript.WakeType.Fish, 1);
                audioClipLength = ws.fishWakeUpClips[1].length;
            }
        }        
        else
        {
            yield break;
        }

        yield return new WaitForSeconds(ws.delayTime + GetBiggestFloat(audioClipLength, animationClipLength));
        
        if((type == 0 && coffeeEntered) || (type == 1 && fishEntered))
        {
            barAnimator.Play("Bar|2to3");
            barAudioSource.pitch = 0.9f;
            barAudioSource.Play();
			barAnimator.SetFloat ("Speed3", 1f);

			currentStage = 3;
            stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
            animationClipLength = stateInfo.length;
            if (type == 0 && coffeeEntered)
            {
                ws.WakeUp(WakingScript.WakeType.Coffee, 2);
                audioClipLength = ws.coffeeWakeUpClips[2].length;
            }
            else if (type == 1 && fishEntered)
            {
                ws.WakeUp(WakingScript.WakeType.Fish, 2);
                audioClipLength = ws.fishWakeUpClips[2].length;
            }
        }        
        else
        {
            yield break;
        }

        yield return new WaitForSeconds(ws.delayTime + GetBiggestFloat(audioClipLength, animationClipLength));

        if ((type == 0 && coffeeEntered) || (type == 1 && fishEntered))
        {
//			barAnimator.SetFloat ("Speed", 1f);
            barAnimator.Play("Bar|3to4");
            barAudioSource.pitch = 1f;
            barAudioSource.Play();
            barAnimator.SetFloat("Speed4", 1f);
            currentStage = 4;
        }
        yield return new WaitForSeconds(ws.delayTime + animationClipLength);

        if (type == 0 && coffeeEntered)
        {
            ws.WakeUp(WakingScript.WakeType.Coffee, 3);
        }
        else if (type == 1 && fishEntered)
        {
            ws.WakeUp(WakingScript.WakeType.Fish, 3);
        }                
        else
        {
            yield break;
        }
    }

	IEnumerator DecreaseBar(float timeforstart)
	{
		stateInfo = barAnimator.GetCurrentAnimatorStateInfo(0);
		float animationClipLength = 0f;
		currentlyReducing = true;
		if (!coffeeEntered && !fishEntered)
		{
			if (currentStage == 3)
			{
				barAnimator.Play ("Bar|2to3", 0, timeforstart);
                barAudioSource.pitch = 0.5f;
                barAudioSource.Play();
				barAnimator.SetFloat ("Speed3", -1f);
				animationClipLength = stateInfo.length;
			}
			else if (currentStage == 2)
			{
				barAnimator.Play ("Bar|1to2", 0, timeforstart);
                barAudioSource.pitch = 0.5f;
                barAudioSource.Play();
				barAnimator.SetFloat ("Speed2", -1);
				animationClipLength = stateInfo.length;
			}
			else if (currentStage == 1)
			{
				barAnimator.Play("Bar|0to4",0,timeforstart);
                barAudioSource.pitch = 0.5f;
                barAudioSource.Play();
				barAnimator.SetFloat ("Speed1", -1);
				animationClipLength = stateInfo.length;
			}
			else if (currentStage == 0)
			{
				ResetBar ();
				wakeupBar.SetActive(false);
				currentlyReducing = false;
			}
		}	
		yield return new WaitForSeconds (animationClipLength);
		if(currentStage != 0)
		{
			currentStage--;
			StartCoroutine (DecreaseBar (animationClipLength-1f));
		}			
	}

	void ResetBar()
	{
		barAnimator.Play ("Bar|0to4");
		barAnimator.SetFloat ("Speed1", 1f);
		barAnimator.Stop();
		wakeupBar.SetActive (false);
		StopAllCoroutines ();
	}
}
