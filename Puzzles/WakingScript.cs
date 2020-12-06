using UnityEngine;
using System.Collections;

public class WakingScript : MonoBehaviour 
{

	// Animation stuff
	public Animator animator;
    private ItemProperties ip;

	// Audio stuff
	public AudioClip audioSnore;
	private bool snoringStarted = false;
	public AudioClip audioSneeze;
	public AudioClip audioFrustrated;
	AudioSource audioSource;
    //	public AudioClip audioWin;
    public AudioClip[] coffeeWakeUpClips;
    public AudioClip[] fishWakeUpClips;
    public AudioClip[] electrifiedWakeUpClips;
    public AudioClip[] wrongItemClips;

    public bool wakeUpDone = false;
	public ParticleSystem particleSystem;
	public InputOptionsMessage iom;
	public GameManager gameManager;
    public float delayTime;
    public enum WakeType { Coffee = 0, Fish = 1, Electricity = 2, WrongItem = 3}
    bool beingElectrified = false;    
    private bool snoringCheckGoing = false;

	IEnumerator delayedSetdayComplete(float delay)
	{
		yield return new WaitForSeconds(delay);
		gameManager.setGameState ("InEnding");
		gameManager.setDayComplete ();
	}

	void Start()
	{        
        audioSource = GetComponent<AudioSource> ();
        animator = GetComponent<Animator>();
		animator.enabled = true;
		animator.Play("Idle");
		particleSystem = this.GetComponentInChildren<ParticleSystem> ();
        ip = GetComponent<ItemProperties>();
		gameManager = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<GameManager> ();

//        StartSnoring();
    }

	void Update()
	{
		// Check when timeScale is set to 1 after player presses start and that snoring is not enabled yet
       
        if (ip.GetConductingStatus() && !beingElectrified)
        {
            beingElectrified = true;
            StartCoroutine(ElectricityWakeUp());
        }
        else if(!ip.GetConductingStatus() & beingElectrified)
        {
            beingElectrified = false;
        }
        if (audioSource.clip != audioSnore)
        {
            
        }

        if (!snoringCheckGoing && !audioSource.isPlaying)
        {
            StartCoroutine(checkIfCanSnoreAgain(5f));
        }
		/*if (gameManager.getGameState () == "InEnding")
		{
			WakeUp (WakeType.);
		}*/
	}

    IEnumerator checkIfCanSnoreAgain(float delay)
    {
        snoringCheckGoing = true;

        if (!audioSource.isPlaying && gameManager.getGameState() == "InGameplay")
        {
            audioSource.clip = audioSnore;
//            audioSource.volume = 0.1f;
            audioSource.Play();
            audioSource.loop = true;
        }
        yield return new WaitForSeconds(delay);
        snoringCheckGoing = false;

    }

	public void StartSnoring()
	{            
//        audioSource.volume = 0.1f;
        audioSource.loop = true;
        snoringStarted = false;
	}

    public void WakeUp(WakeType wt, int stage)
	{
        switch (wt)
        {
            case WakeType.Coffee:
                if (stage != 3)
                {
//                    audioSource.volume = 0.5f;
                }             
                PlayClip(coffeeWakeUpClips[stage]);                
                break;     
               
            case WakeType.Fish:
                PlayClip(fishWakeUpClips[stage]);
                break;

            case WakeType.Electricity:
                PlayClip(electrifiedWakeUpClips[stage]);
                break;

            case WakeType.WrongItem:
                int randomClip = Random.Range(0, wrongItemClips.Length);
                PlayClip(wrongItemClips[randomClip]);
                break;

            default:
                break;
        }
        // Check if the captain has already start waking up so this won't get spammed
        if (!wakeUpDone && stage == 3)
		{
			wakeUpDone = true;
			// Disable particles
			particleSystem.Stop ();
			// Audio
			audioSource.loop = false;
			audioSource.Stop ();
			// Wake up sound. Make this dependant on the kind of wakeuå he gets.
			//audioSource.clip = audioFrustrated; 
//			audioSource.volume = 0.5f;
			audioSource.PlayDelayed (0.1f);

//			gameManager.setDayComplete ();
			StartCoroutine (delayedSetdayComplete (7.7f));

			Camera.main.GetComponentInChildren<MusicPlayer> ().PlayWinMusic ();
			// Animation
			animator.enabled = true;
			animator.Play ("Wake");
		}        
    }

    void PlayClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }

    IEnumerator ElectricityWakeUp()
    {
        WakeUp(WakeType.Electricity, 0);
        yield return new WaitForSeconds(delayTime + electrifiedWakeUpClips[0].length);

        if(beingElectrified)
        {
            WakeUp(WakeType.Electricity, 1);
        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(delayTime + electrifiedWakeUpClips[1].length);
        if (beingElectrified)
        {
            WakeUp(WakeType.Electricity, 2);
        }
        else
        {
            yield break;
        }
        yield return new WaitForSeconds(delayTime + electrifiedWakeUpClips[2].length);
        if (beingElectrified)
        {
            WakeUp(WakeType.Electricity, 3);
        }
        else
        {
            yield break;
        }
    }

	public void WrongItem()
	{
		// Play false sound (grumbling) without spamming it if item is on the captain

		// Play some random captain animation after which it just continues sleeping
	}
}
