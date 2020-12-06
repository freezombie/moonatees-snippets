using UnityEngine;
using System.Collections;
using NewtonVR;

public class HolderTrigger : MonoBehaviour 
{
    public CoffeeGrinder cg;
    public CoffeeMaker cm;
    bool happenedOnce = false;
    void OnTriggerEnter(Collider col)
    {            
        if(cg.GetCupReady() && cm.GetCanInsertCup() && col.transform.name == "Beancup" && !happenedOnce)
        {
            happenedOnce = true;

            col.GetComponent<NVRInteractableItem>().EndInteraction();
            if (col.GetComponent<NVRInteractableItem>().leftAttachedHand != null)
                Debug.Log("Da fuggen hell. Löyty vielä vasen käsi kiinni");
            if (col.GetComponent<NVRInteractableItem>().rightAttachedHand != null)
                Debug.Log("Da fuggen hell. Löyty vielä oikee käsi kiinni");


            col.GetComponent<NVRInteractableItem>().enabled = false;
            Destroy(col.GetComponent<Rigidbody>()); // this had to be done, even with kinematic the physics went nuts

            col.transform.SetParent(this.gameObject.transform.parent);
            col.transform.localPosition = new Vector3(-1.08f, -0.96f, 1.206f); // this was just found by messing in the editor, seems imprecise.
            col.transform.localRotation = new Quaternion(0,0,0,0); // First i thought original rotations would be best, I guess the models faced different ways in blender or something and that rot is something else here.                  
        
            cm.ChangeHasCoffeeGrind(true);
        }
    }
}
