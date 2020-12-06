using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefMovement : MonoBehaviour
{
    public bool goingForward = true;
    private float movingSpeed = 0.7f;
	
	// Update is called once per frame
	void Update ()
    {
        if (goingForward)
        {
            if (this.transform.localPosition.z < 2.5f)
            {
                transform.Translate(Vector3.left * Time.deltaTime * movingSpeed);
            }
            else
            {
                goingForward = false;
                this.transform.localScale = new Vector3 (1, -1f, 1);
            }
        }
        else
        {
            if (this.transform.localPosition.z > -9)
            {
                transform.Translate(Vector3.right * Time.deltaTime * movingSpeed);
            }
            else
            {
                goingForward = true;
                this.transform.localScale = new Vector3 (1, 1f, 1);
            }

        }
	}
}
