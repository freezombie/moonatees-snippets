using UnityEngine;
using System.Collections;

public class AlignToPlayerView : MonoBehaviour
{
	public GameObject player;

	void Start () 
	{
//		Debug.Log ("ASDASDASDd2d2d2d");
	}
	
	public void alignToPlayerView()
	{
//		Debug.Log ("Finding player within align thingy");

		player = GameObject.FindGameObjectWithTag ("Player");

		if (player != null)
		{
//			Debug.Log ("Align found a player");
			this.transform.position = player.transform.position;
			//		Vector3 lookingPosition = Camera.main.transform.forward * 2 - Camera.main.transform.position;
			//		Vector3 lookingPosition = player.transform.position - Camera.main.transform.forward * 2;

			//		lookingPosition.y = 0;
			//		this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
			this.transform.rotation = Quaternion.LookRotation (player.transform.forward);
			//		this.transform.rotation = Quaternion.LookRotation(lookingPosition);
		}
	}
}
