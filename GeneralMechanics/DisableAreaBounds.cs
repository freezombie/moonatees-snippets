using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAreaBounds : MonoBehaviour 
{
    public AreaBounds[] areaToModify;
    bool enabled = true;
    float buffer = 0f;

    void Update()
    {
        if(buffer>0)
            buffer -= Time.deltaTime;
        if (buffer <= 0)
            buffer = 0;
    }
    void OnTriggerEnter(Collider col)
    {
//        if (enabled && (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "BoxColliderTop" || col.name == "Camera (eye)") && buffer == 0)
            
        if (enabled && (col.name == "Camera (eye)") && buffer == 0)
        {
//            Debug.Log("Enter");
            enabled = false;
            buffer = 1f;
        }            
    }

    void OnTriggerExit(Collider col)
    {
        if (!enabled && (col.name == "Camera (eye)"))
            
//        if (!enabled && (col.name == "BoxColliderBottom" || col.name == "BoxColliderMid" || col.name == "BoxColliderTop"))
        {
//            Debug.Log("Exit");
            foreach (AreaBounds ab in areaToModify)
            {                
                ab.ToggleResetting();
            }           

            enabled = true;
        }
    }
}
