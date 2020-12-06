using UnityEngine;
using System.Collections;

public class SpeechProximitySensor : MonoBehaviour
{

    SphereCollider mySphereCollider;
    private SpeechBubble mySpeechBubble;

    public float proximityTimeBeforeShowing = 20f;
    public float time;
    public bool actionTriggered = false;

	void Start () 
    {
        mySphereCollider = GetComponent<SphereCollider>();
        mySpeechBubble = GetComponentInChildren<SpeechBubble>();
	}
	
	void Update ()
    {
        if (time >= proximityTimeBeforeShowing && !actionTriggered)
        {
            mySpeechBubble.setBeFullyTransparentOff();
            actionTriggered = true;
        } 	    
	}

    void OnTriggerStay (Collider collider)
    {
        if (time < proximityTimeBeforeShowing)
        {
            time += Time.deltaTime;
        }
    }
}
