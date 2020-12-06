using UnityEngine;
using System.Collections;

public class LiquidCoffee : MonoBehaviour {
    float lifetime;
    bool receivedLifetime;
    GameObject boardOfInstructions;
    void Update()
    {
        if (receivedLifetime)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetLifetime(float time)
    {
        lifetime = time;
        receivedLifetime = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "CoffeeTrigger")
        {
            if(col.transform.parent.FindChild("Water")!=null && !col.transform.parent.FindChild("Water").gameObject.activeSelf)
            {
                col.transform.parent.FindChild("Water").gameObject.SetActive(false);
            }
//            Debug.Log("Sending deactivate to BOI");
            col.transform.parent.gameObject.GetComponent<FillWithliquid>().ToggleInterAction(this.gameObject, FillWithliquid.Liquid.Coffee);
            boardOfInstructions.GetComponent<BoardOfInstructions>().Deactivate();
			Destroy (col.transform.gameObject);
        }
    }

    public void SetBoardOfInstructions(GameObject BOI)
    {
        boardOfInstructions = BOI;
    }
}
