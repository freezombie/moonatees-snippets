using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jukebox : MonoBehaviour 
{
    public Transform snapPos; // get these in editor as jukebox's child
    public Transform finalPos;
    Transform soundLines;
    public float CDInsertSpeed;
    float targetVolume;
    AudioSource jukeboxAS;
    public List<AudioClip> jukeboxClips = new List<AudioClip>();
    int currentIndex = 0;
    GameObject CD;
    bool cdInserted = false;
    bool fade = false;
    bool continueToPlay = false;
    bool doneFading = false;
    bool snapped = false;
    bool disabled = false;
    public AnimationClip CDChange;
    Animator animator;
    JukeboxTriggerArea jbta;

    void Start()
    {
        jbta = GetComponentInChildren<JukeboxTriggerArea>();
        animator = GetComponent<Animator>();
        jukeboxAS = GetComponent<AudioSource>();
        soundLines = transform.FindChild("Soundlines").transform;
    }

    public void SetCD(GameObject CD)
    {
        this.CD = CD;
        Destroy(this.CD.GetComponent<NewtonVR.NVRInteractableItem>());
        Destroy(this.CD.GetComponent<Rigidbody>());
        Destroy(this.CD.GetComponent<BoxCollider>());
        CD.transform.rotation = Quaternion.Euler(new Vector3(0f,90f,-90f));
        cdInserted = true;
        jbta.SetDisabled(true);
    }

    void Stop()
    {
        fade = true;
        targetVolume = 0;
        disabled = true;
        soundLines.gameObject.SetActive(false);
    }

    void Previous()
    {
        if (!disabled)
        {
            Stop();
            continueToPlay = true;
//            Debug.Log("Currentindex: " + currentIndex);
            if (currentIndex > 0)
                currentIndex--;
            else
                currentIndex = jukeboxClips.Count - 1;
//            Debug.Log("Currentindex: " + currentIndex);
        }
    }

    void Next()
    {
        if (!disabled)
        {
            Stop();
            continueToPlay = true;
//            Debug.Log("Currentindex: " + currentIndex);
            if (currentIndex < jukeboxClips.Count-1)
                currentIndex++;
            else
                currentIndex = 0;
//            Debug.Log("Currentindex: " + currentIndex);
        }
    }

    void Play()
    {
//        Debug.Log("Received play");
        fade = true;//FADE IN
        targetVolume = 1;
//        Debug.Log("Currentindex: " + currentIndex);
        jukeboxAS.clip = jukeboxClips[currentIndex];
        jukeboxAS.Play();
        animator.Play("MusicOn");
        soundLines.gameObject.SetActive(true);
        disabled = false;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.V))
        {
            Previous();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Stop();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Next();
        }*/
        if (fade)
        {
            jukeboxAS.volume = Mathf.Lerp(jukeboxAS.volume, targetVolume, 2 * Time.deltaTime);

            if (Mathf.Abs(jukeboxAS.volume - targetVolume) < 0.1f)
            {
                fade = false;
                if (continueToPlay)
                {
                    animator.Play("CDchange");
                    StartCoroutine(WaitAndPlay(CDChange.length));
                    continueToPlay = false;
                }
                else if (targetVolume == 0)
                {
                    jukeboxAS.Stop();
                    animator.Play("Idle");
                }
            }
        }
        if (cdInserted)
        {
            if (!snapped)
            {
                CD.transform.position = snapPos.transform.position;
                snapped = true;
            }
            else
            {
                CD.transform.position = Vector3.MoveTowards(CD.transform.position, finalPos.position, CDInsertSpeed * Time.deltaTime);
                //Debug.Log("X:" + Mathf.Abs(CD.transform.position.x - finalPos.position.x) + " Y:" + Mathf.Abs(CD.transform.position.y - finalPos.position.y) + " Z: " + Mathf.Abs(CD.transform.position.z - finalPos.position.z));
                if (Mathf.Abs(CD.transform.position.x - finalPos.position.x) < 0.005f)
                {
                    jbta.SetDisabled(false);
                    snapped = false;
                    cdInserted = false;
                    jukeboxClips.Add(CD.GetComponent<CD>().GetContent());
                    currentIndex = jukeboxClips.Count - 1;
                    Destroy(CD);
                    animator.Play("CDchange");
                    StartCoroutine(WaitAndPlay(CDChange.length));
                }
            }
         }
     }

    // TEE PLAY ANIMAATIO!

    IEnumerator WaitAndPlay(float time)
    {
        yield return new WaitForSeconds(time);

        disabled = false;
        Play();
        animator.Play("MusicOn");
        soundLines.gameObject.SetActive(true);
    }
}
