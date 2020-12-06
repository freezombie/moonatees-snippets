using UnityEngine;
using System.Collections;

public class ItemTeleportTrigger : MonoBehaviour {

    public GameObject[] activatedObjects;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
            foreach (GameObject gameObject in activatedObjects)
            {
                gameObject.SendMessage("ToggleBoolean");
            }
        }
    }



    void OnTriggerExit(Collider col)
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
