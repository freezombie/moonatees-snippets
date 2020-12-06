using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndScreenActivator : MonoBehaviour {

    public Animator ani;


    void Start()
    {
       
        ani.runtimeAnimatorController = Resources.Load("Canvas") as RuntimeAnimatorController;


    }
    
    public void Level(){
		StartCoroutine(LevelLoad());
	} 

	//load level after one sceond delay
	IEnumerator LevelLoad(){
        ani.Play("fadein");
        Debug.Log("fadein");
        yield return new WaitForSeconds(1f);
        Application.LoadLevel("EndScene");
	}

}