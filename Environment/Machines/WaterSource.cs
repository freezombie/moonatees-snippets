using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NewtonVR;

public class WaterSource : MonoBehaviour
{
	public Transform snapTransform;
	Vector3 snapPos;
	Quaternion snapRot;
	bool objIsFull;
	bool objInserted = false;
	GameObject obj;
    public GameObject boardOfInstructions;
	void Start ()
	{
		snapPos = new Vector3 (snapTransform.localPosition.x, snapTransform.localPosition.y, snapTransform.localPosition.z);
		snapRot = new Quaternion(snapTransform.localRotation.x, snapTransform.localRotation.y, snapTransform.localRotation.z, snapTransform.localRotation.w);
	}	

	void OnTriggerEnter(Collider col)
	{
		/*if (col.transform.FindChild("Water") != null && col.transform.FindChild("Water").gameObject.activeSelf == false && mugIsFull == false)
        {
            col.transform.SetParent(this.transform.parent.transform);
            col.transform.localPosition = snapPos;
            col.transform.localRotation = snapRot;
            col.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            col.GetComponent<Rigidbody>().isKinematic = true;
            col.GetComponent<Grabbable>().enabled = false;
            StartCoroutine(WaitAndEnable(col.gameObject));
			insertedMug = col.gameObject;
        }
        */
		if(col.transform.name.Contains("Buckit"))
		{
			snapRot = Quaternion.Euler(-90f,snapTransform.localEulerAngles.y,snapTransform.localEulerAngles.z);
		}
		if (col.transform.GetComponent<ItemProperties>()!= null && !objInserted) 
		{
			if (col.transform.GetComponent<ItemProperties>().CanContainLiquid) 
			{
				objInserted = true;


                col.GetComponent<NVRInteractableItem>().EndInteraction();


//                col.transform.position = snapPos;
//                col.transform.rotation = snapRot;

                if (col.GetComponent<Rigidbody>() != null)
                {
                    col.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    col.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    col.GetComponent<Rigidbody>().isKinematic = true;
                }
                else if (col.transform.parent != null && col.transform.parent.GetComponent<Rigidbody>() != null)
                {
                    col.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    col.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    col.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                }

                col.transform.SetParent(this.transform.parent.transform);
                col.transform.localPosition = snapPos;
                col.transform.localRotation = snapRot;


//                col.GetComponent<NVRInteractableItem> ().enabled = false;

				obj = col.gameObject;
			}
		}
	}


	public void ToggleBoolean()
	{;
		if (obj != null) 
		{
			if (!objIsFull) 
			{
				obj.GetComponent<FillWithliquid>().ToggleInterAction(this.transform.parent.gameObject, FillWithliquid.Liquid.Water);
				objIsFull = true;
                boardOfInstructions.GetComponent<BoardOfInstructions>().SetStateBoolean(true, 0);
                boardOfInstructions.GetComponent<BoardOfInstructions>().ChangeTexture(1);
                obj.GetComponent<Rigidbody> ().isKinematic = false;
				obj.GetComponent<NVRInteractableItem> ().enabled = true;
			}     
		}
	}

	void OnTriggerExit(Collider col)
	{
        
		if(col.gameObject == obj)
		{
			obj = null;
			objIsFull = false;
			objInserted=false;
			snapRot = new Quaternion(snapTransform.localRotation.x, snapTransform.localRotation.y, snapTransform.localRotation.z, snapTransform.localRotation.w);
		}
     
	}
}
