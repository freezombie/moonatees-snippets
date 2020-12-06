using UnityEngine;
using System.Collections;

public class ReduceLiquid : MonoBehaviour
{
    public enum Dimension {X,Y,Z}
    public Dimension dimension;
    public GameObject Liquid;
    public int AmountOfLiquid;
    float oneStep;
	
	void Start ()
    {
        switch (dimension)
        {
            case Dimension.X:
                oneStep = Liquid.transform.localScale.x / AmountOfLiquid;
                break;
            case Dimension.Y:
                oneStep = Liquid.transform.localScale.y / AmountOfLiquid;
                break;
            case Dimension.Z:
                oneStep = Liquid.transform.localScale.z / AmountOfLiquid;
                break;
        }
    }
	
	
	public void Reduce()
    {
        switch (dimension)
        {
            case Dimension.X:
                Liquid.transform.localScale -= new Vector3(oneStep, 0, 0);
                Liquid.transform.localPosition -= new Vector3(oneStep / 2, 0, 0);
                break;
            case Dimension.Y:
                Liquid.transform.localScale -= new Vector3(0, oneStep, 0);
                Liquid.transform.localPosition -= new Vector3(0, oneStep, 0);
                break;
            case Dimension.Z:
                Liquid.transform.localScale -= new Vector3(0,0, oneStep);
                Liquid.transform.localPosition -= new Vector3(0, 0, oneStep / 2);
                break;
        }
        
        if(Liquid.transform.localScale.x < 0.01f || Liquid.transform.localScale.y < 0.01f || Liquid.transform.localScale.z < 0.01f)
        {
            Liquid.SetActive(false);
        }
    }
}
