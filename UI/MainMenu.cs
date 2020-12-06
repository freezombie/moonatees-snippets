using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	
	public GameObject Pnl_Main;
	public GameObject Pnl_Load;
	public GameObject Pnl_Settings;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.volume = PlayerPrefs.GetFloat("Setting.Sound", 0.5F);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonStart () {
		audioSource.Play();
		SceneManager.LoadScene ("PauseMenu");
	}

	public void ButtonLoad () {
		audioSource.Play();
		Pnl_Main.gameObject.SetActive (false);
		Pnl_Load.gameObject.SetActive (true);
	}

	public void ButtonSettings () {
		audioSource.Play();
		Pnl_Main.gameObject.SetActive (false);
		Pnl_Settings.gameObject.SetActive (true);
	}

	public void ButtonBack () {
		audioSource.volume = PlayerPrefs.GetFloat("Setting.Sound", 0.5F);
		audioSource.Play();
		Pnl_Main.gameObject.SetActive (true);
		Pnl_Load.gameObject.SetActive (false);
		Pnl_Settings.gameObject.SetActive (false);
	}

	public void DetailLevel (BaseEventData data) {
		audioSource.Play();
		Debug.Log (data.selectedObject.name);
	}
}
