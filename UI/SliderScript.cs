using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {

	private AudioSource audioSource;
	private Slider slider;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		slider = transform.GetChild (1).GetComponent<Slider> ();

		// load setting
		slider.value = PlayerPrefs.GetFloat("Setting." + transform.name.Substring(4), 0.5F);
		audioSource.volume = slider.value;
		audioSource.enabled = true;
	}

	// Update is called once per frame
	void Update () {

	}

	public void ValueChange () {
		PlayerPrefs.SetFloat("Setting." + transform.name.Substring(4), slider.value);
		audioSource.volume = slider.value;
		audioSource.Play();

		int value = (int) (slider.value * 100);
		transform.GetChild (2).GetComponent<Text> ().text = value.ToString ();
	}
}
