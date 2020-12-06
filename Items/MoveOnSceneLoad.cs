using UnityEngine;
using System.Collections;

public class MoveOnSceneLoad : MonoBehaviour 
{
    private ItemManagerScript itemManager;
    private ItemManagerScript.ItemLocation itemLocation;    

	void Start () 
    {
        itemManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ItemManagerScript>();
        if (itemManager.GetListCount()>0)
        {
            StartCoroutine(moveItem());            
        }        
	}

    IEnumerator moveItem ()
    {
        yield return 0;
        itemLocation = itemManager.GetItemLocation(this.gameObject.GetComponent<ItemProperties>().GetID());
        if (itemLocation != null)
        {
            transform.position = itemLocation.position;
            transform.rotation = itemLocation.rotation;
            transform.localScale = itemLocation.scale;
            Debug.Log("Transforming to " + itemLocation.position);
        }
        else
        {
            Debug.Log("ItemLocation was null");
        }
    }
}
