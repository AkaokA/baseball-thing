﻿using UnityEngine;
using System.Collections;

public class BaseballView : BaseballElement {

	public GameObject heightIndicator;
	private GameObject landingPointView;

	public float hitForceMin = 500f;
	public float hitForceMax = 1200f;
	public float hitForce;
	public Vector3 hitDirection = new Vector3(0f,0f,0f);

	private float pitchSpeed;


	// Use this for initialization
	void Start () {
		landingPointView = app.views.baseballLandingPoint;
	}
	
	// Update is called once per frame
	void Update () {
		// kill ball if it goes too far away
		if (transform.position.x < -6f || transform.position.z < -6f || transform.position.x > 40f || transform.position.z > 40f) {
			app.controller.ResetPlay ();
		}
	}

	public void PitchBaseballWithSpeed(Transform target, float pitchSpeed, float accuracy) {
		// move to location of pitcher's mound
		Vector3 pitcherPosition = app.controller.currentGame.homeTeam.players [1 -1].idleLocation; // fielding positions are 1-indexed; that's why this is ugly
		pitcherPosition.y = 1f;
		transform.position = pitcherPosition;

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
		pitchDirection.y = Mathf.Sin (pitchAngle);

		GetComponent<Rigidbody> ().velocity = pitchSpeed * pitchDirection;
	}

	public void HitBaseball() {
		hitDirection.x = Random.Range (0f,1f);
		hitDirection.y = Random.Range (0f,0.5f);
		hitDirection.z = Random.Range (0f,1f);

		hitForce = Random.Range (hitForceMin, hitForceMax);

		GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
		GetComponent<Rigidbody> ().WakeUp();
		GetComponent<Rigidbody> ().AddForce( hitDirection * hitForce);

		app.controller.currentGame.currentInning.ballIsInPlay = true;
		StartCoroutine (SetLandingPoint ());
	}

	public void ThrowBaseballAt(Transform target) {
		// Hide height indicator
		app.controller.currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (false);

		// put the ball back near the ground
		Vector3 tempPosition = transform.position;
		tempPosition.y = 1.0f;
		transform.position = tempPosition;

		// This seems wrong...
		float throwHeight = 2f;
		float throwSpeed = Mathf.Sqrt (2 * throwHeight * Physics.gravity.magnitude);

		Vector3 throwDirection = target.position - transform.position; // get target direction 
		throwDirection.y = 0; // retain only the horizontal direction

		float distanceToTarget = throwDirection.magnitude; // get horizontal distance
		throwDirection = throwDirection.normalized;

		// find angle using trajectory formula
		float throwAngle = Mathf.Asin ( Physics.gravity.magnitude * distanceToTarget / Mathf.Pow (throwSpeed, 2) ) / 2;

		// set vertical angle
		throwDirection = throwDirection.normalized;

		if (float.IsNaN (throwAngle)) {
			throwSpeed = 10;
			throwDirection.y = Mathf.Sin (45 * Mathf.Deg2Rad);
		} else {
			throwDirection.y = Mathf.Sin (throwAngle);
		}

		GetComponent<Rigidbody> ().WakeUp();
		GetComponent<Rigidbody> ().velocity = throwSpeed * throwDirection;
	}

	void OnCollisionStay () {
		landingPointView.SetActive (false);
	}

	void OnCollisionExit (Collision collision) {
		if (app.controller.currentGame.currentInning.ballIsInPlay) {
			StartCoroutine (SetLandingPoint ());
		}
	}

	public IEnumerator SetLandingPoint () {
		yield return 0;

		Vector3 landingPoint = new Vector3 (0, 0, 0);
		Vector3 velocity = GetComponent<Rigidbody> ().velocity;
		Vector3 unitVelocity = velocity.normalized;

		float angle;
		float distance;

		angle = Mathf.Asin (unitVelocity.y); // lol I guess?

		distance = (velocity.sqrMagnitude * Mathf.Sin (2 * angle)) / Physics.gravity.magnitude;

		unitVelocity.y = 0;
		landingPoint = app.controller.currentBaseballInstance.transform.position + (unitVelocity * distance);

		landingPointView.transform.position = landingPoint;
		landingPointView.SetActive (true);
	}

}