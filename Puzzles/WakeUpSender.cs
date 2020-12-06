using UnityEngine;
using System.Collections;

public class WakeUpSender : MonoBehaviour {


	public GameObject[] WakingComponent;



	public void OnCollisionEnter(Collision col)
	{
		for(int i=0; i <WakingComponent.Length; i++)
		{
			if (col.gameObject == WakingComponent[i])
			{
				gameObject.SendMessage("WakeUp");
//				Debug.Log("WakeUp");
//				Debug.Log(col.gameObject);
			}
		}        
	}



}
