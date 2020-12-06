using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStart : MonoBehaviour 
{
    float timer;
    bool timerOn = false;
    bool playedMoveOn = false;
    AudioSource playerAS;
    public AudioClip[] audioClips;

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Camera (eye)")
        {
            timerOn = true;
            playerAS = col.transform.GetComponent<AudioSource>();
            playerAS.volume = 1f;
        }
    }

    void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
        }
        if (timer > 30f && !playedMoveOn)
        {
            playerAS.loop = false;
            playerAS.clip = audioClips[0]; // play get a move on.
            playerAS.Play();
            playedMoveOn = true;
        }
    }

    public void SetTimerOff()
    {
        if (timer < 15)
            playerAS.clip = audioClips[1]; // play seahorse
        else if (timer < 25)
            playerAS.clip = audioClips[2]; // play wonderful
        else
            playerAS.clip = audioClips[3]; // play turtle
        playerAS.loop = false;
        playerAS.Play();
        timerOn = false;
        timer = 0f;
    }
}
