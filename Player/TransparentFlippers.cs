using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentFlippers : MonoBehaviour
{
    private Renderer myRenderer;
    private float xOffset = 0f;

	// Use this for initialization
	void Start () 
    {
        myRenderer = this.GetComponent<Renderer>();	
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        xOffset += 0.01f;
        if (this.transform.gameObject.activeSelf == true)
            myRenderer.material.mainTextureOffset = new Vector2(xOffset, 0);
	}
}
