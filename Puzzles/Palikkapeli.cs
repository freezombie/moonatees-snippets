using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palikkapeli : MonoBehaviour
{
    public bool puzzleSolved = false;
    public bool squarePalikkaInside = false;
    public bool rectanglePalikkaInside = false;
    public bool circlePalikkaInside = false;
    public bool trianglePalikkaInside = false;

    public Rigidbody myRigidbody;

    public PalikkaWatch pw;

    public bool allPalikatReadyToBeChecked = false;
    public bool palikkaCheckGoing = false;
    AudioSource audioSource;
    int counter=0;
    public AudioClip[] audioClips;
    bool increasedOnce1 = false;
    bool increasedOnce2 = false;
    bool increasedOnce3 = false;
    bool increasedOnce4 = false;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody>();
        pw = this.GetComponentInChildren<PalikkaWatch>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool getPuzzleState()
    {
        return puzzleSolved;
    }

    public void disableKinematics()
    {
        myRigidbody.isKinematic = false;
    }

    void Update()
    {
        if (allPalikatReadyToBeChecked && !palikkaCheckGoing)
        {
            palikkaCheckGoing = true;
            StartCoroutine(palikkaCheck(1f));
        }
    }
      
    public void OnTriggerEnter(Collider col)
    {       
        if (col.gameObject.CompareTag("SquarePalikka"))
        {
            squarePalikkaInside = true;
            if (!increasedOnce1)
            {
                PlaySound();
                counter++;
                increasedOnce1 = true;
            }
        }            
        if (col.gameObject.CompareTag("TrianglePalikka"))
        {
            trianglePalikkaInside = true;
            if (!increasedOnce2)
            {
                PlaySound();
                counter++;
                increasedOnce2 = true;
            }

        }
        if (col.gameObject.CompareTag("CirclePalikka"))
        {
            circlePalikkaInside = true;
            if (!increasedOnce3)
            {
                PlaySound();
                counter++;
                increasedOnce3 = true;
            }
        }
        if (col.gameObject.CompareTag("RectanglePalikka"))
        {
            rectanglePalikkaInside = true;
            if (!increasedOnce4)
            {
                PlaySound();
                counter++;
                increasedOnce4 = true;
            }
        }
        if (squarePalikkaInside && rectanglePalikkaInside && circlePalikkaInside && trianglePalikkaInside)
        {
            allPalikatReadyToBeChecked = true;
        }
    }

    void PlaySound()
    {
        audioSource.clip = audioClips[counter];
        audioSource.Play();
    }

    IEnumerator palikkaCheck(float delay)
    {
        bool result = pw.checkIfEachPalikkaHasARigidbody(); // So that they are not held

        if (result)
        {
            StartCoroutine(puzzleCompleted(1f));
        }
         
        yield return new WaitForSeconds (delay);
        palikkaCheckGoing = false;
    }

    IEnumerator puzzleCompleted(float delay)
    {
        yield return new WaitForSeconds (delay);

        puzzleSolved = true;
        pw.disableChecking();
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("SquarePalikka"))
        {
            squarePalikkaInside = false;
            allPalikatReadyToBeChecked = false;
        }
        if (col.gameObject.CompareTag("TrianglePalikka"))
        {
            trianglePalikkaInside = false;
            allPalikatReadyToBeChecked = false;
        }
        if (col.gameObject.CompareTag("CirclePalikka"))
        {
            circlePalikkaInside = false;
            allPalikatReadyToBeChecked = false;
        }
        if (col.gameObject.CompareTag("RectanglePalikka"))
        {
            rectanglePalikkaInside = false;
            allPalikatReadyToBeChecked = false;
        }
    }
}
