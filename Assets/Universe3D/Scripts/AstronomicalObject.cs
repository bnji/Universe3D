using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public abstract class AstronomicalObject : IAstronomicalObject, IPause
{
	public float InitialMass { get { return initialMass; } }

	public List<AstronomicalObject> planets;
	[Description ("if stationary, gravity won't have any affect")]
	public bool
		isStationary = false;
	[Description ("If Zero, there's no life time limit")]
	public float
		lifeSpan = 0f;
	
	protected float initialMass;
	protected float lifeStarted;
	protected bool isPaused = false;

	public void OnPauseGame ()
	{
		isPaused = true;
	}

	public void OnResumeGame (PauseGameOptions options)
	{
		isPaused = false;
	}

	// Use this for initialization
	protected void Start ()
	{
//		Debug.Log ("Astronomical Object " + name + " Start");
		if (planets == null) {
			planets = new List<AstronomicalObject> ();
		}
		initialMass = GetComponent<Rigidbody> ().mass;
		lifeStarted = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isPaused) {
			return;
		}

		if (lifeSpan != 0 && Time.time - lifeStarted > lifeSpan) {
			//StartCoroutine (PlayExplosion ());
			Destroy (gameObject);
		}
		
		if (planets == null) {
			return;
		}
		
		for (int i = 0; i < planets.Count; i++) {
			var planet = planets [i];
			if (!isStationary && planet != null) {
				//						var dist = Vector2.Distance (transform.position, planet.transform.position);
				PhysicsHelper.ApplyGravity (this, planet);
				//					Debug.Log ("planet: " + planet.name + " - time: " + planet.timeAffectedByPlanetFinger + ", dist: " + dist);
			}
		}


	}

	public void Destroy (bool removeFromGame = false, bool canUseExplosion = true)
	{
		Destroy (this, removeFromGame, canUseExplosion);
	}

	protected abstract void OnEnterAthmosphere (AstronomicalObject otherPlanet);

	protected abstract void OnExitAthmosphere (AstronomicalObject otherPlanet);

	protected abstract void Destroy (AstronomicalObject ps, bool removeFromGame, bool canUseExplosion);
}