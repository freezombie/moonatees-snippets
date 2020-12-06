using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemTeleport : MonoBehaviour {

    public GameObject StartPoint;
    public GameObject gameobject;
   
    public GameObject  EndPoint;
  
    public bool status = false;
    public Collider col;
    private bool teleportedItemAtEndPoint = false;

    //maybe start ja get gameobject by name
    void Start()
    {
       gameobject.transform.position = StartPoint.transform.position;

    }

    public void ToggleBoolean()
    {
            evenHitCount += 1;
         if(evenHitCount == 1)
        {
            evenHitCount += 1;

        }
        
        if (evenHitCount == 1)
        {
            teleportedItemAtEndPoint = true;
            gameobject.transform.position = EndPoint.transform.position;
        }
        if (evenHitCount % 2 == 0)
        {
            teleportedItemAtEndPoint = false;
            gameobject.transform.position = EndPoint.transform.position;
        }
        if (evenHitCount % 2 == 1)
        {
            teleportedItemAtEndPoint = true;
            gameobject.transform.position = StartPoint.transform.position;

        }

       
        Debug.Log(EndPoint.transform.ToString() + "teleported succes"  +EndPoint.transform.ToString());
    }

    public int evenHitCount = 0;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
           
               
            
            teleportedItemAtEndPoint = false;
            OnHit();
           
        }
   
            
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
        
            status = false;

        }
    }

    void OnHit()
    {
        status = false;
        if (status == false)
        {
            teleportedItemAtEndPoint = true;

        }
     
        
            if (teleportedItemAtEndPoint)
            {
                status = true;
                
            }


        
        if (status == true)
        {
            gameObject.SendMessage("ToggleBoolean");

        }



    }











}






