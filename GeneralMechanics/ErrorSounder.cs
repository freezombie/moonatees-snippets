using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSounder : MonoBehaviour {

    ButtonScript callingBS;
    AudioSource errorAS;
    public AudioClip[] errorClips;

	// Use this for initialization
	void Start () 
    {
        errorAS = GetComponent<AudioSource>();
        errorAS.clip = errorClips[Random.Range(0, errorClips.Length)];
        errorAS.Play();
        StartCoroutine(WaitAndDestroy((errorAS.clip.length + 0.2f)));  
	}
	
	// Update is called once per frame
	

    IEnumerator WaitAndDestroy(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        callingBS.SetErrorSpawned(false);
        Destroy(this);
    }

    public void SetButtonScript(ButtonScript button)
    {
        callingBS = button;
    }
}
