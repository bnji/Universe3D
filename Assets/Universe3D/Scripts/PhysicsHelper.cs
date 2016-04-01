using UnityEngine;
using System.Collections;

public class PhysicsHelper
{
	public static Vector3 CalculateGravity (Rigidbody A, Rigidbody B)
	{
		Vector3 dist = B.transform.position - A.transform.position; 
		float r = dist.magnitude;
		dist /= r;
		float G = 9.8f;
		float m1 = A.mass;
		float m2 = B.mass;
		float F = (G * m1 * m2) / (r * r);
		return dist * F; // = force
	}

	public static Vector3 CalculateGravity (IAstronomicalObject A, IAstronomicalObject B)
	{
		return CalculateGravity (A.GetComponent<Rigidbody> (), B.GetComponent<Rigidbody> ());
	}

	public static Vector2 ApplyGravity (IAstronomicalObject A, IAstronomicalObject B)
	{
		var force = CalculateGravity (A, B);
		if (force.magnitude > Vector3.zero.magnitude) {
			A.GetComponent<Rigidbody> ().AddForce (force);
			B.GetComponent<Rigidbody> ().AddForce (-force);
		}
		return force;
	}
}
