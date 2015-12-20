using UnityEngine;
using System.Collections;

public class BaseballView : BaseballElement {

	public GameObject heightIndicator;

	public float hitForceMin = 500f;
	public float hitForceMax = 1200f;
	public float hitForce;
	public Vector3 hitDirection = new Vector3(0f,0f,0f);

	private float pitchSpeed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// kill ball if it goes too far foul
		if (transform.position.x < -10f || transform.position.z < -10f || transform.position.x > 40f || transform.position.z > 40f) {
			Destroy (gameObject);
		}
	}

	public void PitchBaseballWithSpeed(Transform target, float pitchSpeed, float accuracy) {
		// move to location of pitcher's mound
		transform.position = new Vector3(5f, 0.75f, 5f);

		Vector3 pitchDirection = target.position - transform.position; // get target direction 
		pitchDirection.y = 0; // retain only the horizontal direction
		float distanceToTarget = pitchDirection.magnitude; // get horizontal distance
		pitchDirection = pitchDirection.normalized;

		// find angle using trajectory formula
		float pitchAngle = Mathf.Asin ( Physics.gravity.magnitude * distanceToTarget / Mathf.Pow (pitchSpeed, 2) ) / 2;

		// add randomness according to accuracy
		float randomness = 0.25f;
		pitchDirection.x += Random.Range (-randomness, randomness) * (1 - accuracy);
		pitchDirection.y += Random.Range (-randomness, randomness) * (1 - accuracy);

		// set vertical angle
		pitchDirection = pitchDirection.normalized;
		pitchDirection.y = Mathf.Tan (pitchAngle);

		GetComponent<Rigidbody> ().velocity = pitchSpeed * pitchDirection;
	}

	public void HitBaseball() {
		hitDirection = Random.onUnitSphere;
		hitDirection.x = Mathf.Abs(hitDirection.x);
		hitDirection.y = Mathf.Abs(hitDirection.z);
		hitDirection.z = Mathf.Abs(hitDirection.z);

		hitForce = Random.Range (hitForceMin, hitForceMax);

		GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
		GetComponent<Rigidbody> ().WakeUp();
		GetComponent<Rigidbody> ().AddForce( hitDirection * hitForce);

		app.model.currentGame.currentInning.ballIsInPlay = true;
	}

	public void ThrowBaseballAt(Transform target) {
		// put the ball back near the ground
		Vector3 tempPosition = transform.position;
		tempPosition.y = 1.0f;
		transform.position = tempPosition;

		float throwHeight = 8f;
		float throwSpeed = Mathf.Sqrt (2 * throwHeight * Physics.gravity.magnitude);

		Vector3 throwDirection = target.position - transform.position; // get target direction 
		throwDirection.y = 0; // retain only the horizontal direction

		float distanceToTarget = throwDirection.magnitude; // get horizontal distance
		throwDirection = throwDirection.normalized;

		// find angle using trajectory formula
		float throwAngle = Mathf.Asin ( Physics.gravity.magnitude * distanceToTarget / Mathf.Pow (throwSpeed, 2) ) / 2;

		// set vertical angle
		throwDirection = throwDirection.normalized;
		throwDirection.y = Mathf.Tan (throwAngle);

		GetComponent<Rigidbody> ().WakeUp();
		GetComponent<Rigidbody> ().velocity = throwSpeed * throwDirection;
	}

//	public Vector3 SetLandingPoint () {
//		GameObject landingPointView = app.views.baseballLandingPoint;
//		Vector3 landingPoint = new Vector3 (0, 0, 0);
//		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
//		float angle;
//		float distance;
//
//		velocity = velocity.normalized;
//		angle = Mathf.Asin (velocity.y); // lol I guess?
//		distance = 0; // lol gotta check the formulas
//
//		landingPointView.transform.position = landingPoint;
//		landingPointView.SetActive (true);
//	}

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