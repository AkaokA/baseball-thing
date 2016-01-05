using UnityEngine;
using System.Collections;

public class FielderView : BaseballElement {

	public Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	private float maxSpeed = 8f;

	public bool hasTheBall = false;
	public bool isClosestToBall = false;
	public bool fielderCanMove = true;

	public Vector3 idleLocation;
	public int fieldingPosition;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		// while the ball is in play
		if (app.controller.currentGame.currentInning.ballIsInPlay) {
			PlayDefense ();
		} else {
			// go back to idle position
			Idle ();
		}

		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);		
	}

	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}
		
	public void PlayDefense () {
		Vector3 landingPoint = app.views.baseballLandingPoint.transform.position;
		Vector3 ballPosition = app.controller.currentBaseballInstance.transform.position;
		Vector3 distanceToBall = ballPosition - transform.position;
		distanceToBall.y = 0; // use only horizontal distance
		Vector3 distanceFromIdlePosition = idleLocation - transform.position;

		if (hasTheBall) {
			MoveToward (ballPosition);
		} else {
			if (distanceToBall.magnitude < 5f && distanceFromIdlePosition.magnitude < 10f) {
				ChaseBall ();
			} else {
				// go back to a useful position
				BeUseful (fieldingPosition);
			}
		}
	}

	public void Idle () {
		MoveToward (idleLocation);
	}

	public void ChaseBall () {
		if (app.controller.currentBaseballInstance.GetComponent<BaseballView>().ballIsRolling) {
			MoveToward (app.controller.currentBaseballInstance.transform.position);
		} else {
			// go to where the ball will be
			MoveToward (app.views.baseballLandingPoint.transform.position);
		}
	}

	public void BeUseful(int fieldingPosition) {
		switch (fieldingPosition) {
		case 1:
			MoveToward (app.controller.fieldingTeam.players [fieldingPosition - 1].idleLocation);
			break;
		case 2:
			MoveToward (app.views.homePlate.transform.position);
			break;
		case 3:
			MoveToward (app.views.firstBase.transform.position);
			break;
		case 4:
			MoveToward (app.views.secondBase.transform.position);
			break;
		case 5:
			MoveToward (app.views.thirdBase.transform.position);
			break;
		case 6:
			MoveToward (app.controller.fieldingTeam.players [fieldingPosition - 1].idleLocation);
			break;
		case 7:
			MoveToward (app.controller.fieldingTeam.players [fieldingPosition - 1].idleLocation);
			break;
		case 8:
			MoveToward (app.controller.fieldingTeam.players [fieldingPosition - 1].idleLocation);
			break;
		case 9:
			MoveToward (app.controller.fieldingTeam.players [fieldingPosition - 1].idleLocation);
			break;
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Baseball" && !hasTheBall) {
			hasTheBall = true;
		}
	}


	void OnTriggerExit (Collider collider) {
		if (collider.tag == "Baseball") {
			hasTheBall = false;
		}
	}
}
