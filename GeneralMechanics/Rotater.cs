using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour 
{
    public enum RotationDirection {Forward,Back,Right,Left,Up,Down}
    public float speed;
    public bool status;
    public RotationDirection dir;
    public bool continuous;
    public float stepAmount;
	private Vector3 to = new Vector3();


	void Start () 
    {
		stepAmount-=1;
	}
	
	void Update () 
    {
        if(status)
        {
			if(GetComponent("Rigidbody")==null)
			{
	            if (continuous)
	            {
	                switch(dir)
	                {
	                    case RotationDirection.Forward:
	                        transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);
	                        break;
	                    case RotationDirection.Back:
	                        transform.Rotate(Vector3.back * speed * Time.deltaTime, Space.Self);
	                        break;
	                    case RotationDirection.Right:
	                        transform.Rotate(Vector3.right * speed * Time.deltaTime, Space.Self);
	                        break;
	                    case RotationDirection.Left:
	                        transform.Rotate(Vector3.left * speed * Time.deltaTime, Space.Self);
	                        break;
	                    case RotationDirection.Up:
	                        transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.Self);
	                        break;
	                    case RotationDirection.Down:
	                        transform.Rotate(Vector3.down * speed * Time.deltaTime, Space.Self);
	                        break;
	                }
	            }
	            else // if the movement is not continuous but step-by-step            
	            {
					Debug.Log(status);
	                if (status)
	                {	                  
						Debug.Log("to.z: " +to.z+ " localEulerAngles.z: " + transform.localEulerAngles.z);
	                    if (Vector3.Distance(transform.localEulerAngles, to) > 0.2f)
	                    {
	                        transform.localEulerAngles = Vector3.Lerp(transform.localRotation.eulerAngles, to, Time.deltaTime*speed);
	                    }
	                    else // if the distance is small enough, just change the value into the value that we are aiming for
	                    {
	                        transform.localEulerAngles = to;
	                        status = false;
	                    }
	                }
	             }
			}
			else // If there is a rigidbody attached
			{
				return; //This is here just to remind to do these things in FIXEDUPDATE!
			}
        }     
    }

	void FixedUpdate()
	{
		if (status)
		{
			if(GetComponent("RigidBody")!=null)
			{
				if(continuous)
				{

				}
				else //if not continuous but step-by-step
				{

				}
			}
		}
	}

    void ToggleBoolean()
    {
        if(status)
        {
			if(continuous)
			{
				status = false;
			}
			else
			{
				return; // if the movement is step-by-step and the turning hasn't finished, do nothing
			}
        }
        if(!status)
        {
			if(!continuous)
			{
				switch (dir)
				{
				case RotationDirection.Forward:
					to.z = transform.localEulerAngles.z + Vector3.forward.z + stepAmount;
					break;
				case RotationDirection.Back:
					to.z = transform.localEulerAngles.z + (Vector3.back.z - stepAmount);
					break;
				case RotationDirection.Right:
					to.x = transform.localEulerAngles.x + Vector3.right.x + stepAmount;
					break;
				case RotationDirection.Left:
					to.x = transform.localEulerAngles.x + (Vector3.left.x - stepAmount);
					break;
				case RotationDirection.Up:
					to.y = transform.localEulerAngles.y + Vector3.up.y + stepAmount;
					break;
				case RotationDirection.Down:
					to.y = transform.localEulerAngles.y + (Vector3.down.y - stepAmount);
					break;
				}
			}	
			status = true;
        }
    }
}
