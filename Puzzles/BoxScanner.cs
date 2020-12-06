using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScanner : MonoBehaviour 
{
    private GameObject myLazer;
    public AudioClip[] audioClips;
    AudioSource audioSource;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        myLazer = this.transform.FindChild("Lazer1").gameObject;
        myLazer.SetActive(false);
	}

    // Called from animation clip function
    void StartLazer()
    {
        myLazer.SetActive(true);
    }

    // Called from animation clip function
    void StopLazer()
    {
        myLazer.SetActive(false);
    }

    // Called from animation clip function
    void ScanningComplete()
    {
        //Debug.Log("Scan is complete. Box is truly solved");
    }

    public void PlayAudio(int index)
    {
        Debug.Log("This happened with index: " + index);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
