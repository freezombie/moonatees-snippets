using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterSpawner : MonoBehaviour 
{
    private BoxCollider myBoxCollider;
    private ParticleSystem[] myParticleSpawners; // Optional for cpt hand washer for example
    public GameObject curtain; // Optional game object to move

    private Vector3 curtainStartPosition;
    private Vector3 curtainEndPosition;
    public bool curtainGoingDown = false;
    public bool curtainAtTarget = false;
    public bool curtainCallActive = false;

	void Start () 
    {
        myBoxCollider = GetComponent<BoxCollider>();
        myParticleSpawners = this.GetComponentsInChildren<ParticleSystem>();

        if (this.transform.FindChild("Curtain") != null)
        {
            curtain = this.transform.FindChild("Curtain").gameObject;
            curtainStartPosition = curtain.transform.position;
            curtainEndPosition = curtainStartPosition;
            curtainEndPosition.y -= 4f;
        }   
	}

    IEnumerator lowerCurtain(float duration)
    {
        yield return new WaitForSeconds (duration);
        curtainCallActive = false;
        curtainGoingDown = false;
    }

    void FixedUpdate()
    {
        if (curtain != null)
        {
            if (!curtainAtTarget)
            {
                if (curtainGoingDown)
                {
                    curtain.transform.position = Vector3.Slerp(curtain.transform.position, curtainEndPosition, 3f * Time.deltaTime);
                }
                else
                    curtain.transform.position = Vector3.Slerp(curtain.transform.position, curtainStartPosition, 3f * Time.deltaTime);
            }
        }
    }
	
    void OnTriggerEnter(Collider col)
    {
        // Play child water spawners if existing
        if (myParticleSpawners != null)
        {
            foreach (ParticleSystem ps in myParticleSpawners)
            {
                ps.Play();
            }
        }

        if (curtain != null)
        {
            if (!curtainCallActive)
            {
                StartCoroutine(lowerCurtain(4.5f));
                curtainGoingDown = true;
                curtainAtTarget = false;
                curtainCallActive = true;
            }           
        }
        else
        {
            if(GetComponent<AudioSource>()!=null && GetComponent<AudioSource>().isPlaying == false)
                GetComponent<AudioSource>().Play();
        }
        // Spawn glitter for colliding objects
        if (col.GetComponentInChildren<ParticleSystem>() == null)
        {
            GameObject glitterParticles = Instantiate(Resources.Load("GlitterParticle", typeof(GameObject)) as GameObject);
            glitterParticles.GetComponent<ParticleSystem>().Play();

            glitterParticles.transform.parent = col.transform;
            glitterParticles.transform.localPosition = Vector3.zero;
        }
        else // Play existing glitter spawner again
        {
            ParticleSystem existingGlitterSpawner = col.GetComponentInChildren<ParticleSystem>();
            existingGlitterSpawner.Play();
        }
    }
}
