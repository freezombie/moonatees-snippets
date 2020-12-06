using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour 
{
    public GameObject[] activatedObjects;
	private float yRotation;
	public bool status;
    public float localRotationXSwitchPoint;
	public enum Type {ActivateOnlyForDown = 0, ActivateOnUpandDown = 1}
	public Type type = 0;
	float originalXRot;

	void Start()
	{
		originalXRot = transform.localRotation.x;
	}

	void Update () 
	{
        //Debug.Log("localrotation.x " + transform.localRotation.x);
		if(type == Type.ActivateOnlyForDown)
		{
			if(originalXRot > localRotationXSwitchPoint)
			{
				if(localRotationXSwitchPoint > transform.localRotation.x)
				{
					foreach (GameObject go in activatedObjects)
					{
						go.SendMessage("ToggleBoolean");
					}
				}
			}
			else
			{
				if(localRotationXSwitchPoint < transform.localRotation.x)
				{
					foreach (GameObject go in activatedObjects)
					{						
						go.SendMessage("ToggleBoolean");
					}
				}
			}
		}
		if(type == Type.ActivateOnUpandDown)
		{
			if (status==false)
			{
				if (transform.localRotation.x > localRotationXSwitchPoint)
				{
					foreach (GameObject gameObject in activatedObjects)
					{
						gameObject.SendMessage("ToggleBoolean");
					}
					status = true;
				}
			}
			else if (status==true)
			{
				if (transform.localRotation.x < localRotationXSwitchPoint)
				{
					foreach(GameObject gameObject in activatedObjects)
					{
						gameObject.SendMessage("ToggleBoolean");
					}
					status = false;
				}
			}
		}        
	}
}
