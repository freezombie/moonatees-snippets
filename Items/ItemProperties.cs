using UnityEngine;
using System.Collections;
using System;

public class ItemProperties : MonoBehaviour 
{
    private Guid ID;
    private ItemManagerScript itemManager;
    public bool conductivity;
    private bool conductingstatus;
    public float conductingArea;
    public bool CanContainLiquid;
    public bool startWithRotatingForces = false;
    Rigidbody rb;

	void Start()
    {
        // start of id stuff
//        itemManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ItemManagerScript>();
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManagerScript>();

        if(itemManager.NameExists(this.gameObject.name))
        {
            ID = itemManager.GetID(this.gameObject.name);
            if (ID == Guid.Empty)
            {
                Debug.Log("ASSIGNED AN EMPTY GUID!");
            }
        }
        else
        {
            ID = Guid.NewGuid();
        }
        //end of id stuff
        //start of electricity stuff
        if(!conductivity)
        {
            conductingstatus = false;
            conductingArea = 0.0f;
        }
        if(conductivity)
        {
            GameObject electricArea = new GameObject("Electric Area");
			electricArea.transform.localScale = new Vector3(conductingArea, conductingArea, conductingArea); // change this into scale maybe, test sometime
            electricArea.transform.parent = this.gameObject.transform;
            electricArea.transform.localPosition = new Vector3(0, 0, 0);
            electricArea.AddComponent<SphereCollider>().isTrigger = enabled;
            electricArea.AddComponent<ElectricArea>();
        }
        if (startWithRotatingForces)
        {
            rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddTorque(0.01f, 0.01f, 0.01f, ForceMode.Impulse);
            }
        }
        //end of electricity stuff
    }
    public bool  checkCanContainLiquid(bool status)
    {
        if (CanContainLiquid)
        {
            status = true;
        }
        else
        {
            status = false;

        }

        return status;
    }
    public Guid GetID()
    {
        return ID;
    }

    public bool GetConductingStatus()
    {
        return conductingstatus;
    }

    public void ChangeConductingStatus(bool status)
    {
        conductingstatus = status;
//        Debug.Log("ChangeConductingStatus");
    
		// Jos tämä objekti jossa tämä skripti on kiinni on nimeltään WakingManatee, niin suorita herättäminen
		if (this.gameObject.name == "WakingManatee" && conductingstatus) 
		{
//            Debug.Log("Sending wake up message with electricity");
			this.gameObject.SendMessage("WakeUp");
		}	
	}

    public void ToggleBoolean()
    {
        //for shits and giggles and to get rid of an error. This might be needed aswell at some point
    }
}
