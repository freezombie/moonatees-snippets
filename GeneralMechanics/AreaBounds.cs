using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaBounds : MonoBehaviour 
{
    GameObject player;
    public Transform resetToPoint;
    AudioSource audiosource;
    GameManager gm;
    Rigidbody playerRB;
    //public Image imageToFade;    
    //public float fadeSpeed;
    public AudioClip[] outOfBoundsClips;
    public bool resetting = true;
    public Fader fader;

    void Start()
    {
        audiosource = resetToPoint.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerRB = player.GetComponent<Rigidbody>();
    }

    void OnTriggerExit(Collider col)
    {
//        Debug.Log("col " + col.name + " exited");
        if (resetting && col.name == "Camera (eye)")
        {            
            //gm.setMovementControlState(false);
            fader.SetTeleporting(true, this);            
        }            
    }

    void Update()
    {
        /*if (fadeOut)
        {
            if (imageToFade.color.a < 1)
            {
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, imageToFade.color.a + fadeSpeed);
                if (imageToFade.color.a > 0.3f)
                {
                    if (!audiosource.isPlaying)
                    {
                        audiosource.clip = outOfBoundsClip;
                        audiosource.Play();
                    }
                }
                return;
            }
            else
            {
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 1);
                fadeOut = false;
                fadeIn = true;
            }
        }
        if (fadeIn)
        {
            if (imageToFade.color.a > 0)
            {
                player.transform.position = resetToPoint.position;
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, imageToFade.color.a - fadeSpeed);
                return;
            }
            else
            {
                fadeIn = false;
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0);
                playerRB.velocity = Vector3.zero;
                gm.setMovementControlState(true);
            }
        }*/
    }

//    public void SetResetting(bool boolean)
//    {
//        resetting = boolean;
//    }
//
//    public bool GetResetting()
//    {
//        return resetting;
//    }

    public void ToggleResetting()
    {
        if (resetting)
            resetting = false;
        else
            resetting = true;
//        Debug.Log("Resetting now: " + resetting);
    }    

    public void Teleport()
    {
        player.transform.position = resetToPoint.position;
        if (!audiosource.isPlaying)
        {
            audiosource.clip = outOfBoundsClips[Random.Range(0,outOfBoundsClips.Length)];
            audiosource.Play();
        }
        playerRB.velocity = Vector3.zero;
        //gm.setMovementControlState(true);
    }
}
