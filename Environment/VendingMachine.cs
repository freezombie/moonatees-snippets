using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour 
{
    float buffer=0;
    public GameObject[] bottles;
    GameObject spawnPoint;
    GameObject curObject;

    void Start()
    {
        spawnPoint = transform.FindChild("SpawnPoint").gameObject;
    }

	void Update () 
    {
        if (buffer > 0)
        {
            buffer -= Time.deltaTime;
            if (buffer <= 0)
                buffer = 0;
        }
	}

    public void SpawnBottle(int index)
    {
        if (buffer == 0)
        {
            //Debug.Log("got here");
            curObject = Instantiate(bottles[index], spawnPoint.transform.position, Quaternion.identity,spawnPoint.transform);
            curObject.transform.rotation = new Quaternion(0, 0, 0, 0);           
            curObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.back*0.25f,ForceMode.Impulse);
            if(index==0 || index==1)
                curObject.GetComponentInChildren<Rigidbody>().AddTorque(Vector3.right * 0.005f, ForceMode.Impulse);
            else
                curObject.GetComponentInChildren<Rigidbody>().AddTorque(Vector3.right * 0.02f, ForceMode.Impulse);
            buffer = 2f;
            //Debug.Log("Curobject is " + curObject.name + " and it's rigidbody is at " + curObject.GetComponentInChildren<Rigidbody>().transform.name);
            //curObject.GetComponen<Rigidbody>().AddForce(Vector3.forward*3);
        }
    }
}
