using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class JukeboxTriggerArea : MonoBehaviour 
{
    Jukebox jb;
    bool disabled=false;
        
    void Start()
    {
        jb = GetComponentInParent<Jukebox>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (!disabled && col.GetComponent("CD") != null)
        {
            col.GetComponent<NVRInteractableItem>().EndInteraction();
            jb.SetCD(col.gameObject);
        }
    }
        
    public void SetDisabled(bool boolean)
    {
        disabled = boolean;
    }
}
