using UnityEngine;
using System.Collections;

public class CoffeePackScript : MonoBehaviour
{
    public int coffeeBeanAmount;
    public GameObject coffeeBean;
    Transform coffeeBeanSpawnPoint;
    Vector3 coffeeBeanSpawnPointV3;
    public float coffeeBeanLifeTime;
    int maxCoffeeBeans;
    int currentBeans;
    public int spawnAtOnce;
    Rigidbody rb;
    Vector3 velocity;
    float speed;
    float lastFrameSpeed;
    float deltaSpeed;
    public float amountOfRandomnessInVelocity;
    public float amountofRandomnessInSpawnPointXCoordinate;
    public float amountofRandomnessInSpawnPointZCoordinate;
    Vector3 coffeeBeanSpawnPointOriginalPos;
    public bool rigidbodyFound = false;
	
	void Start ()
    {
        maxCoffeeBeans = coffeeBeanAmount;
        rb = GetComponent<Rigidbody>();
        coffeeBeanSpawnPoint = transform.FindChild("CoffeeBeanSpawnPoint");
        currentBeans = 0;
        coffeeBeanSpawnPointOriginalPos = coffeeBeanSpawnPoint.transform.localPosition;
	}	
	
	void FixedUpdate ()
    {
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

        velocity = rb.velocity;
        speed = Mathf.Abs(velocity.magnitude); // this used to be velocity.y
        deltaSpeed = Mathf.Abs(speed - lastFrameSpeed);

        if (deltaSpeed > 0.15f) // The maxcoffeebeans limit is not yet here, not sure if it's needed since they die inside a minute
        {
            for(int i=0; i<spawnAtOnce; i++) // spawn as many beans as defined in spawnatonce
            {
                GameObject coffeeBeanClone;
                coffeeBeanSpawnPointV3 = new Vector3(coffeeBeanSpawnPoint.position.x + Random.Range(amountofRandomnessInSpawnPointXCoordinate * -1, amountofRandomnessInSpawnPointXCoordinate),coffeeBeanSpawnPoint.position.y, coffeeBeanSpawnPoint.position.z + Random.Range(amountofRandomnessInSpawnPointZCoordinate*-1,amountofRandomnessInSpawnPointZCoordinate));
                coffeeBeanClone = (GameObject)Instantiate(coffeeBean, coffeeBeanSpawnPointV3, new Quaternion(0, 0, 0, 0));
                currentBeans++;
                Rigidbody coffeeBeanRB = coffeeBeanClone.GetComponent<Rigidbody>();
                //coffeeBeanRB.velocity = (velocity) * -1;
                coffeeBeanRB.velocity = new Vector3(velocity.x*-1+Random.Range(amountOfRandomnessInVelocity * -1, amountOfRandomnessInVelocity), velocity.y * -1 + Random.Range(amountOfRandomnessInVelocity * -1, amountOfRandomnessInVelocity), velocity.z * -1 + Random.Range(amountOfRandomnessInVelocity * -1, amountOfRandomnessInVelocity));
                coffeeBeanClone.name = "CoffeeBean" + currentBeans;
                //coffeeBeanClone.transform.SetParent(transform);
                coffeeBeanClone.GetComponent<CoffeeBeanScript>().SetLifetime(coffeeBeanLifeTime);
            }            
        }
        lastFrameSpeed = speed;
	}
}
