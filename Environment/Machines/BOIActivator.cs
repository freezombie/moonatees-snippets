using UnityEngine;
using System.Collections;

public class BOIActivator : MonoBehaviour 
{
	bool playerNear;
	BoardOfInstructions boi;

	void Start()
	{
		boi = transform.parent.GetComponent<BoardOfInstructions> ();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.F12))
		{
			boi.SetPlayerNear (true);
		}
		if(Input.GetKey(KeyCode.F11))
		{
			boi.SetPlayerNear (false);
		}
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "MainCamera")
		{
			boi.SetPlayerNear (true);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "MainCamera")
		{
			boi.SetPlayerNear (false);
		}
	}
}
