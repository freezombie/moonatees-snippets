using UnityEngine;
using System.Collections;
using NewtonVR;

public class CoffeeGrinder : MonoBehaviour
{
    public GameObject cup;
    public GameObject coffee;
	public GameObject cover;
    Rigidbody cupRb;
    public float coverSpeed;
	public float cupSpeed;
    Vector3 cupOriginalPos;
    Vector3 coverOriginalPos;
	Vector3 grinderOriginalPos;
    Quaternion cupOriginalRot;
    Quaternion coverOriginalRot;
	Quaternion grinderOriginalRot;
    Vector3 cupTarget;
    bool cupAttached;
    bool cupReady; // we need to eject and make the cup so that the player can grab it etc.
    //bool transitionDone; // looks like we are not even using this for anything. Probably it was in the past, but cannot find logical reason to have it anymore.
    public int neededBeans;
    public float timeOn; // how long will this remain on.
    float timeOnBuffer; // the one we actually countdown.
    int beans;
    enum State { On, Off, Transition }
    State previousState;
    State currentState;
    Renderer coffeeRenderer;
    Material coffeeBeansMaterial;
    public Material coffeeGrindMaterial;
    public GameObject boardOfInstructions;
	private AudioSource myAudioSource;
    private bool coffeeDone = false;

	// Use this for initialization
	void Start ()
    {
        timeOnBuffer = timeOn;
        beans = 0;
        currentState = State.Off;
        cupAttached = true;
        cupReady = false;
        cupOriginalPos = new Vector3(cup.transform.localPosition.x,cup.transform.localPosition.y,cup.transform.localPosition.z);
        cupOriginalRot = new Quaternion(cup.transform.localRotation.x,cup.transform.localRotation.y,cup.transform.localRotation.z,cup.transform.localRotation.w);
        coverOriginalPos = new Vector3(cover.transform.localPosition.x, cover.transform.localPosition.y, cover.transform.localPosition.z);
        coverOriginalRot = new Quaternion(cover.transform.localRotation.x, cover.transform.localRotation.y, cover.transform.localRotation.z, cover.transform.localRotation.w);
        cupTarget = new Vector3 (cupOriginalPos.x, cupOriginalPos.y, cupOriginalPos.z);
        cupTarget.y -= 0.532124f;
        cupRb = cup.GetComponent<Rigidbody>();
		cup.GetComponent<NVRInteractableItem>().enabled = false;
        coffeeRenderer = coffee.GetComponent<Renderer>();
        coffeeBeansMaterial = new Material (coffeeRenderer.material);
		grinderOriginalPos = transform.position;
		grinderOriginalRot = transform.rotation;
		myAudioSource = GetComponent<AudioSource> ();
    }	

    public void AddIngredients(GameObject bean)
    {        
		if(beans!=neededBeans && !cupReady && cupAttached)
        {
            beans++;
            if (beans >= neededBeans / 3)
            {
                coffee.SetActive(true);
                // fill the cup up to third
            }
            if (beans >= neededBeans / 2)
            {
                coffee.transform.localPosition = new Vector3 (0f, 0f, -3.228f);                
                //fill the cup up to two thirds
            }
            if (beans >= neededBeans)
            {
                coffee.transform.localPosition = new Vector3(0f,0f,-2.943f);
                coffee.transform.localScale = new Vector3(1.1f,1f,1.1f);
                beans = neededBeans;
                //fill all of the cup
            }
            Destroy(bean);
        }        
    }  

    public void ChangeCupStatus (bool status)
    {
        cupAttached = status;
    }    

    public bool GetCupStatus()
    {
        return cupAttached;
    }

    public Vector3 GetCupOrigPos()
    {
        return cupOriginalPos;
    }

    public Vector3 GetCoverOrigPos()
    {
        return coverOriginalPos;
    }

    public Quaternion GetCupOrigRot ()
    {
        return cupOriginalRot;
    }

    public Quaternion GetCoverOrigRot ()
    {
        return coverOriginalRot;
    }

    public GameObject GetCoverObject()
    {
        return cover;
    }

    public GameObject GetCupObject()
    {
        return cup;
    }
    
    public bool GetCupReady()
    {
        return cupReady;
    }

    public void ToggleBoolean()
    {
		/*if(currentState==State.On)
        {            
            //float time = 5.0f;
            //for (int i = 0; i < time; time-=Time.deltaTime)
            //{
            //    //play sound
            //}
            currentState = State.Transition;
            transitionDone = false;
            previousState = State.On;
			myAudioSource.Stop ();
			transform.position = grinderOriginalPos;
			transform.rotation = grinderOriginalRot;
        }*/
        if (currentState == State.Off && cupAttached && beans == neededBeans && !coffeeDone)
        {
            myAudioSource.Play();
            currentState = State.Transition;
            //transitionDone = false;
            previousState = State.Off;
        }   
    }

    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            cupAttached = true;
            beans = neededBeans;
            ToggleBoolean();
        }*/
        if (currentState == State.On)
		{
			transform.position = grinderOriginalPos + new Vector3(Random.Range (-0.005f, 0.005f),Random.Range (-0.005f, 0.005f),Random.Range (-0.005f, 0.005f));
            timeOnBuffer -= Time.deltaTime;
            if(timeOnBuffer<=0)
            {
                currentState = State.Transition;
                //transitionDone = false;
                previousState = State.On;
                myAudioSource.Stop();
                transform.position = grinderOriginalPos;
                transform.rotation = grinderOriginalRot;
                timeOnBuffer = timeOn;
            }			
		}
		if (currentState == State.Transition)
        {
			float step = cupSpeed * Time.deltaTime;

            if (cupRb != null)
            {
                cupRb.isKinematic = true;
                cup.GetComponent<NVRInteractableItem>().enabled = false;
            }
            
            if (previousState == State.Off)
            {				
//				Debug.Log("angle: " + cover.transform.localEulerAngles.z);
				if(cover.transform.localEulerAngles.z >= 270)
				{
					cover.transform.Rotate(Vector3.forward * (coverSpeed * Time.deltaTime));
				}
				else
				{					
					cover.transform.localEulerAngles = Vector3.zero;
				}
				cup.transform.localPosition = Vector3.MoveTowards(cup.transform.localPosition, cupTarget, step);
				if(cup.transform.localPosition == cupTarget && cover.transform.localEulerAngles.z == 0)
                {
                    //transitionDone = true;
                    currentState = State.On;
					previousState = State.Transition;
                }
            }
            else // if previousstate was on.
            {
				if(cover.transform.localEulerAngles.z > 270 || cover.transform.localEulerAngles.z == 0)
				{
					cover.transform.Rotate(Vector3.back * (coverSpeed * Time.deltaTime));
				}
				else
				{					
					cover.transform.rotation = coverOriginalRot;
				}
                cup.transform.localPosition = Vector3.MoveTowards(cup.transform.localPosition, cupOriginalPos, step); // go up
				if(coffeeRenderer.material != coffeeGrindMaterial)
				{
					coffeeRenderer.material = coffeeGrindMaterial;
				}
				if (cover.transform.localPosition == coverOriginalPos && cup.transform.localPosition == cupOriginalPos)
                {
                    cup.transform.localPosition = cupOriginalPos;
                    cover.transform.localPosition = coverOriginalPos;
                    currentState = State.Off;
                    cupRb.isKinematic = false;
                    cupRb.velocity = Vector3.zero;
                    cupRb.angularVelocity = Vector3.zero;                  
                    cup.transform.parent = null; // KOKEILU
                    cup.GetComponent<NVRInteractableItem>().enabled = true;
                    boardOfInstructions.GetComponent<BoardOfInstructions>().SetStateBoolean(true, 2);
                    boardOfInstructions.GetComponent<BoardOfInstructions>().ChangeTexture(3);
                    cupReady = true;
                    coffeeDone = true;
                }
            }
        }
    }
}
