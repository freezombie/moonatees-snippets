using UnityEngine;
using System.Collections;
using System;

public class WaterBlendAnimation : MonoBehaviour {
//Using C#
 

    public int blendShapeCount = 10;
    public SkinnedMeshRenderer[] skinnedMeshRenderer;
    public Mesh skinnedMesh;
    float blendOne = 0f;
    float blendTwo = 0f;
    float blendSpeed = 1f;
    bool blendOneFinished = false;
 
  
    void Awake()
    {
        skinnedMeshRenderer = GetComponents<SkinnedMeshRenderer>();
        
    }

   

   

    void Update()
    {
        if (blendShapeCount > 2)
        {

            if (blendOne < 100f)
            {
                skinnedMeshRenderer[blendShapeCount + 1].SetBlendShapeWeight(0, blendOne);
                blendOne += blendSpeed;
            }
            else {
                blendOneFinished = true;
            }

            if (blendOneFinished == true && blendTwo < 100f)
            {
                skinnedMeshRenderer[blendShapeCount + 1].SetBlendShapeWeight(1, blendTwo);
                blendTwo += blendSpeed;
            }

        }
    }
}


