using UnityEngine;
using System.Collections;

public class CoffeeMaker : MonoBehaviour 
{
    public GameObject coffeeJug;
    public HingeJoint holderHinge; //0 - -90.
    public ReduceLiquid reduceLiquid;
    bool hasWater = false;
    bool hasCoffeeGrind = false;
    bool holderInPos = true;
    bool canInsertCup = false; // whether or not player can insert cup into the holder.
    public GameObject boardOfInstructions;   

	void ToggleBoolean()
    {
        if(hasWater && hasCoffeeGrind && holderInPos)
        {
            coffeeJug.GetComponent<CoffeeJug>().StartSteam();
            reduceLiquid.Reduce();
            boardOfInstructions.GetComponent<BoardOfInstructions>().SetStateBoolean(true, 3);
            boardOfInstructions.GetComponent<BoardOfInstructions>().ChangeTexture(4);
        }
    }

    void FixedUpdate()
    {
        if (holderHinge.angle < -15f)
        {
            holderInPos = false;
        }
        else
        {
            holderInPos = true;
        }
        if (holderHinge.angle < -55f)
        {
            canInsertCup = true;
        }
        else
        {
            canInsertCup = false;
        }
    }

    public bool GetCanInsertCup()
    {
        return canInsertCup;
    }

    public void ChangeHasCoffeeGrind(bool status)
    {
        hasCoffeeGrind = status;
    }

    public void ChangeHasWater(bool status)
    {
        hasWater = status;
    }
}
