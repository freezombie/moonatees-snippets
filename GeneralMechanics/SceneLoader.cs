using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour 
{    
    public enum ConnectedScenes
    {
        Closet = 0,
        Corridor = 1,
        ChestRoom = 2,
        ElectricityRoom = 3,
        WindRoom = 4,
        RotatingRoom = 5,
    };
    public List<ConnectedScenes> connectedScenes = new List<ConnectedScenes>();
	private GameObject[] objects;
	private ItemManagerScript itemManager;
	
    void Start()
    {
        itemManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ItemManagerScript>();
    }

	void OnTriggerExit(Collider other)
	{
		if(other.tag=="PlayerBody")
		{
			objects = GameObject.FindGameObjectsWithTag("Interactable");
			foreach(GameObject obj in objects)
			{
				if(obj.scene.buildIndex == this.gameObject.scene.buildIndex)
				{
                    itemManager.AddItemLocation(obj.transform, this.gameObject.scene.buildIndex, obj.GetComponent<ItemProperties>().GetID());
				}
			}
		}
	}

    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlayerBody")
		{
			List<int> loadedLevels = new List<int>();
			SceneManager.MoveGameObjectToScene(other.transform.parent.parent.gameObject, this.gameObject.scene);
			SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("GameController"), this.gameObject.scene);
			SceneManager.SetActiveScene(this.gameObject.scene);
            
            for (int i = SceneManager.sceneCount - 1 ; i >= 0 ; i--)
            {
                loadedLevels.Add(SceneManager.GetSceneAt(i).buildIndex);
            }
            foreach(ConnectedScenes scene in connectedScenes)
            {
                if(!loadedLevels.Contains((int)scene))
                {
                    SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
                }                
            }            
            List<int> scenesToUnload = new List<int>();
            foreach (int loaded in loadedLevels)
            {
                if (!connectedScenes.Contains((ConnectedScenes)loaded) && loaded!=this.gameObject.scene.buildIndex)
                {
                    scenesToUnload.Add(loaded);
                }
            }
            foreach (int loaded in scenesToUnload)
            {
                //StartCoroutine(unloadScene(loaded));
            }
		}
		else if(other.GetComponent("ItemProperties")!=null)
		{
			if(other.gameObject.scene.buildIndex!=this.gameObject.scene.buildIndex)
			{
				SceneManager.MoveGameObjectToScene(other.gameObject,this.gameObject.scene);
				if(itemManager.NameExists(other.gameObject.name))
				{					
					itemManager.MoveObjectToScene(other.GetComponent<ItemProperties>().GetID(),this.gameObject.scene.buildIndex);
				}
				else
				{
					itemManager.AddItemLocation(other.transform,gameObject.scene.buildIndex,other.GetComponent<ItemProperties>().GetID());
				}
			}
		}
	}
    
	public GameObject[] GetObjects()
	{
		return objects;
	}

    IEnumerator unloadScene(int loadedScene)
	{
        yield return 0;
        SceneManager.UnloadScene(loadedScene);		
        yield return 0;
	}


}
