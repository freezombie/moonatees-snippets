using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketHandle : MonoBehaviour
{

//	public Rigidbody myRigidbody;
	private HingeJoint myHingleJoint;
	private Vector3 myOriginalTransform;

	// Use this for initialization
	void Start () 
	{
		myHingleJoint = this.GetComponent<HingeJoint> ();
		myOriginalTransform = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (myHingleJoint.connectedBody == null) {
			myHingleJoint.connectedBody = this.transform.parent.parent.GetComponent<Rigidbody> ();
			this.transform.localPosition = myOriginalTransform;
		}

		if (myHingleJoint.connectedBody == null) {
			myHingleJoint.connectedBody = this.transform.parent.GetComponent<Rigidbody> ();
			this.transform.localPosition = myOriginalTransform;
		}
	}
}
