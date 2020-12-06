using UnityEngine;
using System.Collections;

public class StatusLamp : MonoBehaviour 
{
	Material redLight;
	public Material greenLight;
	Renderer rend;

	void Awake () 
	{
		rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("could not find renderer");
        }
		redLight = rend.material;
	}
	
	public void changeLightToRed()
	{
        if (rend.material == null)
        {
            Debug.Log("The material has not been assigned yet, that's why");
        }
        else if (redLight == null)
        {
            Debug.Log("red light has not been assigned yet, that's why");
        }
        else if(rend.material != redLight)
		{
			rend.material = redLight;
		}

	}

	public void changeLightToGreen()
	{		
        if (rend.material == null)
        {
            Debug.Log("The material has not been assigned yet, that's why");
        }
        else if (greenLight == null)
        {
            Debug.Log("Green light has not been assigned yet, that's why");
        }
        else if (rend.material != greenLight)
        {
            rend.material = greenLight;
        }

	}
}
