using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
    public GameObject playerPrefab;
	public GameObject gameControllerPrefab;

	void Awake () 
    {
	    if (GameObject.FindGameObjectWithTag("Player")==null)
        {
			Instantiate(playerPrefab,transform.position,transform.rotation);           
        }
		if (GameObject.FindGameObjectWithTag("GameController")==null)
		{
			Instantiate(gameControllerPrefab,transform.position,transform.rotation);
		}
	}
}
