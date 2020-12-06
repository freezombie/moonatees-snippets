using UnityEngine;
using System.Collections;

public class AquariumGridObject
{
	GameObject go;
	int x;
	int z;
	bool active=false;

	public AquariumGridObject (GameObject gameObject, int newX, int newZ)
	{
		go = gameObject;
		x = newX;
		z = newZ;
	}

	public GameObject GetGameObject()
	{
		return go;
	}

	public int getX()
	{
		return x;
	}

	public int getZ()
	{
		return z;
	}

	public void setActive(bool status)
	{
		active = status;
	}

	public bool getActive()
	{
		return active;
	}
}
