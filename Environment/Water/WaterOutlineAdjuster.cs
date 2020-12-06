using UnityEngine;
//using UnityEditor;
using System.Collections;
using System;

public class WaterOutlineAdjuster : MonoBehaviour {
    public float distance;
    public GameObject Player;
    public GameObject WaterPrafab;
    Renderer rend;
    private float lenght = 0.001f;
    private float changed;
    private bool frame;

   


  
    
void Start () {

       

        rend = GetComponent<Renderer>();

        Material material = new Material(Shader.Find("Toon/ToonWater"));
        
      
       }

    void change(float changed)
    {

        rend.material.SetFloat("_Outline", changed);
        
    }


   
    void Update () {


        if (Player == null)
        {

           

            Player = GameObject.FindGameObjectWithTag("Player");

        }
       else {
            distance = Vector3.Distance(WaterPrafab.transform.position, Player.transform.position);

            lenght = 0.25f * 1 / distance;

           
            change(lenght);
        }

       
      
      
    }
}
