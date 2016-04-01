using UnityEngine;
using System.Collections;

public class GravitationalField : MonoBehaviour
{
	public bool isActive = true;
	public GameObject parent;

	void Awake ()
	{
		if (parent == null) {
			if (transform.parent != null && transform.parent.gameObject != null) {
				parent = transform.parent.gameObject;
			} else {
				Debug.LogError ("Gravitational Field " + gameObject.name + " has no parent set!");
				isActive = false;
			}
		}
	}

	void OnTriggerEnter (Collider collider)
	{
		if (!isActive)
			return;

		if (collider.gameObject == null)
			return;

		var otherPlanet = collider.gameObject.GetComponent<AstronomicalObject> ();
				
//				Debug.Log ("OnTriggerEnter2D. " + name + " collided with " + collider.gameObject.name + ", parent name: " + parent.gameObject.name);
		if (otherPlanet != null) {
//						var playerPlanetScript = collider.GetComponent<PlanetScript> ();
//						if (otherPlanet.name == "Player" && playerPlanetScript != null && playerPlanetScript.name != "Player") {
//								Debug.Log ("player entered athmosphere on planet '" + otherPlanet.name + "'");
////								parent.SendMessage ("OnEnterAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
//						}
			parent.SendMessage ("OnEnterAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnTriggerExit (Collider collider)
	{
		if (!isActive)
			return;

		if (collider.gameObject == null)
			return;

		var otherPlanet = collider.gameObject.GetComponent<AstronomicalObject> ();
		if (otherPlanet != null) {
//						var playerPlanetScript = collider.GetComponent<PlanetScript> ();
//						if (otherPlanet.name == "Player" && playerPlanetScript != null && playerPlanetScript.name != "Player") {
//								Debug.Log ("player left athmosphere on planet '" + otherPlanet.name + "'");
////								parent.SendMessage ("OnExitAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
//						}
			parent.SendMessage ("OnExitAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
		}
	}
}
