using UnityEngine;
using System;
using System.Collections;

public class AutomaticProximityDoor : MonoBehaviour
{
    private bool doorOpen = false;
    public bool doorLocked = false;
	public bool openOnUnlock = false;
	private Animator[] animators;
	StatusLamp[] lamps;
    AudioSource[] audioSources;
    public AudioClip[] doorClips;
    void Start()
    {
        animators = gameObject.GetComponentsInChildren<Animator>();
		lamps = GetComponentsInChildren<StatusLamp>();
        audioSources = GetComponentsInChildren<AudioSource>();
		if(lamps.Length != 0)
		{
			if(doorLocked)
			{
				foreach(StatusLamp lamp in lamps)
				{
					lamp.changeLightToRed();
				}
			}
			else
			{
				foreach(StatusLamp lamp in lamps)
				{
					lamp.changeLightToGreen();
				}
			}
		}
    }

    public  void OnTriggerEnter(Collider col)
    {
		if (col.tag == "MainCamera" && !doorOpen && !doorLocked)
            openDoor();
    }

    public  void OnTriggerExit(Collider col)
    {
        if (col.tag == "MainCamera" && doorOpen && !doorLocked)
            closeDoor();
    }
        
    public void openDoor()
    {
		foreach(Animator animator in animators)
		{
			animator.Play("DoorOpen");
		}
        audioSources[1].clip = doorClips[1];
        audioSources[1].Play();
        doorOpen = true;
    }

    public void closeDoor()
    {
		foreach(Animator animator in animators)
		{
			animator.Play("DoorClose");
		}
        audioSources[1].clip = doorClips[2];
        audioSources[1].Play();
        doorOpen = false;
    }

    public void unlockDoor()
	{
		doorLocked = false;
        audioSources[0].clip = doorClips[0];
        audioSources[0].Play();

		if (openOnUnlock)
			openDoor ();

		if(lamps.Length != 0)
		{
			foreach(StatusLamp lamp in lamps)
			{
				lamp.changeLightToGreen();
			}
		}
    }

	public void ToggleBoolean()
	{
        if(doorLocked)
		    unlockDoor();
	}
}