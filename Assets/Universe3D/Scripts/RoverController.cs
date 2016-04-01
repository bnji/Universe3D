using UnityEngine;
using System.Collections;

public class RoverController : MonoBehaviour
{
	public float forwardSpeed = 100f;
	public float backwardSpeed = 25f;
	public float rotationSpeed = 25f;

	public Rigidbody[] rbs;

	void Awake ()
	{
		
	}

	// Use this for initialization
	void Start ()
	{
		//		Cursor.visible = false;
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		var force = transform.forward * Time.deltaTime;
		if (Input.GetKey (KeyCode.W)) {
			foreach (var rb in rbs) {
				rb.AddForce (forwardSpeed * force);
			}
		}
		if (Input.GetKey (KeyCode.S)) {
			foreach (var rb in rbs) {
				if (rb.velocity.sqrMagnitude > 0f) {
					rb.AddForce (-backwardSpeed * force);
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			foreach (var rb in rbs) {
				rb.AddForce (transform.up * Time.deltaTime * 500f, ForceMode.Impulse);
			}
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (-Vector3.up);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up);
		}
//		if (Mathf.Abs (Camera.main.transform.localRotation.y) <= 0.2f) 
		{
			var mousePos = Camera.main.ScreenToViewportPoint (Input.mousePosition);
			mousePos = new Vector3 (0f, mousePos.x - 0.5f, 0f);
			//		Debug.Log ("mousePos: " + mousePos + ", velocity: " + rb.velocity.sqrMagnitude);
			if (Mathf.Abs (mousePos.x) >= 0.1f || Mathf.Abs (mousePos.y) >= 0.1f) {
				Camera.main.transform.Rotate (mousePos * rotationSpeed * Time.deltaTime);
			}
		}
	}
}
