using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Speech
{
	public string speechText;
	public bool speechShown;
	public float speechDuration;
	public float requiredDisplayTime;

	public Speech(string speechText, bool speechShown = false, float requiredDisplayTime = 0f)
	{
		this.speechText = speechText;
		this.speechShown = speechShown;
		this.requiredDisplayTime = requiredDisplayTime;
	}
}

public class SpeechBank : MonoBehaviour
{
	public Dictionary<int, Speech> speechBank = new Dictionary <int, Speech>();

	public List<SpeechBubble> allSpeechBubbles = new List<SpeechBubble> ();

	// Use this for initialization
	void Start ()
	{	
		// Day 1 speeches
		speechBank.Add (1, new Speech ("Alert!\n Incoming collision detected!", false, 15f));
		speechBank.Add (2, new Speech ("Unauthorized pilot detected.\n Initiating hijack prevention measures.", false, 15));
		speechBank.Add (3, new Speech ("Hey recruit!\n We need to wake up the captain ASAP.\n Come to the radio room and\n we’ll figure something out.", false, 15));
		speechBank.Add (4, new Speech ("It’s a regular flipper scanner.\n You have flippers, don’t you?", false, 15));
		speechBank.Add (5, new Speech ("A sure way to wake up the captain\n is to make some fresh coffee.\n If you have a better idea give it a try.\n Just keep in mind that the captain is\n a very heavy sleeper.", false, 15));
		speechBank.Add (6, new Speech ("RghGH…?\nzzzZZZzzzZZzz…", false, 15));

		speechBank.Add (7, new Speech ("Have you tried\n turning me off and on again?"));
		speechBank.Add (8, new Speech ("Holy shit!\n There's a hole in the ship!", false, 20));
		speechBank.Add (9, new Speech ("Moshi moshi", false, 5));
        speechBank.Add (10, new Speech ("*Bzzt!*\nAccess authorized\nRadio room door unlocked", false, 15));
        speechBank.Add (11, new Speech ("", false, 5));

	}

	public string getSpeech(int index)
	{
		return speechBank [index].speechText;
	}

	public float getRequiredDisplayTime(int index)
	{
		return speechBank [index].requiredDisplayTime;
	}

	public bool getSpeechShownState(int index)
	{
		return speechBank [index].speechShown;
	}

	public void setSpeechShown(int index)
	{
		speechBank [index].speechShown = true;
	}

	public void addSpeechToList(SpeechBubble sb)
	{
		allSpeechBubbles.Add (sb);	
	}

	public void setFocusedSpriteForOne(SpeechBubble sb)
	{
		foreach (SpeechBubble ssbb in allSpeechBubbles)
		{
			if (sb.getParentName() == ssbb.getParentName())
			{
				ssbb.setFocusedSprite();
				ssbb.setCanvasLayer(4);
			}
			else
			{
				ssbb.setNonFocusedSprite();
				ssbb.setCanvasLayer(3);
			}
		}
	}
}
