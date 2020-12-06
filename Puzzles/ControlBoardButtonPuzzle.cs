using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoardButtonPuzzle : MonoBehaviour 
{
	public Material green;
	public Material red;
	GameObject button01;
	GameObject button02;
	GameObject button03;
	GameObject button04;
	bool[] buttonStatus = {true,true,false,false};
    private bool puzzleSolved = false;

    public GameObject secretDoor;

	// Use this for initialization
	void Start () 
	{
		button01 = transform.FindChild ("Button01").gameObject;
		button02 = transform.FindChild ("Button02").gameObject;
		button03 = transform.FindChild ("Button03").gameObject;
		button04 = transform.FindChild ("Button04").gameObject;
	}

    public void checkGameStatus()
    {
      if (buttonStatus[0] == true && buttonStatus[1] == true && buttonStatus[2] == true && buttonStatus[3] == true && !puzzleSolved)
        {
            BoxCollider temp = secretDoor.GetComponent<BoxCollider>();
            temp.enabled = false;
            puzzleSolved = true;
        }
    }

	public void ChangeButton01()
	{

        if (!puzzleSolved)
        {

            if (buttonStatus[0])
            {
                buttonStatus[0] = false;
                button01.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[0] = true;
                button01.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[1])
            {
                buttonStatus[1] = false;
                button02.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[1] = true;
                button02.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[3])
            {
                buttonStatus[3] = false;
                button04.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[3] = true;
                button04.GetComponent<Renderer>().material = green;
            }

            checkGameStatus();
        }
	}

	public void ChangeButton02()
	{
        if (!puzzleSolved)
        {

            if (buttonStatus[1])
            {
                buttonStatus[1] = false;
                button02.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[1] = true;
                button02.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[0])
            {
                buttonStatus[0] = false;
                button01.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[0] = true;
                button01.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[2])
            {
                buttonStatus[2] = false;
                button03.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[2] = true;
                button03.GetComponent<Renderer>().material = green;
            }

            checkGameStatus();
        }
	}

	public void ChangeButton03()
	{       
        if (!puzzleSolved)
        {
            if (buttonStatus[2])
            {
                buttonStatus[2] = false;
                button03.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[2] = true;
                button03.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[1])
            {
                buttonStatus[1] = false;
                button02.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[1] = true;
                button02.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[3])
            {
                buttonStatus[3] = false;
                button04.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[3] = true;
                button04.GetComponent<Renderer>().material = green;
            }

            checkGameStatus();
        }
	}

	public void ChangeButton04()
	{
        if (!puzzleSolved)
        {
            if (buttonStatus[3])
            {
                buttonStatus[3] = false;
                button04.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[3] = true;
                button04.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[0])
            {
                buttonStatus[0] = false;
                button01.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[0] = true;
                button01.GetComponent<Renderer>().material = green;
            }
            if (buttonStatus[2])
            {
                buttonStatus[2] = false;
                button03.GetComponent<Renderer>().material = red;
            }
            else
            {
                buttonStatus[2] = true;
                button03.GetComponent<Renderer>().material = green;
            }
            checkGameStatus();
        }

	}
}
