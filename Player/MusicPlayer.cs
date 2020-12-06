using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
	private AudioSource myAudioSource;
	public AudioClip audioMenu;
	public AudioClip audioBackground;
	public AudioClip audioWin;

	void Start ()
	{
//		AudioListener.volume = 0f;

//		StartCoroutine (delayAudio());

		myAudioSource = GetComponent<AudioSource> ();
//
//		if (audioBackground == null) {
//			Debug.Log ("You are missing background music from player camera's MusicPlayer script!");
//		}

		if (audioWin == null) {
			Debug.Log ("You are missing winning music from player camera's MusicPlayer script!");
		}

		if (audioMenu == null) {
			Debug.Log ("You are missing menu music from player camera's MusicPlayer script!");
		}

//		Debug.Log (GetComponent<AudioListener> ().enabled);
//		GetComponent<AudioListener> ().enabled = true;


		PlayMenuMusic ();
	}

	// This is called from InputOptionsMessage in the begining of the game
	public void PlayMenuMusic()
	{
//		AudioListener.volume = 1f;

		myAudioSource.clip = audioMenu;
		myAudioSource.volume = 0.2f;
		myAudioSource.loop = true;
		myAudioSource.Play ();
	}

	// This is called from InputOptionsMessage when Start is pressed
	public void PlayBackgroundMusic()
	{
		if (myAudioSource.clip != audioBackground)
		{
			myAudioSource.clip = audioBackground;
			myAudioSource.volume = 0.2f;
			myAudioSource.loop = true;
			myAudioSource.Play ();
		}
	}

	public void PlayWinMusic()
	{
//		Debug.Log ("Beep, playing win music");
		myAudioSource.Stop ();
		myAudioSource.clip = audioWin;
		myAudioSource.volume = 0.2f;
		myAudioSource.PlayDelayed (1);
	}

    public void StopMusic()
    {
        myAudioSource.Stop();
    }
}
