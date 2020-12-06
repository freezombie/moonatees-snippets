using UnityEngine;
using System.Collections;

public class CockpitDoorUnlocker : MonoBehaviour 
{
	SpeechBank sb; 
	AutomaticProximityDoor apd;
	bool doorOpened = false;

	void Start()
	{
		sb = GameObject.FindGameObjectWithTag("SpeechBank").GetComponent<SpeechBank>();
		apd = this.gameObject.GetComponent<AutomaticProximityDoor>(); 
	}
	void Update () 
	{
		if(!doorOpened)
		{
			//if(sb.getSpeechShownState(5))
			//{
			//	apd.unlockDoor();
			//	doorOpened = true;
			//}
		}
	}
}
