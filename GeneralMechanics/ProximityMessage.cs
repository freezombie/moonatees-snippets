using UnityEngine;
using System.Collections;

public class ProximityMessage : MonoBehaviour
{
    public GameObject[] activatedObjects;

  public  void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
            foreach (GameObject gameObject in activatedObjects)
            {
                gameObject.SendMessage("ToggleBoolean");
            }
        }
    }



  public  void OnTriggerExit(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
            foreach (GameObject gameObject in activatedObjects)
            {
                gameObject.SendMessage("ToggleBoolean");
            }
        }
    }
}