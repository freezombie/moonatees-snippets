using UnityEngine;
using System.Collections;

public class CoffeeBeanScript : MonoBehaviour
{
    float lifetime;
    bool receivedLifetime;	
  
    void Update()
    {
        if(receivedLifetime)
        {
            lifetime -= Time.deltaTime;
            if(lifetime <= 0)
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
        if(col.transform.parent != null && col.transform.parent.name == "Coffeegrinder")
        {
            col.transform.parent.gameObject.SendMessage("AddIngredients",this.gameObject);
        }
    }
}
