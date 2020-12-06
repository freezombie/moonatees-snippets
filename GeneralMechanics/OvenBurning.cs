using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenBurning : MonoBehaviour 
{
    private BoxCollider myBoxCollider;
    public Texture2D burntFishTexture;

	void Start ()
    {
        myBoxCollider = GetComponent<BoxCollider>();
	}
	
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "TheFish")
        {
            col.GetComponentInChildren<Renderer>().material.mainTexture = burntFishTexture;
            col.GetComponentInChildren<Animator>().Stop();
        }
    }
}
