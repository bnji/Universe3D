using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class SpaceController : MonoBehaviour
{
	public bool isMobileControl = true;
	public float forwardSpeed = 100f;
	public float backwardSpeed = 25f;
	public float rotationSpeed = 25f;
	public float slowDownSpeed = 100f;
	public float maxVelocity = 2000f;

	public Text textVelocity;

	Rigidbody rb;

	//The currently highlighted object
	private GameObject currentObject;
	//The old color of the currentObject
	private Color oldColor;
	//How much should the object be highlighted
	private float highlightFactor = 1f;

	private void HighlightCurrentObject ()
	{
		Renderer r = currentObject.GetComponent (typeof(Renderer)) as Renderer;
		oldColor = r.material.GetColor ("_Color");
		Color newColor = new Color (oldColor.r + highlightFactor, oldColor.g + highlightFactor, oldColor.b + highlightFactor, oldColor.a);
		r.material.SetColor ("_Color", newColor);
	}

	//Restores the current object to it's former state.
	private void RestoreCurrentObject ()
	{
		if (currentObject != null) { //IF we actually have an object to restore
			Renderer r = currentObject.GetComponent (typeof(Renderer)) as Renderer;
			r.material.SetColor ("_Color", oldColor);
			currentObject = null;
		}
	}

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start ()
	{
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
//		HighlightObjects ();

		HandleInput ();
	}

	void HandleInput ()
	{
		if (textVelocity != null) {
			textVelocity.text = "" + Mathf.Round (rb.velocity.sqrMagnitude);
		}
		var force = transform.forward * Time.deltaTime;

		if (isMobileControl) {
			if (Input.touchCount == 0) {
				if (rb.velocity.sqrMagnitude >= 1f) {
					rb.AddForce (-1f * rb.velocity * Time.deltaTime * slowDownSpeed);
				} else {
					rb.velocity = Vector3.zero;
				}
			}
		} else {
			if (Input.GetKey (KeyCode.W)) {
				if (rb.velocity.sqrMagnitude < maxVelocity) {
					rb.AddForce (forwardSpeed * force);
				}
			} else if (Input.GetKey (KeyCode.S)) {
				if (rb.velocity.sqrMagnitude > 0f) {
					rb.AddForce (-backwardSpeed * force);
				}
			} else {
				if (rb.velocity.sqrMagnitude >= 1f) {
					rb.AddForce (-1f * rb.velocity * Time.deltaTime * slowDownSpeed);
				} else {
					rb.velocity = Vector3.zero;
				}
			}
		}
		Vector3 mousePos = Vector3.zero;
		if (Input.touchCount >= 1) {
			mousePos = Input.GetTouch (0).position;
			if (rb.velocity.sqrMagnitude < maxVelocity) {
				rb.AddForce (forwardSpeed * force);
			}
		} 
		if (Input.mousePresent && Input.GetMouseButton (0)) {
			mousePos = Input.mousePosition;
		}
		if (mousePos != Vector3.zero) {
			var dist = Vector3.Distance (lastMousePosition, mousePos);
//			Debug.Log (dist);
			if (dist > 50f || lastMousePosition == Vector3.zero) {
				lastMousePosition = mousePos;
			}
			lastMousePosition = Vector3.Slerp (lastMousePosition, mousePos, Time.deltaTime * 0.05f);
			mousePos = Camera.main.ScreenToViewportPoint (lastMousePosition);
			mousePos = new Vector3 (-1f * (mousePos.y - 0.5f), mousePos.x - 0.5f, 0f);
			transform.Rotate (mousePos * rotationSpeed * Time.deltaTime);
		}
	}

	Vector3 lastMousePosition = Vector3.zero;

	void HighlightObjects ()
	{
		var raycasthits = Physics.RaycastAll (transform.position, transform.TransformPoint (Vector3.forward));
		if (raycasthits.Length == 0) {
			RestoreCurrentObject ();
		}
		var hits = new List<string> ();
		foreach (var hit in raycasthits) {
			if (hit.collider != null) {
				if (!hits.Contains (hit.collider.name)) {
					hits.Add (hit.collider.name);
				}
			}
			if (currentObject == null) {
				currentObject = hit.transform.gameObject;
				HighlightCurrentObject ();
			} else if (hit.transform != currentObject.transform) {
				RestoreCurrentObject ();
				currentObject = hit.transform.gameObject;
				HighlightCurrentObject ();
			}
			//			var rend = hit.transform.GetComponent<Renderer> ();
			//			if (rend) {
			//				// Change the material of all hit colliders
			//				// to use a transparent shader.
			//				rend.material.shader = Shader.Find ("Transparent/Diffuse");
			//				Color tempColor = rend.material.color;
			//				tempColor.a = 0.3F;
			//				rend.material.color = tempColor;
			//			}
		}
		//		Debug.Log (string.Join (", ", hits.ToArray ()));
	}
}
