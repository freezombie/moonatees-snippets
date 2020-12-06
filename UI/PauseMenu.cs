using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject Pnl_Pause;
	private AudioSource audioSource;
	private string room;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.volume = PlayerPrefs.GetFloat("Setting.Sound", 0.5F);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pnl_Pause.gameObject.SetActive (true);
		}
	}

	public void ButtonPlay () {
		audioSource.Play();
		if (SceneManager.GetActiveScene().ToString() == room) {
			Pnl_Pause.gameObject.SetActive (false);
			return;
		}
		SceneManager.LoadScene (room);
	}

	public void ButtonBack () {
		audioSource.Play();
		Pnl_Pause.gameObject.SetActive (false);
	}

	public void ButtonExit () {
		audioSource.Play();
		SceneManager.LoadScene ("MainMenu");
	}

	public void RoomToggle (BaseEventData data) {
		audioSource.Play();
		Debug.Log (data.selectedObject.name);
		room = data.selectedObject.name.Substring (4);
	}
}
