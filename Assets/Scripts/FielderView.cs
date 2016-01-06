using UnityEngine;
using System.Collections;

public class FielderView : BaseballElement {
	public int fieldingPositionNumber;

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	public float maxSpeed;

	public bool hasTheBall = false;
	public bool fielderCanMove = true;

	public Vector3 idleLocation;
	private Player activeFielder;

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
		
		// move fielder to target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);

		// point fielder towards stuff
		if (app.controller.currentBaseballInstance) {
			transform.LookAt (targetPosition);
		} else {
			transform.LookAt (app.views.homePlate.transform.position);
		}

	}
		
	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

	public void Idle () {
		MoveToward (idleLocation);
	}

	public void PlayDefense () {
		if (GetActiveFielder ().fielderInstance.GetComponent<FielderView> () == this) {
			ChaseBall ();			
		} else {
			CoverBases (fieldingPositionNumber);
		}
	}
		
	public Player GetActiveFielder () {
		float minimumDistance = float.MaxValue;
		Vector3 targetPosition;

		if (app.controller.currentBaseballInstance.GetComponent<BaseballView>().ballIsRolling) {
			targetPosition = app.controller.currentBaseballInstance.transform.position;
		} else {
			targetPosition = app.views.baseballLandingPoint.transform.position;
		}

		foreach (Player player in app.controller.fieldingTeam.players) {
			FielderView fielderView = player.fielderInstance.GetComponent<FielderView> ();

			Vector3 distanceToTarget = targetPosition - fielderView.transform.position;
			distanceToTarget.y = 0; // use only horizontal distance

			if ( distanceToTarget.magnitude < minimumDistance ) {
				minimumDistance = distanceToTarget.magnitude;
				activeFielder = player;
			}
		}

		return activeFielder;
	}

	public void ChaseBall () {
		if (app.controller.currentBaseballInstance.GetComponent<BaseballView>().ballIsRolling) {
			// skate to where the puck is going to be
			Vector3 ballVelocity = app.controller.currentBaseballInstance.GetComponent<Rigidbody> ().velocity;
			MoveToward (app.controller.currentBaseballInstance.transform.position + (ballVelocity/4));
		} else {
			// go to where the ball will land
			MoveToward (app.views.baseballLandingPoint.transform.position);
		}
	}
		
	public void CoverBases(int fieldingPositionNumber) {
		switch (fieldingPositionNumber) {
		case 1: // Pitcher
			switch (activeFielder.fieldingPositionNumber) {
			case 2:
				MoveToward (app.views.homePlate.transform.position);
				break;
			case 3:
				MoveToward (app.views.firstBase.transform.position);
				break;
			case 5:
				MoveToward (app.views.thirdBase.transform.position);
				break;
			default:
				MoveToward (idleLocation);
				break;
			}
			break;
		case 2: // Catcher
			MoveToward (app.views.homePlate.transform.position);
			break;
		case 3: // 1st
			MoveToward (app.views.firstBase.transform.position);
			break;
		case 4: // 2nd
			if (app.controller.currentBaseballInstance.transform.position.x < app.controller.currentBaseballInstance.transform.position.z) {
				MoveToward (app.views.secondBase.transform.position);
			} else {
				if ( app.controller.currentBaseballInstance.transform.position.magnitude > 20f ) {
					MoveToward (app.controller.currentBaseballInstance.transform.position - transform.position);
				} else {
					MoveToward (idleLocation);
				}
			}
			break;
		case 5: // 3rd
			MoveToward (app.views.thirdBase.transform.position);
			break;
		case 6: // Shortstop
			if (app.controller.currentBaseballInstance.transform.position.x > app.controller.currentBaseballInstance.transform.position.z) {
				MoveToward (app.views.secondBase.transform.position);
			} else {
				if ( app.controller.currentBaseballInstance.transform.position.magnitude > 20f ) {
					MoveToward (app.controller.currentBaseballInstance.transform.position - transform.position);
				} else {
					MoveToward (idleLocation);
				}
			}
			break;
		case 7: // Left Field
			MoveToward (idleLocation);
			break;
		case 8: // Center Field
			MoveToward (idleLocation);
			break;
		case 9: // Right Field
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
}
