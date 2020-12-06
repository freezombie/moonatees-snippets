using UnityEngine;
using System.Collections;

public class ElectricArc
{
	GameObject electricArc;
	Transform target;
	Transform origin;

	int zigs; //= 100;
	float speed; //= 1f;
	float scale; //= 1f;

	Perlin noise;
	float oneOverZigs;

	Particle[] particles;



	public ElectricArc (Transform newOrigin, Transform newTarget, GameObject arc, int newZigs, float newSpeed, float newScale)
	{
		zigs = newZigs;
		speed = newSpeed;
		scale = newScale;
		origin = newOrigin;
		target = newTarget;
		electricArc = arc;

		oneOverZigs = 1f / (float)zigs;
		if (arc != null) 
        {
			arc.transform.parent = origin;
			arc.transform.localPosition = new Vector3 (0, 0, 0);
			arc.GetComponent<ParticleEmitter> ().emit = false;
			arc.GetComponent<ParticleEmitter> ().Emit (zigs);
			particles = arc.GetComponent<ParticleEmitter> ().particles;
		}
	}

	public Particle[] GetParticles ()
	{
		return particles;
	}

	public float GetSpeed ()
	{
		return speed;
	}

	public Transform GetTarget ()
	{
		return target;
	}

	public Transform GetOrigin ()
	{
		return origin;
	}

	public float GetOneOverZigs ()
	{
		return oneOverZigs;
	}

	public Perlin GetNoise ()
	{
		return noise;
	}

	public void SetNoise (Perlin newNoise)
	{
		noise = newNoise;
	}

	public float GetScale ()
	{
		return scale;
	}

	public void SetParticlePosition (int i, Vector3 position)
	{
		particles[i].position = position;
	}

	public void SetParticleColor (int i, Color color)
	{
		particles[i].color = color;
	}

	public void SetParticleEnergy (int i, float energy)
	{
		particles[i].energy = energy;
	}

	public void UpdateParticles ()
	{
		electricArc.GetComponent<ParticleEmitter>().particles = particles;
	}

	public void Destroy ()
	{
		GameObject.Destroy(electricArc);
	}
}
