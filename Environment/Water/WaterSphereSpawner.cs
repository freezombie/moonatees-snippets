using UnityEngine;
using System.Collections;

public class WaterSphereSpawner : MonoBehaviour {

   
    
    
    
    //spawning water objects

    GameObject obj;
    public int limit;
    private int x = 0;
    float creationRange1;
    float creationRange2;
    public Texture selectedTexture;
    public float SpinStartValue;
    public float SpinEnd;
    public float CreationRange1Limit;
    public float CreationRange2Limit;
    public float heightLimit;
    public GameObject SelectedGameObject;
    private float b;
    private float c;
    private float a;


    //make limits for all these
    float DoCreationRange1Limit()
    {
        float vr1 = Random.Range(0, 10);
        Random.Range(vr1, CreationRange1Limit);
        return vr1;

    }
    float DoCreationRange2Limit()
    {
        float vr2 = Random.Range(0,10);
        Random.Range(vr2, CreationRange1Limit);
        return vr2;

    }
    float DoHeightLimit()
    {
        float hr = Random.Range(0, 10);
        Random.Range(hr, heightLimit);
        return hr;

    }


    void InstantiateSelectecObject()
    {

        while (x < limit)
        {

            float xSpin = Random.Range(SpinStartValue, SpinEnd);
            float ySpin = Random.Range(SpinStartValue, SpinEnd);
            float zSpin = Random.Range(SpinStartValue, SpinEnd);



            x++;
           
                obj = SelectedGameObject;
                a = DoCreationRange1Limit();
                b = DoCreationRange2Limit();
                c = DoHeightLimit();
            obj.transform.position = new Vector3(Random.Range(0, a), Random.Range(0, c), Random.Range(0, b));
                obj.GetComponent<Renderer>().material.mainTexture = selectedTexture;
                obj.transform.rotation = Quaternion.Euler(xSpin, ySpin, zSpin);
                Instantiate(obj);
            
            
            
            
         

        }
    }

    void Update()
    {
        InstantiateSelectecObject();
        DoCreationRange1Limit();
        DoCreationRange2Limit();
        DoHeightLimit();
    }





}
