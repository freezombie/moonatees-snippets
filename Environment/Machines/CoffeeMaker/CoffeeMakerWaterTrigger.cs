using UnityEngine;
using System.Collections;

public class CoffeeMakerWaterTrigger : MonoBehaviour 
{
    public GameObject boardOfInstructions;
	void OnTriggerEnter (Collider col)
    {        
        if(col.transform.FindChild("Water")!=null && col.transform.FindChild("Water").gameObject.activeSelf == true)
        {
            GetComponentInParent<FillWithliquid>().ToggleInterAction(col.gameObject,FillWithliquid.Liquid.Water);
            GetComponentInParent<CoffeeMaker>().ChangeHasWater(true);
            boardOfInstructions.GetComponent<BoardOfInstructions>().SetStateBoolean(true, 1);
            boardOfInstructions.GetComponent<BoardOfInstructions>().ChangeTexture(2);
        }
    }
}
