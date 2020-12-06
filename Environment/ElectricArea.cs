using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElectricArea : MonoBehaviour
{
	private Generator gen;
	private Generator generator;
	private ItemProperties ip;

	private List<Collider> itemsInArea = new List<Collider>();
	private bool sentDrawArcs = false;



	void OnTriggerEnter (Collider col)
	{
		//if (col.GetComponent ("ItemProperties") != null || col.GetComponent ("Generator") != null)
		//{
		//	itemsInArea.Add(col);
		//}
        if (col != transform.parent && generator != null)
        {
            if (col.GetComponent("ItemProperties") != null || col.GetComponent("Generator") != null)
            {                
				itemsInArea.Add(col);
				sentDrawArcs = false;
//				Debug.Log ("Generator/Item entered:" + transform.parent.name);
                if (col.GetComponent("ItemProperties") != null)
                {
                    if (transform.parent.GetComponent<ItemProperties>().GetConductingStatus())
                    {
                        if (col.GetComponent<ItemProperties>().conductivity)
                        {
                            generator.NewArc(transform.parent, col.transform);
                        }
                    }
                }
                else
                {
//					Debug.Log ("Trying newarc between " + transform.parent.name + " and " + col.transform.name);
                    generator.NewArc(transform.parent, col.transform);
                }
            }			                      
        }
		else if (generator == null && col.GetComponent("Generator") != null && col.GetComponent<Generator>().conducting==Generator.ConductingStatus.ConstantlyConducting)
        {
			generator = col.GetComponent<Generator>();
			itemsInArea.Add(col);
        }
	}

	void OnTriggerStay (Collider col)
	{
		//gen = (Generator)col.GetComponent("Generator");
		if(generator!=null && (int)generator.conducting==2 && sentDrawArcs==false) //constantly, not only conducting
		{
//            Debug.Log("It's because of this");
			if(col.GetComponent("Generator")!=null)
			{
				sentDrawArcs = true;
				generator.NewArc(generator.transform, transform.parent);
			}
		}
	}


	void OnTriggerExit (Collider col)
	{
		if (itemsInArea.Contains(col))
		{            
			itemsInArea.Remove(col);
			StartCoroutine(ContinueArcRemoval(col));
		}
        if (col == generator)
        {
            generator = null;
        }
	}

	public bool linkedToGenerator(Transform target)
	{
		List<Collider> itemsGoneThrough = new List<Collider>();
		List<Collider> helpList = new List<Collider>();
		Generator gen=(Generator)target.GetComponent("Generator");        
		ItemProperties ip=(ItemProperties)target.GetComponent("ItemProperties");
		if(gen!=null)
		{
			if((int)gen.conducting==2)
			{
				return true;
			}
			if(generator.isTargetofArc(target))
			{
				List<ElectricArc> arcs = generator.GetArcs(target, true); //get the object that is the origin, do this same check from there.
				bool returningValue = false;
				for(int i=0;i<arcs.Count;i++)
				{
					returningValue = linkedToGenerator(arcs[i].GetOrigin());
					if(returningValue)
					{
						return true;
					}
				}
				return false;
			}
		}
		if(ip!=null)
		{
			helpList = target.GetComponentInChildren<ElectricArea>().GetItemsInArea(); 

			foreach (Collider item in helpList)
			{
				if (item.gameObject != this.gameObject)
				{
					itemsGoneThrough.Add(item);
				}
			}

			helpList = null;
			gen = null;
			ip = null;

			for (int i = 0 ; i < itemsGoneThrough.Count ; i++)
			{
				gen = (Generator)itemsGoneThrough[i].GetComponent("Generator");
				ip = (ItemProperties)itemsGoneThrough[i].GetComponent("ItemProperties");
				// for now this function returns true only for the constantly conducting one, 
				//since the sometimes conducting one can be the first item when doing the check on that generator 
				//and probably needs more thought
				if(gen!=null) 
				{
					if((int)gen.conducting==2)
					{
						return true;
					}
				}
				else if(ip!=null)
				{
					helpList = itemsGoneThrough[i].GetComponentInChildren<ElectricArea>().GetItemsInArea();
					if (helpList != null)
					{
						foreach (Collider item in helpList)
						{
							if (!itemsGoneThrough.Contains(item))
							{
								itemsGoneThrough.Add(item);
							}
						}
						helpList = null;
					}
				}                
			}
			return false;
		}
		return false;        
	}

	public void RemoveArcs(Transform target) 
	{
		List<ElectricArc> arcs = new List<ElectricArc>(generator.GetArcs(target, true));
		List<ElectricArc> helpList = new List<ElectricArc>(generator.GetArcs(target, false));
		ItemProperties ip = (ItemProperties)target.GetComponent("ItemProperties");
		ElectricArea ea = null;
		Generator gen = (Generator)target.GetComponent("Generator");

		if(gen!=null)
		{
			List<ElectricArc> arcs2 = new List<ElectricArc>(gen.GetArcs(target.transform, false));
			for(int i=0; i>arcs2.Count;i++)
			{
				gen.RemoveArc(arcs2[i]);
			}
			arcs2.Clear();
			arcs2 = new List<ElectricArc>(generator.GetArcs(target.transform, true));
			for (int i = 0 ; i > arcs2.Count ; i++)
			{
				RemoveArcs(arcs2[i].GetOrigin());
			}
		}

		if(ip!=null && ip.conductivity)
		{
			ea = target.GetComponentInChildren<ElectricArea>();
		}

		arcs.AddRange(helpList);
		helpList.Clear();

		foreach (ElectricArc arc in arcs)
		{
			generator.RemoveArc(arc);            
		}

		if(ea!=null)
		{
			List<Collider> items = ea.GetItemsInArea();
			foreach(Collider col in items)
			{
				if(generator.HasArcs(col.transform))
				{
					RemoveArcs(col.transform);
				}                
			}
		}
	}

	public List<Collider> GetItemsInArea()
	{
		return itemsInArea;        
	}

	public void SetGenerator(Generator newGenerator)
	{
		generator = newGenerator;
	}
	
	IEnumerator ContinueArcRemoval (Collider col)
	{
		Generator gen = (Generator)col.GetComponent("Generator");
		ItemProperties ip = (ItemProperties)col.GetComponent("ItemProperties");
		bool parentConducting = transform.parent.GetComponent<ItemProperties>().GetConductingStatus();

		yield return 0;

		if (ip != null &&  generator != null && generator.HasArcs (col.transform)) 
        {
            ElectricArc firstArc = null;
            firstArc = generator.GetArc (transform.parent, col.transform);
            if (firstArc == null)  // if we didn't find that particular arc
            {
                firstArc = generator.GetArc (col.transform, transform.parent); // we check if it exists the other way around
            }
			if (firstArc != null) 
            {
				if (generator.GetArc (transform.parent, col.transform) != null) 
                {
                    generator.RemoveArc (firstArc); //this is in this if in case the arc has been destroyed by the other items coroutine.
				}
				if (!linkedToGenerator (col.transform)) //Check if the exiting one was connected to a generator. if not, remove all the arc that are connected to it and from it.
                {
					RemoveArcs (col.transform);
			    }                   
			}
		}
		else if (gen != null)
		{
			if ((int)gen.conducting != 0 && parentConducting) 
			{
				ElectricArc firstArc = null;
				firstArc = generator.GetArc(transform.parent, col.transform);
				if (firstArc == null) // if we didn't find that particular arc
				{
					firstArc = generator.GetArc(col.transform, transform.parent); // we check if it exists the other way around
				}
				if (firstArc != null) // if any of the above produced an arc, we destroy it
				{
					generator.RemoveArc(firstArc); //this is in this if in case the arc has been destroyed by the other items coroutine.
				}
				if (!linkedToGenerator(col.transform)) //Check if the exiting one was connected to a generator. if not, remove all the arc that are connected to it and from it.
				{
					RemoveArcs(col.transform);
					if(!linkedToGenerator(transform.parent))
					{
						RemoveArcs(transform.parent);
					}
				}
				//check if connected to a generator
				gen.ToggleBoolean();
			}
			if ((int)gen.conducting == 2 && parentConducting)
			{
				//remove the arc from that to this, and check if the items in the area of this are connected to gen.
				sentDrawArcs = false;
			}
		}       

		yield return 0;
	}
}
