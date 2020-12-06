using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WhackAMoonatee : MonoBehaviour 
{
    public bool machineOn = false; // whether or not the whack a moonatees is running or not;
    GameObject[] miniMoonatees = new GameObject[4]; // change these by hand
    bool[] poppedUp = new bool[4]; // change these by hand
    int randomMoonateeIndex; // which moonatee we are going to pop up
    public float stayTime; // how long the moonatee stays popped up. Modified once in a while
    float originalStayTime;
    public float intervalTime; // how long between each pop up ( should not be dependant on whether something is up or not); Modified once in a while.
    float originalIntervalTime;
    public float miniMoonateeSpeed;
    float intervalTimeCounter; // this is what we modify and countdown in code.
    int score = 0; // how many times has the player hit
    float riseTargetY; // localposition.y of where we are going to rise.
    public int popupUpperLimit; //How many times a moonatee pops up.
    public int popupCounter = 0;
    int currentlyPoppedUpCounter = 0;
    public Fader fader;
    public GameObject sign;
    public Texture signRawrTexture;
    Texture signOriginalTexture;
    bool canLookForNew=false;
    bool foundSomething = false;
    public AnimationClip tRexTiltAnimation;
    bool playedSound = false;

    public AudioClip[] godVoiceClips;
    AudioSource audioSource;

    public Text scoreText;

    void Start () 
    {        
        audioSource = GetComponent<AudioSource>();
        intervalTimeCounter = intervalTime;
        originalStayTime = stayTime;
        originalIntervalTime = intervalTime;
        int helper = 0;
        for (int i = 0; this.transform.childCount > i; i++)
        {            
            if (transform.GetChild(i).name.Contains("Minimana") || transform.GetChild(i).name == "TRex")
            {
                miniMoonatees[helper] = transform.GetChild(i).gameObject;                
                /*catch (Exception e)
                {
                    Debug.Log(e + " when adding " + transform.GetChild(i).name + " to index: " + helper);
                }*/
                //Debug.Log("Added " + transform.GetChild(i).name + " to " + helper);
                helper++;
            }
        }
        for (int i = 0; i < poppedUp.Length; i++)
        {
            poppedUp[i] = false;
        }
        /*for (int i = 0; i < miniMoonatees.Length; i++)
        {
            Debug.Log("Found: " + miniMoonatees[i].name + " at index " + i);
        }*/
        /*miniMoonatees[0].GetComponent<WhackAMoonateeMiniMoonatee>().PopUp(miniMoonateeSpeed,0,originalStayTime);
        poppedUp[0] = true;
        miniMoonatees[1].GetComponent<WhackAMoonateeMiniMoonatee>().PopUp(miniMoonateeSpeed,1,originalStayTime);
        poppedUp[1] = true;
        miniMoonatees[2].GetComponent<WhackAMoonateeMiniMoonatee>().PopUp(miniMoonateeSpeed,2,originalStayTime);
        poppedUp[2] = true;
        miniMoonatees[3].GetComponent<WhackAMoonateeMiniMoonatee>().PopUp(miniMoonateeSpeed,3,originalStayTime);
        poppedUp[3] = true;*/
        //machineOn = true;
	}

	void Update () 
    {
        if (machineOn)
        {
            intervalTimeCounter -= Time.deltaTime;
            if (intervalTimeCounter <= 0 && popupCounter < popupUpperLimit)
            {
                int random = UnityEngine.Random.Range(0, miniMoonatees.Length);
                canLookForNew = false;
                for (int i = 0; i < poppedUp.Length; i++)
                {
                    if (!poppedUp[i])
                    {
                        canLookForNew = true;
                    }  
                }
                if (poppedUp[random] && canLookForNew)
                {
                    do
                    {
                        random = UnityEngine.Random.Range(0, miniMoonatees.Length);
                    } while(poppedUp[random] && currentlyPoppedUpCounter < 8); // testaa jossain välissä oisko pelkkä do while riittäny. sit ku koodi muuten toimii.
                }
                else if(!canLookForNew)
                {
                    return;
                }
                miniMoonatees[random].GetComponent<WhackAMoonateeMiniMoonatee>().PopUp(miniMoonateeSpeed,random,stayTime);
                poppedUp[random] = true;
//                Debug.Log("popped up " + random);
                    currentlyPoppedUpCounter++;
                    popupCounter++;
                    //float intervalTime2 = intervalTime; 
                    intervalTime -= intervalTime*intervalTime / (popupUpperLimit * 2);
                    /*for (int i = 0; i < popupUpperLimit; i++)
                {
                    intervalTime2 -= intervalTime2*intervalTime2 / (popupUpperLimit * 2);
                }
                Debug.Log("deducted interval time by : " + intervalTime * intervalTime / (popupUpperLimit * 2) + "In the end it will be: " + intervalTime2);*/
                    intervalTimeCounter = intervalTime;
                    //float stayTime2 = stayTime;
                    stayTime -= stayTime*stayTime / (popupUpperLimit * 2);
                    /*for (int i = 0; i < popupUpperLimit; i++)
                {
                    stayTime2 -= stayTime2*stayTime2 / (popupUpperLimit * 2);
                }
                Debug.Log("deducted staytime by : " + stayTime*stayTime / (popupUpperLimit * 2)+ "In the end it will be: " + stayTime2 );*/
                //}
            }
            if (popupCounter == popupUpperLimit)
            {
                StartCoroutine(EndWAM(stayTime+3f));
            }
        }
	}

    IEnumerator EndWAM (float timeToWait)
    {
        machineOn = false;
        if (score < 5)
        {
            audioSource.clip = godVoiceClips[0]; // play it's alright you still pass
        }
        else if (score < 12)
        {
            audioSource.clip = godVoiceClips[1]; // play lightning fast reflexes
        }
        else
        {
            audioSource.clip = godVoiceClips[2]; // play your tale will be told to everyone
        }
        audioSource.loop = false;
        if (!playedSound)
        {    
            audioSource.Play();
            playedSound = true;
        }


        yield return new WaitForSeconds(timeToWait);

        //                Debug.Log("You scored " + score);
        intervalTimeCounter = intervalTime;
        originalStayTime = stayTime;
        originalIntervalTime = intervalTime;
        PlayerPrefs.SetInt("TutorialPassed", 1);
        fader.Fade(10f, "LoadCockpit");

        //reset stuff here if needed
    }

    public void HitOn(int index)
    {
        score++;
        scoreText.text = score + "/20";
        poppedUp[index] = false;// tähän et johonkin osuttiin, ja merkitään et sitä voi tästä lähtien popupata.
        currentlyPoppedUpCounter--;
        //Debug.Log("Hit on " + miniMoonatees[index].name);
    }

    public void MissOn(int index)
    {
//        Debug.Log("Miss on " + index);
        poppedUp[index] = false;
        currentlyPoppedUpCounter--;
        //Debug.Log("Miss on " + miniMoonatees[index].name);
        // tähän et johonkin ei osuttu, ja merkitään et sitä voi tästä lähtien popupata.
    }

    public void TurnOn()
    {
        machineOn = true;
    }

    public void TurnOff()
    {
        machineOn = false;
        intervalTimeCounter = intervalTime;
        originalStayTime = stayTime;
        originalIntervalTime = intervalTime;
    }

    public AnimationClip GetTRexTiltAnimation()
    {
        return tRexTiltAnimation;
    }

    void OnTriggerEnter(Collider col)
    {
        if (enabled && (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "5BoxColliderTop"))
        {
            machineOn = true;       
        }            
    }
}
