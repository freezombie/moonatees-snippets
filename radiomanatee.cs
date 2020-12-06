using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radiomanatee : MonoBehaviour 
{
    Animator animator;
    public float hurtAnimationThreshold;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.rigidbody != null)
        {
            Debug.Log("Found rigidbody at " + col.gameObject.name + "It struck the radio moonatee with relative velocity of " + col.relativeVelocity + " and impulse of " + col.impulse);
            if (hurtAnimationThreshold < col.relativeVelocity.sqrMagnitude) /* change this to impulse if it is better */
            {
                animator.Play("Hurt");
            }
        }
    }
}
