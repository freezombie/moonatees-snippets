using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemManagerScript : MonoBehaviour 
{
	public class ItemLocation
	{
        public String name;
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		public int scene;
        public Guid ID;

		public ItemLocation(Transform trans, int sce,Guid itemID)
		{
            name = trans.name;
            position = trans.position;
			rotation = trans.rotation;
			scale = trans.localScale;
			scene = sce;
            ID = itemID;
		}
	}

	public List<ItemLocation> locations = new List<ItemLocation>();

	public void AddItemLocation(Transform trans, int sce, Guid ID)
	{
		ItemLocation itemLocation = new ItemLocation(trans,sce,ID);
        if(!locations.Exists(x => x.ID == ID)) //If an object with this ID doesn't exist...
        {
            Debug.Log("Added something to the list");
            locations.Add(new ItemLocation(trans, sce, ID)); //...then we add it.
        }
        else // if an object with this ID does exist
        {
            if (!Contains(locations, itemLocation)) // then we check if it's exactly the same, if not...
            {
                Debug.Log("Added something to the list and removed the old one");
                locations.RemoveAt(locations.FindIndex(ItemLocation => ItemLocation.ID.Equals(ID)));// ...we delete the old one...
                locations.Add(new ItemLocation(trans, sce, ID)); // and replace it with a new one.
            }
        }
	}

    public ItemLocation GetItemLocation(Guid ID)
    {
        int index=-1;
        for (int i = 0 ; i < locations.Count ; i++)
        {
            Debug.Log("locations[i].ID " + locations[i].ID + " The ID we are comparing this to " + ID);
            if(locations[i].ID==ID)
            {
                index = i;
                return locations[index];
            }
        }
        Debug.Log("GetItemLocation Index: " + index);
        return null;     
        //ItemLocation result = locations.Find(
        //delegate(ItemLocation il)
        //{
        //    return il.objectInstanceId == instanceID;
        //}
        //);
        //return result;
    }
	
	private bool Contains(List<ItemLocation> list, ItemLocation itemLocation)
	{
		Vector3 position = itemLocation.position;
		Quaternion rotation = itemLocation.rotation;
		Vector3 scale = itemLocation.scale;

		int scene = itemLocation.scene;
		Guid objectID = itemLocation.ID;
		foreach(ItemLocation item in list)
		{
			Vector3 listItemPosition = item.position;
			Quaternion listItemRotation = item.rotation;
			Vector3 listItemScale = item.scale;

			int listItemScene = item.scene;
			Guid listItemObjectID = item.ID;
			if (position == listItemPosition && rotation == listItemRotation && scale == listItemScale && scene == listItemScene && objectID == listItemObjectID)
			{
				return true;
			}
		}
		return false;
	}

    public int GetListCount()
    {
        return locations.Count;
    }

    public void ToggleBoolean ()
    {
        Debug.Log("ListCount: " + locations.Count);
        foreach (ItemLocation loc in locations)
        {
            Debug.Log("ID: " + loc.ID + " Transform: " + loc.position + " Scene: " + loc.scene);
        }
    }

    public bool NameExists(String name)
    {
        foreach(ItemLocation item in locations)
        {
            if(String.Compare(name,item.name)==0)
            {
                return true;
            }
        }
        return false;
		// This probably can work with findindex or something
    }

    public Guid GetID(string name)
    {
        foreach(ItemLocation item in locations)
        {
            if (String.Compare(name, item.name) == 0)
            {
                return item.ID;
            }
        }
        return Guid.Empty;
		//This probably can work with findindex or something
    }

	public void MoveObjectToScene(Guid ID, int buildindex)
	{
		int index = locations.FindIndex(ItemLocation => ItemLocation.ID.Equals(ID));
		locations[index].scene = buildindex;
	}
}
