using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CaptainsTV : MonoBehaviour 
{
    Material screenMaterial;
    public Texture screenTexture;
    bool transitioning = false;
    bool currentlyOn = false;
    Color startingColor;
    Color targetColor;
    AudioSource audioSource;
    public AudioSource staticAudioSource;
    AudioSource snapAudioSource;
    public AudioClip[] staticClips;
    public AudioClip[] channelClips;
    public AudioClip[] commercialClips;
    public float transitionSpeed;
    public int currentChannel;
    //public Texture[] channelTextures;
    bool transitionTwice = false;
    int currentStatic = 0;
    int targetVolume = 0;
    Transform soundLines;
    bool distortionGoingUp = true;
    float buffer;

	void Start () 
    {
        screenMaterial = GetComponent<Renderer>().material;
        screenTexture = screenMaterial.mainTexture;
        audioSource = GetComponentInParent<AudioSource>();
        snapAudioSource = GetComponent<AudioSource>();
        soundLines = transform.FindChild("Soundlines").transform;
	}
	
    void ToggleBoolean()
    {
        if (!currentlyOn)
        {
            currentStatic = Random.Range(0, channelClips.Length);
            staticAudioSource.clip = staticClips[currentStatic];
            staticAudioSource.Play();
        }
        GetComponent<Animator>().enabled = false;
        snapAudioSource.Play();
        Transition();       
    }

    void Transition()
    {
        if (!transitioning)
        {
            transitioning = true;
            if (currentlyOn)
            {
                screenMaterial.mainTexture = null;
                targetColor = Color.black;
                targetVolume = 0;
                soundLines.gameObject.SetActive(false);
            }
            else
            {
                targetColor = Color.white;
                targetVolume = 1;
                soundLines.gameObject.SetActive(true);
                /*if (currentChannel != 0 && currentChannel != 1)
                {                   
                    //Debug.Log("Triggering this");
                    GetComponent<Animator>().enabled = false;
                    screenTexture = channelTextures[currentChannel];
                }*/     
                //Debug.Log("Triggering that");
                GetComponent<Animator>().enabled = true;
                switch (currentChannel)
                {
                    case 0:
                        GetComponent<Animator>().Play("CaptainsTVCommercial", 0, 0); // animaatio jutut
                        break;
                    case 1:
                        GetComponent<Animator>().Play("GrannyTV", 0, 0);
                        break;
                    case 2:
                        GetComponent<Animator>().Play("Bellamoon", 0, 0);
                        break;
                    default:
                        break;
                }
                if (audioSource.clip != channelClips[currentChannel] && currentChannel != 0)
                {
                    audioSource.clip = channelClips[currentChannel];
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    void Next()
    {
        if (currentlyOn && !transitioning)
        {
            snapAudioSource.Play();
            transitionTwice = true;
            currentStatic = Random.Range(0, channelClips.Length);
            staticAudioSource.clip = staticClips[currentStatic];
            staticAudioSource.Play();
            audioSource.Stop();
            if (currentChannel == 2)
                currentChannel = 0;
            else
                currentChannel++;
            Transition();
        }
    }

    void Previous()
    {
        if (currentlyOn && !transitioning)
        {
            snapAudioSource.Play();
            transitionTwice = true;
            currentStatic = Random.Range(0, channelClips.Length);
            staticAudioSource.clip = staticClips[currentStatic];
            staticAudioSource.Play();
            audioSource.Stop();
            if (currentChannel == 0)
                currentChannel = 2;
            else
                currentChannel--;
            Transition();
        }
    }

	void Update () 
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Previous();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleBoolean();
        }
        #endif
        screenMaterial.SetFloat("_dotWeight", Random.Range(1.5f,2f)); //Tämä muihin screeneihin myös jos haluaa.
        buffer += Time.deltaTime;
        if (distortionGoingUp)
        {
            screenMaterial.SetFloat("_Distortion", Mathf.Lerp(0f, 0.125f, buffer));
            if (buffer >= 1)
            {
                distortionGoingUp = false;
                buffer = 0;
            }
        }
        else
        {
            screenMaterial.SetFloat("_Distortion", Mathf.Lerp(0.125f,0f,buffer));
            if (buffer >= 1)
            {
                distortionGoingUp = true;
                buffer = 0;
            }
        }
        if (screenMaterial.mainTexture != screenTexture)
        {
            screenMaterial.mainTexture = screenTexture;
        }
        if (transitioning)
        {                        
            screenMaterial.color = Color.Lerp(screenMaterial.color, targetColor, transitionSpeed * Time.deltaTime);
            audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, transitionSpeed * Time.deltaTime);
            float staticTargetVolume = targetVolume * 0.02f;
            staticAudioSource.volume = Mathf.Lerp(staticAudioSource.volume,staticTargetVolume, transitionSpeed * Time.deltaTime);
            if (Mathf.RoundToInt(screenMaterial.color.r * 10) == Mathf.RoundToInt(targetColor.r * 10)
                && Mathf.RoundToInt(screenMaterial.color.g * 10) == Mathf.RoundToInt(targetColor.g * 10)
                && Mathf.RoundToInt(screenMaterial.color.b * 10) == Mathf.RoundToInt(targetColor.b * 10))
            {
                if (currentlyOn)
                {
                    snapAudioSource.Stop();
                    staticAudioSource.Stop();
                    audioSource.Stop();
                    currentlyOn = false;
                }
                else
                {
                    currentlyOn = true;
                }
                transitioning = false;
                if (transitionTwice)
                {
                    Transition();
                    transitionTwice = false;
                }
            }
        }
	}

    public void TriggerAudio(int whichOne)
    {
        if (audioSource.loop == true)
        {
            audioSource.loop = false;
        }
        switch (whichOne)
        {
            case 0:
                audioSource.clip = commercialClips[0];
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = commercialClips[1];
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = commercialClips[2];
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = commercialClips[3];
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = commercialClips[4];
                audioSource.Play();
                break;
            default:
                break;
        }
    }
}
