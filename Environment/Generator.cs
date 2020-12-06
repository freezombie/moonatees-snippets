using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour 
{
	public enum ConductingStatus { Off=0, Conducting=1, ConstantlyConducting=2}

	public ConductingStatus conducting;
	public GameObject[] activatedObjects;
	public GameObject electricArc;


	private List<ElectricArc> electricArcs = new List<ElectricArc>();
	private Transform sparklesTransform;
	private List<Transform> linkedItems = new List<Transform>(); // maybe store the first items here, maybe it's needed at some point. THIS is not done currently.



	void Awake()
	{
		GameObject sparkles = new GameObject("Sparkles");
		sparkles.transform.parent = this.gameObject.transform;
		sparkles.transform.localPosition = new Vector3(0,0,0);
		sparkles.transform.rotation = new Quaternion(0,0,0,0);
		sparklesTransform = sparkles.transform;
	}

	void Start () 
	{
		if((int)conducting==2)
		{
			NewArc(this.gameObject.transform, sparklesTransform);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		//Katso onko noisea jos ei tee uus noise.

		foreach(ElectricArc arc in electricArcs)
		{
			if(arc.GetNoise()==null)
			{
				arc.SetNoise(new Perlin());
			}
			float timex = Time.time * arc.GetSpeed() * 0.1365143f;
			float timey = Time.time * arc.GetSpeed() * 1.21688f;
			float timez = Time.time * arc.GetSpeed() * 2.5564f;
			for (int i = 0 ; i < arc.GetParticles().Length ; i++)
			{
				Vector3 position = Vector3.Lerp(arc.GetOrigin().position,arc.GetTarget().position,arc.GetOneOverZigs()*(float)i);
				Vector3 offset = new Vector3(arc.GetNoise().Noise(timex + position.x, timex + position.y, timex + position.z),
					                         arc.GetNoise().Noise(timey + position.x, timey + position.y, timey + position.z),
					                         arc.GetNoise().Noise(timez + position.x, timez + position.y, timez + position.z));
				position += (offset * arc.GetScale() * (float)i * arc.GetOneOverZigs());
				arc.SetParticlePosition(i,position);
				arc.SetParticleColor(i,Color.white);
				arc.SetParticleEnergy(i,1f);
			}
			arc.UpdateParticles();
		}        
	}

	public void ToggleBoolean ()
	{
        //Debug.Log("Toggleboolean at " + this.gameObject.name); 
		if ((int)conducting==0)
		{
			conducting = (ConductingStatus)1;
			NewArc(this.gameObject.transform,sparklesTransform);

			foreach(GameObject obj in activatedObjects)
			{
				obj.SendMessage("ToggleBoolean");
			}
			return;
		}
		if ((int)conducting == 1)
		{
			conducting = (ConductingStatus)0;
			ElectricArc toBeDeleted= GetArc(this.gameObject.transform, sparklesTransform);
			electricArcs.Remove(toBeDeleted);
			toBeDeleted.Destroy();

			foreach (GameObject obj in activatedObjects)
			{
				obj.SendMessage("ToggleBoolean");
			}
			return;
		}
	}

	public void NewArc(Transform origin,Transform target)
	{        
   		Generator gen=(Generator)target.GetComponent("Generator");
		ItemProperties ip=(ItemProperties)target.GetComponent("ItemProperties");

		if(!CheckArcs(origin, target))
		{
			//Debug.Log("There was no arc between " + origin.name + " and " + target.name + " so we made one.");
			electricArcs.Add(new ElectricArc(origin, target, Instantiate(electricArc), 100, 1f, 1f));
			if (gen != null && gen.conducting==(ConductingStatus)0)
			{
				gen.ToggleBoolean();
			}
			else if (ip != null)
			{
				ip.ChangeConductingStatus(true);
				target.GetComponentInChildren<ElectricArea>().SetGenerator(this);
				List<Collider> itemsInAreaofTarget = new List<Collider>();
				itemsInAreaofTarget = target.GetComponentInChildren<ElectricArea>().GetItemsInArea();
				if(itemsInAreaofTarget.Count>0)
				{
					foreach(Collider col in itemsInAreaofTarget)
					{
						NewArc(target,col.transform);
					}
				}
			}
			gen = null;
			ip = null;
		}
	}   

	public bool CheckArcs(Transform origin, Transform target)
	{
		for (int i=0 ; i < electricArcs.Count ; i++)
		{
			if(electricArcs[i].GetOrigin()==origin && electricArcs[i].GetTarget()==target)
			{
				//Debug.Log("There is an arc going from " + origin.name + " to " + target.name);
				return true;
			}
			if(electricArcs[i].GetOrigin()==target && electricArcs[i].GetTarget()==origin)
			{
				//Debug.Log("There is an arc going from " + target.name + " to " + origin.name);
				return true;
			}
		}
		return false;
	}

	public bool HasArcs(Transform target)
	{
		for(int i=0 ; i < electricArcs.Count ; i++)
		{
			if(electricArcs[i].GetOrigin()==target)
			{
				List<ElectricArc> arcs = GetArcs(target, false);
				if(arcs.Count > 1)
				{
					return true;
				}
				else
				{
					if(arcs[0].GetTarget()==sparklesTransform)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			else if(electricArcs[i].GetTarget()==target)
			{
				return true;
			}
		}
		return false;
	}

	public ElectricArc GetArc(Transform origin, Transform target)
	{
		for (int i=0 ; i < electricArcs.Count ; i++)
		{
			if(electricArcs[i].GetOrigin() == origin && electricArcs[i].GetTarget() == target)
			{
				return electricArcs[i];
			}
		}
		return null;
	}

	public void RemoveArc(ElectricArc arc)
	{
		Transform target = arc.GetTarget();
		ItemProperties ip = target.GetComponent<ItemProperties>();
		Generator gen = GetComponent<Generator>();
		electricArcs.Remove(arc);
		if(!isTargetofArc(target))
		{
			if(ip!=null)
			{
				ip.ChangeConductingStatus(false);
			}
			if(gen!=null)
			{
				gen.ToggleBoolean();
			}
		}
		arc.Destroy();
	}

	public bool isTargetofArc(Transform obj)
	{
		for(int i=0 ; i<electricArcs.Count ; i++)
		{
			if(electricArcs[i].GetTarget() == obj)
			{
				return true;
			}
		}
		return false;
	}

	public List<ElectricArc> GetArcs (Transform transform, bool isTarget) 
	{
		List<ElectricArc> results = new List<ElectricArc>();

		if(isTarget)
		{
			foreach (ElectricArc arc in electricArcs)
			{
				if (arc.GetTarget() == transform)
				{
					results.Add(arc);
				}
			}
		}
		else
		{
			foreach (ElectricArc arc in electricArcs)
			{
				if (arc.GetOrigin() == transform && arc.GetTarget() != sparklesTransform)
				{
					results.Add(arc);
				}
			}            
		}
		return results;
	}    
}


