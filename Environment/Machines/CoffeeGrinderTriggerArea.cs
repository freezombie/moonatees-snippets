using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoffeeGrinderTriggerArea : MonoBehaviour 
{
    CoffeeGrinder cg;
    int i;
    bool canPutBack;

    void Start()
    {
        cg = GetComponentInParent<CoffeeGrinder>();
        i = 0;
    }

    void OnTriggerEnter (Collider col)
    {        
        if(col.transform.parent != null && col.transform.parent.name=="Beancup")
        {
            if(cg.GetCupStatus()==false && canPutBack) // if it's not attached we attach it again, if its already attached there probably is nothing we should be doing
            {                
                col.transform.parent.localPosition = cg.GetCupOrigPos();
                col.transform.parent.localRotation = cg.GetCupOrigRot();
                col.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
                col.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;
                col.GetComponentInParent<Rigidbody>().isKinematic = true;
                col.GetComponentInParent<Grabbable>().enabled = false;
                StartCoroutine(WaitAndEnable(cg.GetCupObject()));
                cg.ChangeCupStatus(true);
            }
        }        
    }

    void OnTriggerExit (Collider col)
    {
        if (col.transform.parent != null && col.transform.parent.name == "Beancup")
        {
            if (cg.GetCupStatus())
            {
                i++;
                if(i>=4)
                {
                    cg.ChangeCupStatus(false);
                    i = 0;
                    StartCoroutine(WaitAndSetTrue());
                }                
            }
        }        
    }

    IEnumerator WaitAndSetTrue()
    {
        canPutBack = false;
        yield return new WaitForSeconds(2);
        canPutBack = false;

//        canPutBack = true;
    }

    IEnumerator WaitAndEnable(GameObject obj)
    {
        yield return new WaitForSeconds(5);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Grabbable>().enabled = true;
    }
}
