using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 lastFramePosition;
    public GameObject[] activatedObjects;
    private Rigidbody rb;
    private RigidbodyConstraints rbc;
    public enum Orientation {X,Y,Z}
    public enum Direction {Minus,Plus}
    public Direction direction;
    public Orientation orientation;
	public enum ButtonType {ToggleBoolean,Custom, Error} // Add whatever you want here
	public ButtonType type;
	public string customMessage;
    public bool useCustomParameter = false;
    public int customMessageIntParameter; // this has to be at least 1
	public float buttonRecoverySpeed = 0.2f;
	bool slowedDown = false;
    bool errorSpawned = false;

	void Start () 
	{
		startPosition = this.gameObject.transform.localPosition;
        lastFramePosition = startPosition;
		rb = GetComponent<Rigidbody>();
//		if(transform.parent.name=="ButtonGame")
//			Debug.Log ("activatedobjects length: " + activatedObjects.Length); 
	}

	
	void Update () 
	{        
		switch(orientation)
        {
            case Orientation.X:
                if(direction == Direction.Minus)
                {
                    if (transform.localPosition.x > startPosition.x)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                if(direction == Direction.Plus)
                {
                    if (transform.localPosition.x < startPosition.x)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                break;
            case Orientation.Y:
                if(direction == Direction.Minus)
                {
                    if (transform.localPosition.y > startPosition.y)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                if(direction == Direction.Plus)
                {
                    if (transform.localPosition.y < startPosition.y)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                break;
            case Orientation.Z:
                if(direction == Direction.Minus)
                {
                    if (transform.localPosition.z > startPosition.z)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                if(direction == Direction.Plus)
                {
                    if (transform.localPosition.z < startPosition.z)
                    {
                        transform.localPosition = startPosition;
                    }
                }
                break;

        }           
        if (transform.localPosition != startPosition)
        {
			//if (transform.localPosition == lastFramePosition)
			if(Vector3.Distance(transform.localPosition,lastFramePosition)<buttonRecoverySpeed*Time.deltaTime)
			{				
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, buttonRecoverySpeed * Time.deltaTime);
            }
            lastFramePosition = transform.localPosition;
        }
//		if((transform.eulerAngles.y>=0 && transform.eulerAngles.y<90)
//		{
//			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;	
//		}
	}
		

	void OnCollisionEnter (Collision col)
	{		
		//Debug.Log ("collided with something tagged: " + col.gameObject.tag + " it was named: " +col.gameObject.name);
		if(col.gameObject.tag=="Static") // add a new tag called buttonplatform if it is needed.
		{
			//Debug.Log ("Collided with something that has tag Static");
			if (!slowedDown)
			{
				rb.velocity = Vector3.zero;
				rb.Sleep ();
				slowedDown = true;
			}
            bool foundObject = false;
			switch (type)
			{               
			case ButtonType.ToggleBoolean:
				//Debug.Log ("the button has a toggleboolean message");
                if (activatedObjects.Length != 0)
                {
					//Debug.Log ("activated objects wasn't empty");
                    foreach (GameObject activatedObject in activatedObjects)
                    {
                    	if (activatedObject != null)
                        {
							//Debug.Log ("We found something to send the message to");
                            foundObject = true;
                            activatedObject.SendMessage("ToggleBoolean");
                        }                                             
                    }
                }
                if (activatedObjects.Length == 0 || !foundObject)
                {
					//Debug.Log ("Activated objects didn't have anything, or the objects were null");
                    Error();
                }
    			break;
                case ButtonType.Custom:
                    //Debug.Log ("the button has a custom message. Button was: " + this.name + " collider was: " + col.gameObject.name);
                    if (activatedObjects.Length != 0)
                    {
                        foreach (GameObject activatedObject in activatedObjects)
                        {
                            if (activatedObject != null)
                            {
                                foundObject = true;
                                if (!useCustomParameter)
                                    activatedObject.SendMessage(customMessage);
                                else
                                    activatedObject.SendMessage(customMessage, customMessageIntParameter);
                            }                                             
                        }
                    }
                    if (activatedObjects.Length == 0 || !foundObject)
                    {
                        Error();
                    }
    				break;
                case ButtonType.Error:
                    Error();
                    break;
			}
		}
	}

    void Error()
    {
        if(!errorSpawned)
        {
            errorSpawned = true;
            GameObject errorSounder = Instantiate(Resources.Load("Errorsounder") as GameObject);
            errorSounder.transform.parent = this.transform;
            errorSounder.transform.localPosition = Vector3.zero;
            errorSounder.GetComponent<ErrorSounder>().SetButtonScript(this.GetComponent<ButtonScript>());
        }
    }

    public void SetErrorSpawned(bool boolean)
    {
        errorSpawned = boolean;
    }
	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Static")
		{
			slowedDown = false;
		}
	}


}
