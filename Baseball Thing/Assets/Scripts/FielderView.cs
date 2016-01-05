using UnityEngine;
using System.Collections;

public class FielderView : BaseballElement {
	public int fieldingPositionNumber;

	public Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	private float maxSpeed = 8f;

	public bool hasTheBall = false;
	public bool fielderCanMove = true;

	public Vector3 idleLocation;

	private Player closestPlayer;

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

	public void Idle () {
		MoveToward (idleLocation);
	}

	public void PlayDefense () {
		Vector3 ballPosition = app.controller.currentBaseballInstance.transform.position;

		if (hasTheBall) {
			MoveToward (ballPosition);
		} else {
			if (IsClosestToBall()) {
				ChaseBall ();
			} else {
				// go back to a useful position
				BeUseful (fieldingPositionNumber);
			}
		}
	}
		
	public bool IsClosestToBall () {

		float minimumDistance = float.MaxValue;

		foreach (Player player in app.controller.fieldingTeam.players) {
			FielderView fielderView = player.fielderInstance.GetComponent<FielderView> ();
			Vector3 distanceToBall = app.controller.currentBaseballInstance.transform.position - fielderView.transform.position;
			distanceToBall.y = 0; // use only horizontal distance

			if ( distanceToBall.magnitude < minimumDistance ) {
				minimumDistance = distanceToBall.magnitude;
				closestPlayer = player;
			}
		}

		if (closestPlayer.fielderInstance.GetComponent<FielderView> () == this) {
			return true;
		} else {
			return false;
		}
	}

	public void ChaseBall () {
		if (app.controller.currentBaseballInstance.GetComponent<BaseballView>().ballIsRolling) {
			MoveToward (app.controller.currentBaseballInstance.transform.position);
		} else {
			// go to where the ball will be
			MoveToward (app.views.baseballLandingPoint.transform.position);
		}
	}
		
	public void BeUseful(int fieldingPositionNumber) {
		switch (fieldingPositionNumber) {
		case 1:
			MoveToward (idleLocation);
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
			MoveToward (idleLocation);
			break;
		case 7:
			MoveToward (idleLocation);
			break;
		case 8:
			MoveToward (idleLocation);
			break;
		case 9:
			MoveToward (idleLocation);
			break;
		}
	}

	public void AssignFieldingPosition (int positionNumber) {
		fieldingPositionNumber = positionNumber;

		switch (fieldingPositionNumber) {
		case 1: // pitcher
			idleLocation = new Vector3 (5f, 0f, 5f);
			break;
		case 2: // catcher
			idleLocation = new Vector3 (-1.5f, 0f, -1.5f);
			break;
		case 3: // 1st base
			idleLocation = new Vector3 (12.5f, 0f, 2.5f);
			break;
		case 4: // 2nd base
			idleLocation = new Vector3 (12.5f, 0f, 8f);
			break;
		case 5: // 3rd base
			idleLocation = new Vector3 (2.5f, 0f, 12.5f);
			break;
		case 6: // shortstop
			idleLocation = new Vector3 (8f, 0f, 12.5f);
			break;
		case 7: // left field
			idleLocation = new Vector3 (7f, 0f, 25f);
			break;
		case 8: // center field
			idleLocation = new Vector3 (20f, 0f, 20f);
			break;
		case 9: // right field
			idleLocation = new Vector3 (25f, 0f, 7f);
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
