using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindBlower : MonoBehaviour 
{
    public Transform windStartPoint;
    public Transform windEndPoint;
    public float windBoxX;
    public float windBoxY;
    public float force;
	//public Collider[] windAreaColliders;
	public GameObject visualWind;
    private Quaternion rotation;
    private Collider[] inWindArea;
	private List<Rigidbody> itemsInArea = new List<Rigidbody>();
    private bool particleSystemSpawned = false;
    private ParticleSystem.ShapeModule shapeModule; // has to be here for some reason 

    private float distance;
    private bool status=false;


	void Start () 
    {
				
	}
	

	void FixedUpdate () 
    {
        if(status)
        {        
		    foreach (Rigidbody item in itemsInArea)
		    {
			    item.AddForce((Vector3)(windEndPoint.position-windStartPoint.position) * (force/Vector3.Distance(windStartPoint.position,item.transform.position)* Time.deltaTime));
		    }
        }
	}

	public void AddToList(Rigidbody item)
	{
		itemsInArea.Add(item);
	}

	public void RemoveFromList(Rigidbody item)
	{
		itemsInArea.Remove(item);
	}

    public void ToggleBoolean()
    {
        if(!status)
        {
            Debug.Log(this.gameObject.GetComponent("Animator"));
            if(this.gameObject.GetComponent("Animator")!=null)
            {
                this.gameObject.GetComponent<Animator>().enabled = true;
            }            
            if(!particleSystemSpawned)
            {
				rotation = Quaternion.LookRotation((windEndPoint.position - windStartPoint.position).normalized);
				visualWind = (GameObject)Instantiate(visualWind, windStartPoint.position, rotation);
                visualWind.transform.parent = this.transform;
                shapeModule = visualWind.GetComponent<ParticleSystem>().shape;
                shapeModule.box = new Vector3(windBoxX, windBoxY, 1);
                particleSystemSpawned = true;
            }
            else
            {
                visualWind.GetComponent<ParticleSystem>().Play();
            }
            status = true;
        }
        else if(status)
        {            
            if(particleSystemSpawned)
            {
                if (this.gameObject.GetComponent("Animator") != null)
                {
                    this.gameObject.GetComponent<Animator>().enabled = false;
                }
                visualWind.GetComponent<ParticleSystem>().Stop();
            }
            status = false;
        }
    }
}

//if(status==false)
//{
//    if(yRotation==180)
//    {
//        ventilationFan.GetComponent<Animator>().enabled=true;
//        ventilationFan.GetComponent<WindBlower>().enabled=true;
//        particles.GetComponent<ParticleSystem>().Play();
//        status=true;
//    }
//    else
//    {
//        return;
//    }
//}
//if(status==true)
//{
//    if(yRotation<100)
//    {
//        ventilationFan.GetComponent<Animator>().enabled=false;
//        ventilationFan.GetComponent<WindBlower>().enabled=false;
//        particles.GetComponent<ParticleSystem>().Stop();
//        status=false;
//    }
//    else
//    {
//        return;
//    }