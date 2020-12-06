using UnityEngine;
using System.Collections;

public class PlayStartMusic : MonoBehaviour
{



    IEnumerator Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        yield return new WaitForSeconds(1f);
        audio.Play();


    }


}