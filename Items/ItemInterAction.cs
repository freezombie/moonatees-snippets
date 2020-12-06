using UnityEngine;
using System.Collections;
using System;

public class ItemInterAction : MonoBehaviour {
    
	public GameObject door;

    //script variable
	public Interactions script;
    
	//all scripts which can be chosen for interaction
	public enum Interactions {Rotater,Rotater1,Rotater2, door}


    public RotationDirection dir;

	//also parameters can be used this enum way
	public enum RotationDirection {Forward,Back,Right,Left,Up,Down}

	//kohteita joita tulee mieleen interactiosta
	//on sähkö, ovet, kahvinkeittimet
	//vesi, tuulettimet, manaattien toiminta



	void ToggleInterAction()
    {

		//interaction chunk
	
			
			switch (script)
			{
			//these together could be some named funny thing as interaction
			//and could be chosen by public enum which changes all these
		case Interactions.Rotater:
			    if (RotationDirection.Forward.Equals (true))
				gameObject.SendMessage ("RoatationDirection.Forward");	
				gameObject.SendMessage ("Speed", 10f);
				Debug.Log ("rot speed changed1 10");

				break;
				
			case Interactions.Rotater2:
				if(RotationDirection.Forward.Equals(true))
				gameObject.SendMessage ("RoatationDirection.Forward");
				gameObject.SendMessage ("Speed", 100f);
				Debug.Log ("rot speed changed1 100");
				break;

		    case Interactions.door:
			
			door.gameObject.SendMessage ("ToggleBoolean", true);


			Debug.Log ("Door Opened");
			break;



			}

		}

    
}
