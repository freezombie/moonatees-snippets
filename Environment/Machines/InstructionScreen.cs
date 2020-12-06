using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionScreen : MonoBehaviour 
{
    Material screenMaterial;
    public Texture screenTexture;
    public enum Direction {right,up,down,left,forward}
    public Direction dir;
    Animator animator;

	void Start () 
    {
        animator = GetComponent<Animator>();
        screenMaterial = GetComponent<Renderer>().material;
        screenTexture = screenMaterial.mainTexture;
        switch (dir)
        {
            case Direction.right:
                animator.Play("Right");
                break;
            case Direction.up:
                animator.Play("Up");
                break;
            case Direction.down:
                animator.Play("Down");
                break;
            case Direction.left:
                animator.Play("Left");
                break;
            case Direction.forward:
                animator.Play("Forward");
                break;
            default:
                break;
        }
	}
	
	void Update () 
    {
        if (screenMaterial.mainTexture != screenTexture)
        {
            screenMaterial.mainTexture = screenTexture;
        }
	}
}
