using UnityEngine;
using System.Collections;

public class FillWithliquid : MonoBehaviour
{
    public enum Liquid { Water, Coffee };    
    GameObject liquidGameObject;
    
	public void ToggleInterAction(GameObject go,Liquid liquid)
    {		
        switch (liquid)
        {
            case Liquid.Water:				
                liquidGameObject = transform.FindChild("Water").gameObject;
                if (liquidGameObject.activeSelf == false)
                {
					liquidGameObject.SetActive(true);
                    if (go.transform.FindChild("Water") != null && go.transform.FindChild("Water").gameObject.activeSelf == true)
                    {
                        liquidGameObject.SetActive(true);
                        if (go.GetComponent("ReduceLiquid") != null)
                        {
                            go.GetComponent<ReduceLiquid>().Reduce();
                        }
                        //if (go.transform.FindChild("Water").gameObject.activeSelf == true)
                        //{

                        //}
                    }
                    else if (go.transform.parent != null && go.transform.parent.FindChild("Water") != null && go.transform.parent.FindChild("Water").gameObject.activeSelf == true)
                    {
                        liquidGameObject.SetActive(true);
                        if (go.GetComponent("ReduceLiquid") != null)
                        {
                            go.GetComponent<ReduceLiquid>().Reduce();
                        }
                        //if(go.transform.parent.FindChild("Water").gameObject.activeSelf == true)
                        //{

                        //}
                    }                    
                }
                break;
            case Liquid.Coffee:
                liquidGameObject = transform.FindChild("Coffee").gameObject;
                if (liquidGameObject.activeSelf == false)
                {
                    liquidGameObject.SetActive(true);
                }
                break;
        }        
    }
}
