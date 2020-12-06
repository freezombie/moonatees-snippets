using UnityEngine;
using System.Collections;

public class WaterShaderMod : MonoBehaviour
{

    float z = 0.05f;
    float x = 0.05f;
    float y = 0.05f;

    public float lenght = 0.05f;
    float lenghtMin = 0.05f;
    public Shader shader;
    private Material material;
    public Renderer rend;
    public float time;
    private float increment = 0.005f;
    float interval = 1f;
    bool hit = false;
    bool animationStop = false;
    bool isIncreasing = false;
    bool hitAgain = false;


    float lenghtMax = 0.1f;
    bool isAnimationPlaying = false;

   

    void Start()
    {

        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Toon/ToonWater");

    }

    void changing1()
    {



        if(hit == true)
        {

            isIncreasing = true;
            isAnimationPlaying = true;
        }


       
            

        
        if (isIncreasing == true)
        {

             
            if (isAnimationPlaying)
            {


                time += Time.deltaTime;
                if (time > interval)
                {


                    change(lenght);
                    lenght += increment;
                    time = 0;

                }


            }
        }

        else
        {
            if (isIncreasing == false)
            {


                if (isAnimationPlaying)
                {


                    time += Time.deltaTime;
                    if (time > interval)
                    {


                        change(lenght);
                        lenght -= increment;
                        time = 0;

                    }


                }
            
            

            }
        }

        if (lenght >= lenghtMax)
        {
            lenght -= increment;
            isIncreasing = false;
         
        }
        if (lenght < lenghtMin)
        {
            lenght = lenghtMin;
        }
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.collider.tag == "PlayerBody")
        {
            if (hit == false)
            {

               hit = true;
                            

            }
            else
            {

                isIncreasing = true;

            }

        }
    }

    void change(float changed)
    {

        rend.material.SetFloat("_ZLength", changed);
        rend.material.SetFloat("_YLength", changed);
        rend.material.SetFloat("_XLength", changed);
    }

   void OnCollisionExit(Collision col)
    {
        if (col.collider.tag == "PlayerBody")
        {
            hit = false;
                                  
        }
    }

   
    void Update()
    {


        changing1();
        
        
    }
}







