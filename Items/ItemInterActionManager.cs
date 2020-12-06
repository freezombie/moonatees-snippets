using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemInterActionManager : MonoBehaviour
{
    

    public GameObject[] interactionObject;



   

    public void OnCollisionEnter(Collision col)
    {
        for(int i=0; i <interactionObject.Length; i++)
        {
            if (col.gameObject == interactionObject[i])
            {
				col.gameObject.SendMessage("ToggleInterAction", gameObject);
                Debug.Log("ToggleInterAction");
            }
        }        
    }

    public void OnTriggerEnter(Collider col)
    {
        for (int i = 0; i < interactionObject.Length; i++)
        {
            if (col.gameObject == interactionObject[i])
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SendMessage("ToggleInterAction", gameObject);
                Debug.Log("ToggleInterAction");
            }                
        }
    }  
}


