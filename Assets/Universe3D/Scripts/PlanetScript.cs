using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlanetScript : AstronomicalObject
{
	public float dir = 1f;

	void Awake ()
	{
		GetComponent<Rigidbody> ().AddForce (Vector3.up * dir);
		planets.AddRange (GameObject.FindObjectsOfType<PlanetScript> ());
	}

	protected override void OnEnterAthmosphere (AstronomicalObject otherPlanet)
	{
		if (!planets.Contains (otherPlanet)) {
//			Debug.Log ("Added " + otherPlanet.name + " to " + gameObject.name);
//			planets.Add (otherPlanet);
		}
	}

	protected override void OnExitAthmosphere (AstronomicalObject otherPlanet)
	{
		if (planets != null && planets.Contains (otherPlanet)) {
//			Debug.Log ("Removed planet " + otherPlanet.name + " from planet " + gameObject.name);
//			planets.Remove (otherPlanet);
		}
	}

	protected override void Destroy (AstronomicalObject ps, bool removeFromGame, bool canUseExplosion)
	{
		if (ps != null) {
			OnExitAthmosphere (ps);
			// Remove the planet from the list in the other planets
			for (int i = 0; i < planets.Count; i++) {
				var planet = planets [i];
				if (planet != null && planet.enabled) {
					planet.planets.Remove (ps);
				}
			}

			// Destroy the planet
			if (removeFromGame) {
				GameObject.Destroy (gameObject);
				GameObject.Destroy (ps.gameObject);
			} else {
				gameObject.SetActive (false);
			}
		}
	}

	//	void OnBecameVisible ()
	//	{
	//		Highlight ();
	//		Debug.Log (name + " is visible");
	//	}
	//
	//	void OnBecameInvisible ()
	//	{
	//		RestoreCurrentObject ();
	//		Debug.Log (name + " is invisible");
	//	}
	//
	//	//The old color of the currentObject
	//	private Color oldColor;
	//	//How much should the object be highlighted
	//	private float highlightFactor = 1f;
	//
	//	private void Highlight ()
	//	{
	//		Renderer r = GetComponent (typeof(Renderer)) as Renderer;
	//		oldColor = r.material.GetColor ("_Color");
	//		Color newColor = new Color (oldColor.r + highlightFactor, oldColor.g + highlightFactor, oldColor.b + highlightFactor, oldColor.a);
	//		r.material.SetColor ("_Color", newColor);
	//	}
	//
	//	private void RestoreCurrentObject ()
	//	{
	//		Renderer r = GetComponent (typeof(Renderer)) as Renderer;
	//		r.material.SetColor ("_Color", oldColor);
	//	}
}