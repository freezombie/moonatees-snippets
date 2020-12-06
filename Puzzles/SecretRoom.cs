using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SecretRoom : MonoBehaviour
{
    private GameObject player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    public  void OnTriggerEnter(Collider col)
    {
        if (col.name == "Camera (eye)")
        {
//            player.GetComponentInChildren<BloomAndFlares>().enabled = true;
        }
    }

    public  void OnTriggerExit(Collider col)
    {
        if (col.name == "Camera (eye)")
        {
//            player.GetComponentInChildren<BloomAndFlares>().enabled = false;
        }
    }
}
