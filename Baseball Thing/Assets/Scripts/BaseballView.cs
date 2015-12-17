using UnityEngine;
using System.Collections;

public class BaseballView : MonoBehaviour {
	public float hitForceMin = 500f;
	public float hitForceMax = 1200f;
	public float hitForce;
	public Vector3 hitDirection = new Vector3(0f,0f,0f);

	private Vector3 pitchDirection = new Vector3(0f,0f,0f);
	private float distanceToTarget;
	private float pitchSpeed;

	const float gravityConstant = -9.81f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PitchBaseballWithSpeed(Transform target, float pitchSpeed) {
		// move to location of pitcher's mound
		transform.position = new Vector3(5f, 0.75f, 5f);

		pitchDirection = target.position - transform.position; // get target direction 
		pitchDirection.y = 0; // retain only the horizontal direction
		distanceToTarget = pitchDirection.magnitude; // get horizontal distance
		pitchDirection = pitchDirection.normalized;

		// find angle using trajectory formula
		float pitchAngle = Mathf.Asin ( Physics.gravity.magnitude * distanceToTarget / Mathf.Pow (pitchSpeed, 2) ) / 2;

		// set vertical angle
		pitchDirection.y = Mathf.Tan (pitchAngle);

		GetComponent<Rigidbody>().velocity = pitchSpeed * pitchDirection;
	}

	public void HitBaseball() {
		hitDirection = Random.onUnitSphere;
		hitDirection.x = Mathf.Abs(hitDirection.x);
		hitDirection.z = Mathf.Abs(hitDirection.z);

		hitForce = Random.Range (hitForceMin, hitForceMax);

		GetComponent<Rigidbody>().AddForce( hitDirection * hitForce);
	}

//	public void PitchBaseballWithAngle(Transform target, float pitchAngle) {
//		// move to location of pitcher's mound
//		transform.position = new Vector3(5f, 1f, 5f);
//
//		pitchDirection = target.position - transform.position; // get target direction 
//		heightDifference = pitchDirection.y; // get height difference
//		pitchDirection.y = 0; // retain only the horizontal direction
//		distanceToTarget = pitchDirection.magnitude; // get horizontal distance
//
//		float pitchAngleRad = pitchAngle * Mathf.Deg2Rad; // convert angle to radians 
//
//		pitchDirection.y = distanceToTarget * Mathf.Tan (pitchAngleRad);
//		distanceToTarget += heightDifference / Mathf.Tan (pitchAngleRad);
//
//		pitchSpeed = Mathf.Sqrt (distanceToTarget * Physics.gravity.magnitude / Mathf.Sin (2 * pitchAngleRad));
//		Debug.Log (pitchSpeed);
//
//		GetComponent<Rigidbody>().velocity = pitchSpeed * pitchDirection.normalized;
//	}

}