using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour 
{
	public enum fishState {RestLeft,RestRight,SwimLeft,SwimRight};
	fishState state = fishState.RestRight;
	fishState oldState = fishState.RestRight;

	public void changeFishState(fishState newState)
	{
		
		oldState = state;
		state = newState; 
		/*
		if(state == fishState.RestLeft)
		{
			GetComponent<Animator>().Play("RestLeft");
		}
		if(state == fishState.RestRight)
		{
			GetComponent<Animator>().Play("RestRight");
		}
		if(state == fishState.SwimLeft)
		{
			GetComponent<Animator>().Play("SwimLeft");
		}
		if(state == fishState.SwimRight)
		{
			GetComponent<Animator>().Play("SwimRight");
		}
		*/
	}

	public fishState getState()
	{
		return state;
	}

	public fishState getOldState()
	{
		return oldState;
	}
}
