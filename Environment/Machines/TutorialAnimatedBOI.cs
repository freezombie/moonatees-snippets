using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimatedBOI : MonoBehaviour 
{
    public Texture animationSprite;
    Material screenMaterial;

    void Start()
    {
        screenMaterial = GetComponent<Renderer>().material;
        animationSprite = screenMaterial.mainTexture;
    }

    void Update()
    {
        if (screenMaterial.mainTexture != animationSprite)
        {
            screenMaterial.mainTexture = animationSprite;
        }
    }

    public void StartAnimation()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("Default");
    }
}
