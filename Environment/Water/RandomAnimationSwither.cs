using UnityEngine;
using System.Collections;

public class RandomAnimationSwither : MonoBehaviour
{


    Shader shader;

    Renderer rend;
    

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Toon/ToonWater");

    }




    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
            Debug.Log("whats there");

            float _ZLenght = Mathf.PingPong(Time.time, 5f);
            rend.material.SetFloat("__ZLength", _ZLenght);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "PlayerBody")
        {
            Debug.Log("whats there2");
            float _ZLenght = Mathf.PingPong(Time.time, 1.5f);
            rend.material.SetFloat("__ZLength", _ZLenght);

        }
    }
}

    





