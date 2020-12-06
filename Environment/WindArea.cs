using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindArea : MonoBehaviour 
{
//	private List<Collider> inArea = new List<Collider>();


	void OnTriggerEnter(Collider item)
	{
		if(item.tag=="Item" || item.tag == "Player")
		{
//			inArea.Add(item);
			this.gameObject.GetComponentInParent<WindBlower>().AddToList(item.GetComponent<Rigidbody>());
		}
	}

	void OnTriggerExit(Collider item)
	{
		if(item.tag=="Item" || item.tag == "Player")
		{
//			inArea.Remove(item);
			this.gameObject.GetComponentInParent<WindBlower>().RemoveFromList(item.GetComponent<Rigidbody>());
		}
	}

//	public int getListCount()
//	{
//		return inArea.Count;
//	}
//
//	public Collider getItem(int index)
//	{
//		return inArea[index];
//	}
}