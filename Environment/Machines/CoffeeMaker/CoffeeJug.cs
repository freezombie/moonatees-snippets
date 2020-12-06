using UnityEngine;
using System.Collections;
using NewtonVR;

public class CoffeeJug : MonoBehaviour 
{
    ParticleSystem ps;
    public bool coffeeReady = false;
    public Transform coffeeSpawnPoint;
    public GameObject LiquidCoffeeSphere;
    public GameObject playerBody;
    public float coffeeLifetime;
    Rigidbody rb;
    Rigidbody playerBodyRB;
	private Vector3 playerVelocity;
    public float speed;
	Vector3 velocity;
    public GameObject boardOfInstructions;

//    Grabbable thisGrabbable;
	NVRInteractableItem thisInteractableItem;

    void Start () 
    {
        rb = GetComponent<Rigidbody>();
        playerBody = GameObject.FindGameObjectWithTag("Player");
        playerBodyRB = playerBody.GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
//        thisGrabbable = GetComponent<Grabbable>();
		thisInteractableItem = GetComponent<NVRInteractableItem>();
	}
	
	public void StartSteam()
    {
        ps.Play();

        // This once crashed as there was no rigidbody anymore so..
        if (this.gameObject.GetComponent<Rigidbody>() != null)
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        
		this.gameObject.GetComponent<NVRInteractableItem>().enabled = true;
        coffeeReady = true;
    }
    void FixedUpdate()
    {       
        if(coffeeReady)
        {
//			playerVelocity = playerBodyRB.velocity;    
//			speed = playerVelocity.magnitude;

            /*
            if (this.GetComponent<Rigidbody>() == null)
            {
                rb = this.transform.parent.GetComponent<Rigidbody>();
            }
            else
            {
                rb = this.GetComponent<Rigidbody>();
            }
            */
            if (rb == null)
            {
                if (this.GetComponent<Rigidbody>() != null)
                {
                    rb = this.GetComponent<Rigidbody>();
                }
                else if (this.transform.parent.GetComponent<Rigidbody>() != null)
                {
                    rb = this.transform.parent.GetComponent<Rigidbody>();
                }
            }

            // Tänne päästiin kerran ja se kaatu ku rigidbodyä ei ollukkaa
			velocity = rb.velocity;
            speed = Mathf.Abs(velocity.magnitude);
//			speed = Mathf.Abs(velocity.y);
//			speed = Mathf.Abs(velocity); // This could be changed into checking the velocity in the reverse direction of the coffee packs 'mouthpiece'. It's not vital however.

			if (speed > 1f)
            {
                GameObject LiquidCoffeeClone;
                LiquidCoffeeClone = (GameObject)Instantiate(LiquidCoffeeSphere, coffeeSpawnPoint.position, new Quaternion(0, 0, 0, 0));
                LiquidCoffeeClone.GetComponent<Rigidbody>().AddForce(rb.velocity * 0.5f);
//				LiquidCoffeeClone.GetComponent<Rigidbody>().velocity = (playerVelocity);
                LiquidCoffeeClone.name = "LiquidCoffee";
//                LiquidCoffeeClone.transform.SetParent(transform);
                LiquidCoffee liquidCoffeeScript = LiquidCoffeeClone.GetComponent<LiquidCoffee>();
                liquidCoffeeScript.SetLifetime(coffeeLifetime);
                liquidCoffeeScript.SetBoardOfInstructions(boardOfInstructions);                
            }
        }        
    }
}
