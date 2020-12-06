using UnityEngine;
using System.Collections;

public class ItemHoldingPosition : MonoBehaviour
{
	private GameObject rightHandPalm;
	private GameObject leftHandPalm;

	public Vector3 itemHoldingPosition;

	public float distanceBetweenHands;

	void Start ()
	{
		rightHandPalm = GameObject.FindGameObjectWithTag ("RightHandPalm");
		leftHandPalm = GameObject.FindGameObjectWithTag ("LeftHandPalm");
	}
	
	void Update ()
	{
		// Line between palms
		Debug.DrawLine (rightHandPalm.transform.position, leftHandPalm.transform.position, Color.green);

		itemHoldingPosition = (rightHandPalm.transform.position + leftHandPalm.transform.position) / 2f;

		// Distance between hands
		distanceBetweenHands = Vector3.Distance(rightHandPalm.transform.position, leftHandPalm.transform.position);
		

		// Ray up from item holding position
		Debug.DrawRay(itemHoldingPosition, Vector3.up * 0.1f, Color.red);
	}
}
