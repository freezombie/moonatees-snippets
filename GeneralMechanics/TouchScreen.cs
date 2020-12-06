using UnityEngine;
using System.Collections;

public class TouchScreen : MonoBehaviour 
{
	private Renderer myRenderer;
	private int textureIndex = 0;
	private float time;
	public float cooldown = 1;
	public bool cooldownDone = true;

	// Set all textures to be cycled here
	public Texture2D[] textures;

	void Start () 
	{
		myRenderer = gameObject.GetComponent<Renderer>();

		// If there's no textures set
		if (textures [0] == null) 
		{
			Debug.Log ("No texture set for 'TouchScreen' script in game object " + this.transform.name + "!");
			Debug.Log ("Disabling this TouchScreen script");
			this.enabled = false;
		}
		else // Start by using the first texture
		{
			getNextTexture ();
		}
	}

	void Update()
	{
		if (time < cooldown)
		{
			time += Time.deltaTime;
		} else
		{
			cooldownDone = true;
		}
	}

	void changeTexture(int index)
	{
		myRenderer.material.mainTexture = textures[index];
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.name == "RightHandTrigger" || col.name == "LeftHandTrigger")
			getNextTexture ();
	}

	void getNextTexture()
	{
		if (cooldownDone) {
			textureIndex++;

			if (textureIndex > textures.Length - 1)
				textureIndex = 0;

			changeTexture (textureIndex);
			cooldownDone = false;
			time = 0;
		}
	}
}
