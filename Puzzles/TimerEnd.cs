using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEnd : MonoBehaviour 
{
    TimerStart ts;

    void Start()
    {
        ts = transform.parent.FindChild("TimerStart").GetComponent<TimerStart>();
    }

    void OnTriggerExit(Collider col)
    {
        if (col.name == "Camera (eye)")
        {
            ts.SetTimerOff();
        }
    }
}
