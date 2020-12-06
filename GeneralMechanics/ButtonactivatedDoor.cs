using UnityEngine;
using System.Collections;

public class ButtonactivatedDoor : MonoBehaviour {
	public bool status = false;
	public Animator animator;
	public GameObject doorsystem;
	public float AnimationSpeed;

	public BoxCollider DoorBoxCollider;


	void Start()
	{
		DoorBoxCollider = doorsystem.GetComponentInParent<BoxCollider> ();
		
			DoorBoxCollider.enabled = false;

		animator.runtimeAnimatorController = Resources.Load("doorAnimator") as RuntimeAnimatorController;

		animator = gameObject.GetComponent<Animator>();

		animator.Play("Idle");

	}


	public void ToggleBoolean()
	{
		DoorBoxCollider.enabled = true;

			if (!status) {

				animator.speed = AnimationSpeed;
				animator.Play ("DoorClose");
				status = true;
			} else if (status) {



					animator.speed = AnimationSpeed;
					animator.Play ("DoorOpen");
					status = false;


			}

	}
}
