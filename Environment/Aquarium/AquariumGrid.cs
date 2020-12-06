using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AquariumGrid : MonoBehaviour
{
    public GameObject gridObject;
    public GameObject fish;
	public GameObject itemSpawnPos;
    public GameObject cannon;
    public GameObject cannonExplosion;
    AudioSource cannonAS;
    public AudioClip[] cannonAudioClips;
    GameObject helper;
    List<AquariumGridObject> gridObjects = new List<AquariumGridObject>();
    int xCoordinate;
    int zCoordinate;
    float startXPos;
    float startZPos;
    float endXPos;
    float endZPos;
    float XPos;
    float ZPos;
    float buttonPressBuffer = 0;
    public float bufferForButton = 0.2f;
    public float circleRadius;
    public float aimedHeight;
    public float risingSpeed;
	int activeX=3;
	int activeZ=1;
    Fish fishScript;
    int fishCoordinateX = 3;
	int fishCoordinateZ = 1;
    int fishTargetX = 0;
    int fishTargetZ = 0;
    GameObject fishTargetGameObject;
	public float fishSpeed;
	public float fishRotationSpeed;
	public int framesForVortexAnimation;
    float oneStepX; // for now it's divided to 7 steps in start
    float oneStepZ; // for now it's divided to 3 steps in start
	bool inactive = false;
	bool fishMoving = false; // maybe it's a bit redundant but i prefer this instead of checking for animations.
    bool explodeFish = false;
    bool fishInVortex = false;
    bool fishLoaded = false;
    bool vortexAnimOn = false; // change vortex animation into this and do it while in the update.
	bool vortexGrowAnimDone = false;
    int vortexAnimX; // badly written, don't need to be global, come up with solution mby.
    int vortexAnimZ;
    Vector3 originalVortexScale;
	Vector3 targetDir;
    GameObject helperGameObject;
    public Vector3 largeVectorLocalScale;
    float beforeVortexX;
    float beforeVortexZ;
    public float secondsForFullCircle;
    float fishAngle;
    bool fadeExplosion = false;
    public float explosionFadeSpeed;

    void Start()
    {
        secondsForFullCircle = 2 * Mathf.PI / secondsForFullCircle;
        fishScript = fish.GetComponent<Fish>();
        xCoordinate = 0;
        zCoordinate = 0;
        startXPos = gridObject.transform.localPosition.x; 
        startZPos = gridObject.transform.localPosition.z;
        XPos = startXPos;
        ZPos = startZPos;
        endXPos = 42f; 
        endZPos = 8.9f; 
        oneStepX = endXPos - startXPos;
        oneStepX = oneStepX / 6;
        oneStepZ = endZPos - startZPos;
        oneStepZ = oneStepZ / 2;
        cannonAS = cannon.GetComponent <AudioSource>();
        for (xCoordinate = 0; xCoordinate < 7; xCoordinate++)
        {
            for (zCoordinate = 0; zCoordinate < 3; zCoordinate++)
            {             
                helper = Instantiate(gridObject, new Vector3(XPos, gridObject.transform.localPosition.y, ZPos),Quaternion.identity, this.gameObject.transform) as GameObject;
                helper.transform.localPosition = new Vector3(XPos, gridObject.transform.localPosition.y, ZPos);
				//helper.GetComponentInChildren<ParticleSystem>().Stop();
                // this is the actual one to be used, but there's a problem with particlesystem culling so that they are not visible from certain angles and stuff.
                helper.name = helper.name + " " + xCoordinate + "," + zCoordinate;
                /*START OF PLACEHOLDER CODE*/
                helper.transform.GetChild(0).gameObject.SetActive(false);
                /*END OF PLACEHOLDER CODE*/
                gridObjects.Add(new AquariumGridObject(helper, xCoordinate, zCoordinate));
                ZPos += oneStepZ;
            }      
            XPos += oneStepX;
            ZPos = startZPos;           
        }
        Destroy(gridObject);
		fish.transform.localPosition = new Vector3(getGoFromCoordinate(3,1).transform.localPosition.x,gridObject.transform.localPosition.y,getGoFromCoordinate(3,1).transform.localPosition.z);
		changeActive(3,1);
        vortexAnimX = 3;
        vortexAnimZ = 1;
        originalVortexScale = new Vector3(getGoFromCoordinate(3, 1).transform.GetChild(0).localScale.x, getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).localScale.y, getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).localScale.z);       
    }

	public void Right()
	{
        if(activeX > 0 && !inactive && buttonPressBuffer == 0)
		{
            buttonPressBuffer = bufferForButton;
            inactive = true;
            activeX -=1;
			changeActive(activeX,activeZ);
		}
	}

	public void Left()
	{
        if(activeX < 5 && !inactive && buttonPressBuffer == 0)
		{
            buttonPressBuffer = bufferForButton;
            inactive = true;
            activeX +=1;
			changeActive(activeX,activeZ);
		}

	}

	public void Up()
	{
        //Debug.Log("Received up");
        if (activeZ > 0 && !inactive && buttonPressBuffer == 0)
        {
            buttonPressBuffer = bufferForButton;
            inactive = true;
            activeZ -= 1;
            //Debug.Log("Sending " + activeX + " and " + activeZ + " into changeactive");
            changeActive(activeX, activeZ);
        }
        /*else
            Debug.Log("Didn't get into if, inactive: " + inactive + " activeZ: " + activeZ);*/
    }

	public void Down()
	{
        if(activeZ < 1 && !inactive && buttonPressBuffer == 0)
		{
            buttonPressBuffer = bufferForButton;
            inactive = true;
            activeZ +=1;
			changeActive(activeX,activeZ);
		}
	}

	public void Vortex()
	{
        if(!inactive && buttonPressBuffer == 0)
		{
            buttonPressBuffer = bufferForButton;
			inactive = true;
            vortexAnimOn = true;
            vortexAnimX = activeX;
            vortexAnimZ = activeZ;
            originalVortexScale = new Vector3(getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).localScale.x, getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).localScale.y, getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).localScale.z);
            if (checkIfActive(fishCoordinateX,fishCoordinateZ))
			{				
                inactive = true;
                //REMOVE THESE 3 LINES. For debugging, these explode the fish instantly.
//                fishInVortex = true;
//                beforeVortexX = fish.transform.localPosition.x;
//                beforeVortexZ = fish.transform.localPosition.z;
                if (fishCoordinateX==0 && fishCoordinateZ == 0 || fishCoordinateX==0 && fishCoordinateZ == 2 || fishCoordinateX==6 && fishCoordinateZ == 0 || fishCoordinateX==6 && fishCoordinateZ == 2)
				{					
                    fishInVortex = true;
                    beforeVortexX = fish.transform.localPosition.x;
                    beforeVortexZ = fish.transform.localPosition.z;
				}
				else if(fishCoordinateX == 1 && fishCoordinateZ == 0 && !checkIfActive(0,0)) // if the fish can move to the bottom left corner it will.
                {
                    fishTargetX = 0;
                    fishTargetZ = 0;
                    fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
                    fishMoving = true;
                }
				else if(fishCoordinateX == 1 && fishCoordinateZ == 2 && !checkIfActive(0,2)) // if the fish can move to the top left corner it will.
                {
                    fishTargetX = 0;
                    fishTargetZ = 2;
                    fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
                    fishMoving = true;
                }
				else if(fishCoordinateX == 1 && fishCoordinateZ == 1) // if we have two choices, see if either is blocked and go to the other, else random which one we go to.
                {
					if(checkIfActive(0,0))
					{
						fishTargetX = 0;
						fishTargetZ = 2;
						fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
						fishMoving = true;
					}
					else if(checkIfActive(0,2))
					{
						fishTargetX = 0;
						fishTargetZ = 0;
						fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
						fishMoving = true;
					}
					else
					{					
	                    if(Random.Range(0,1)<=0.5)
	                    {
	                        fishTargetX = 0;
	                        fishTargetZ = 0;
	                        fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
	                        fishMoving = true;
	                    }
	                    else
	                    {
	                        fishTargetX = 0;
	                        fishTargetZ = 2;
	                        fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
	                        fishMoving = true;
	                    }
					}
                }
				else if(fishCoordinateX == 5 && fishCoordinateZ == 0 && !checkIfActive(6,0)) // if the fish can move to the bottom left corner it will.
				{
					fishTargetX = 6;
					fishTargetZ = 0;
					fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
					fishMoving = true;
				}
				else if(fishCoordinateX == 5 && fishCoordinateZ == 2 && !checkIfActive(6,2)) // if the fish can move to the top right corner it will.
				{
					fishTargetX = 6;
					fishTargetZ = 2;
					fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
					fishMoving = true;
				}
				else if(fishCoordinateX == 5 && fishCoordinateZ == 1) // if we have two choices, see if either is blocked and go to the other, else random which one we go to.
				{
					if(checkIfActive(6,0))
					{
						fishTargetX = 6;
						fishTargetZ = 2;
						fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
						fishMoving = true;
					}
					else if(checkIfActive(6,2))
					{
						fishTargetX = 6;
						fishTargetZ = 0;
						fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
						fishMoving = true;
					}
					else
					{					
						if(Random.Range(0,1)<=0.5)
						{
							fishTargetX = 6;
							fishTargetZ = 0;
							fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
							fishMoving = true;
						}
						else
						{
							fishTargetX = 6;
							fishTargetZ = 2;
							fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
							fishMoving = true;
						}
					}
				}
                else
				{					
					int randomX=fishCoordinateX+Random.Range(-1,2);
					int randomZ=fishCoordinateZ+Random.Range(-1,2);

					for(bool found=false; found!=true; randomX=fishCoordinateX+Random.Range(-1,2),randomZ=fishCoordinateZ+Random.Range(-1,2))
					{
						if(fishCoordinateX == randomX && fishCoordinateZ == randomZ || randomX<0 || randomZ<0 || randomX>=7 || randomZ>=3)
						{
							found = false;
						}
						else if(checkIfActive(randomX,randomZ))
						{								
							found = false;
						}
						else
						{
							fishTargetX = randomX;
							fishTargetZ = randomZ;
							found = true;
						}
					}
                    fishTargetGameObject = getGoFromCoordinate(fishTargetX, fishTargetZ);
					fishMoving = true;
				}
			}
			/*END OF PLACEHOLDER CODE*/
		}        
	}

    /*IEnumerator VortexAnimation(int x, int z)
    {
        // moved these into update.
        
    }*/
    void changeActive(int toBeChangedX, int toBeChangedZ)
	{        
//        Debug.Log("Got into changeactive, tobechanged x,z: " + toBeChangedX + "," + toBeChangedZ);
        foreach (AquariumGridObject go in gridObjects)
		{
			if(go.getX()== toBeChangedX && go.getZ()== toBeChangedZ || go.getX()== toBeChangedX && go.getZ()== toBeChangedZ + 1 || go.getX()== toBeChangedX + 1 && go.getZ()== toBeChangedZ || go.getX()== toBeChangedX + 1 && go.getZ()== toBeChangedZ + 1)
			{
//                Debug.Log("got into if in changeactive");
				go.setActive(true);                
                helperGameObject = go.GetGameObject().transform.GetChild(0).gameObject;
                helperGameObject.SetActive(true);
                Animator anim = helperGameObject.GetComponent<Animator>();
                RuntimeAnimatorController ac = anim.runtimeAnimatorController;
                anim.Play("Armature|VortexAction",0,Random.Range(0f, ac.animationClips[0].length));
                helperGameObject = null;
                anim = null;
            }
			else
			{
//                Debug.Log("got into else");
				if(go.getActive())
				{
//                    Debug.Log("got into the other if");
					go.setActive(false);
                    go.GetGameObject().transform.GetChild(0).gameObject.SetActive(false);
                }
            }
		}
        inactive = false;
    }

	bool checkIfActive(int findX, int findZ)
	{
		foreach (AquariumGridObject go in gridObjects)
		{
			if(go.getX()==findX && go.getZ()==findZ)
			{
				return go.getActive();
			}
		}
		return false;
	}


	GameObject getGoFromCoordinate(int findX, int findZ)
	{
		foreach (AquariumGridObject go in gridObjects)
		{
			if(go.getX()==findX && go.getZ()==findZ)
			{
				return go.GetGameObject();
			}
		}
		return null;
	}

    IEnumerator LoadAndShoot()
    {
        cannonAS.Play();
        yield return new WaitForSeconds(cannonAudioClips[0].length + 0.2f);
        cannonAS.Stop();
        cannonAS.clip = cannonAudioClips[1];
        cannonExplosion.SetActive(true);
        fishLoaded = false;
        cannonAS.Play();
        fish.SetActive(true);
        fadeExplosion = true;
        explodeFish = true;
        yield return new WaitForSeconds(1.3f); //Actual animation time or similar here
        cannonExplosion.SetActive(false);    
        // play fish load sound. wait for a while, play explosion effects, set explodefish to true.
    }

	void Update()
	{        
		//for debugging
//		if(Input.GetKeyDown(KeyCode.Space))
//		{
//            Vortex();           
//		}
//		if(Input.GetKeyDown(KeyCode.W))
//		{
//			Up();
//		}
//		if(Input.GetKeyDown(KeyCode.S))
//		{
//			Down();
//		}
//		if(Input.GetKeyDown(KeyCode.A))
//		{
//			Left();
//		}
//		if(Input.GetKeyDown(KeyCode.D))
//		{
//			Right();
//		}
        //end of debug
        // The below is if we somehow get a fading material to the explosion.   
//        if (fadeExplosion)
//        {
//            foreach (SkinnedMeshRenderer mr in cannonExplosion.GetComponentsInChildren<SkinnedMeshRenderer>())
//            {
//                mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, mr.material.color.a - explosionFadeSpeed);
//            }
//        }
	    // end of possible fade.
        if (buttonPressBuffer > 0)
        {
            buttonPressBuffer -= Time.deltaTime;
            if (buttonPressBuffer < 0)
            {
                buttonPressBuffer = 0;
            }
        }
        if (fishInVortex)
        {
            fishAngle += secondsForFullCircle * Time.deltaTime;

            fish.transform.localPosition = new Vector3(Mathf.Cos(fishAngle)*circleRadius+beforeVortexX,Mathf.Lerp(fish.transform.localPosition.y,aimedHeight+3,Time.deltaTime),Mathf.Sin(fishAngle)*circleRadius+beforeVortexZ);
            targetDir = fishTargetGameObject.transform.position - fish.transform.position;
            fish.transform.rotation = Quaternion.RotateTowards(fish.transform.rotation, Quaternion.LookRotation(targetDir), 2);
            if (aimedHeight - fish.transform.localPosition.y < 0.2f)
            {
                StartCoroutine(LoadAndShoot());
                fishInVortex = false;
                fishLoaded = true;
            }
        }
        if (fishTargetGameObject != null)
        {
            float rotatingStep = fishRotationSpeed * Time.deltaTime;
            targetDir = fishTargetGameObject.transform.position - fish.transform.position;
            Debug.DrawRay(fish.transform.position, fish.transform.rotation.eulerAngles, Color.cyan);
            Debug.DrawRay(fish.transform.position, fish.transform.forward, Color.yellow);
            Debug.DrawRay(fish.transform.position, targetDir, Color.red);
            Debug.DrawRay(fish.transform.position, Quaternion.LookRotation(targetDir).eulerAngles, Color.white);
            if(targetDir.magnitude>0.2f)
                fish.transform.rotation = Quaternion.RotateTowards(fish.transform.rotation, Quaternion.LookRotation(targetDir), rotatingStep);
        }
		if(fishMoving)
		{
			float step = fishSpeed * Time.deltaTime;
			fish.transform.position = Vector3.MoveTowards(fish.transform.position,fishTargetGameObject.transform.position,step); // move toward target until it's reached.
			float distance = Vector3.Distance (fish.transform.position, fishTargetGameObject.transform.position);
			if(distance<0.15f)
			{
				fish.transform.position = fishTargetGameObject.transform.position;
				fishMoving = false;
				fishCoordinateX = fishTargetX;
				fishCoordinateZ = fishTargetZ;
			}
        }
        if (fishLoaded)
        {
            fish.SetActive(false);
        }
        if(vortexAnimOn)
        {
            inactive = true;
            float vortexStepX = (largeVectorLocalScale.x - getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.x) / framesForVortexAnimation;
			float vortexStepY = (largeVectorLocalScale.y - getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.y) / framesForVortexAnimation; 
			float vortexStepZ = (largeVectorLocalScale.z - getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.z) / framesForVortexAnimation;
			            
			if(getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.magnitude < largeVectorLocalScale.magnitude && !vortexGrowAnimDone)
            {
				getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale + new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale + new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale + new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale + new Vector3(vortexStepX, vortexStepY, vortexStepZ);

				//yield return new WaitForSeconds(2f); set vortexgroanimdone true here instead when you do it.
            }           
			if(largeVectorLocalScale.magnitude -getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.magnitude < 0.002f && !vortexGrowAnimDone)
            {
				//BUG: A CLEAR VISUAL SNAP IN THE SIZE CHANGE
                getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale = largeVectorLocalScale;
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale = largeVectorLocalScale;
				getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale = largeVectorLocalScale;
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale = largeVectorLocalScale;
				vortexGrowAnimDone = true;
            }
			if (getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.magnitude >= originalVortexScale.magnitude && vortexGrowAnimDone)
            {
				getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale - new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale - new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale - new Vector3(vortexStepX, vortexStepY, vortexStepZ);
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale = getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale - new Vector3(vortexStepX, vortexStepY, vortexStepZ);
            }            
			if (getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.magnitude - originalVortexScale.magnitude < 0.002f && vortexGrowAnimDone)
            {
				//BUG: A CLEAR VISUAL SNAP IN THE SIZE CHANGE
                getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale = originalVortexScale;
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ).transform.GetChild(0).transform.localScale = originalVortexScale;
				getGoFromCoordinate(vortexAnimX, vortexAnimZ+1).transform.GetChild(0).transform.localScale = originalVortexScale;
				getGoFromCoordinate(vortexAnimX+1, vortexAnimZ+1).transform.GetChild(0).transform.localScale = originalVortexScale;
				vortexGrowAnimDone = false;
            }    
            
            
            
            if(getGoFromCoordinate(vortexAnimX, vortexAnimZ).transform.GetChild(0).transform.localScale.magnitude == originalVortexScale.magnitude)
            {
                vortexAnimOn = false;
                inactive = false;
            }                
        }
	}

    public void FixedUpdate()
    {
        if(explodeFish)
        {
            fish.transform.position = itemSpawnPos.transform.position;
            fish.GetComponent<Rigidbody>().AddForce(fish.transform.forward * 10f,ForceMode.Impulse);
            fish.GetComponent<Rigidbody>().AddForce(fish.transform.up * 10f,ForceMode.Impulse);
            fishTargetGameObject = null;
            fishMoving = false;
        }
        explodeFish = false;        
    }
}
	
	

