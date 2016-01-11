using UnityEngine;
using System.Collections;

public class FielderView : BaseballElement {
	public int fieldingPositionNumber;

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	public float maxSpeed;
	public float throwStrength;

	public bool hasTheBall = false;
	public bool coveringBase = false;

	public Vector3 idleLocation;
	private Player activeFielder;
	private Base targetBase;
	private Transform throwTargetPosition;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		// while the ball is in play
		if (app.controller.currentGame.currentInning.ballIsInPlay) {
			PlayDefense ();
		} else {
			if (app.controller.betweenInnings == false) {
				// go back to idle position
				Idle ();
			}
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

	public void StopMoving () {
		MoveToward (transform.position);
	}

	public void Idle () {
		MoveToward (idleLocation);
	}

	public void PlayDefense () {
		// if the ball is coming to this fielder
		if (GetActiveFielder ().fielderInstance.GetComponent<FielderView> () == this) {
			// don't move if covering a base
			if ( IsCoveringBase () ) { 
				StopMoving ();
			} else { // if not covering a base, go get the ball
				ChaseBall ();
			}				
		} else { // if the ball isn't coming to this fielder, cover an appropriate base
			CoverBases (fieldingPositionNumber);
		}

		// if fielder doesn't have the ball and just got the ball
		if ( !hasTheBall && GotBall () ) {
			// stop the ball
			app.controller.currentBaseballInstance.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);

			// stop moving
			StopMoving ();

			if (IsCoveringBase () == false) {
				// throw the ball somewhere helpful
				StartCoroutine ( ThrowBall () );
			}
			// caught in the air
			if ( app.controller.currentBaseballInstance.GetComponent<BaseballView> ().hasTouchedTheGround == false ) {
				app.controller.IncrementOuts ();
				app.controller.currentBatter.runnerInstance.GetComponent<RunnerView> ().goBackToDugout ();
			}
		} 
	}
		
	public Player GetActiveFielder () {
		float minimumDistance = float.MaxValue;
		Vector3 targetPosition;

		Vector3 leadTheTarget = app.controller.currentBaseballInstance.GetComponent<Rigidbody> ().velocity;
		leadTheTarget.y = 0;
		leadTheTarget = leadTheTarget / 4;

		if (app.controller.currentBaseballInstance.GetComponent<BaseballView> ().ballIsRolling) {
			targetPosition = app.controller.currentBaseballInstance.transform.position + leadTheTarget;
			targetPosition.y = 0;
		} else {
			targetPosition = app.views.baseballLandingPoint.transform.position;
			targetPosition.y = 0;
		}

		foreach (Player player in app.controller.fieldingTeam.lineup) {
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
		Vector3 leadTheTarget = app.controller.currentBaseballInstance.GetComponent<Rigidbody> ().velocity;
		leadTheTarget.y = 0;
		leadTheTarget = leadTheTarget / 4;

		if (app.controller.currentBaseballInstance.GetComponent<BaseballView>().ballIsRolling) {
			// skate to where the puck is going to be
			MoveToward (app.controller.currentBaseballInstance.transform.position + leadTheTarget);
		} else {
			// go to where the ball will land
			MoveToward (app.views.baseballLandingPoint.transform.position);
		}
	}

	public bool GotBall () {
		Vector3 distanceToBall = app.controller.currentBaseballInstance.transform.position - transform.position;
		float proximityThreshold = 1f;

		if ( distanceToBall.magnitude < proximityThreshold ) {
			hasTheBall = true;
			return true;
		} else {
			return false;
		}
	}
		
	public IEnumerator ThrowBall () {

		if ( app.controller.currentBaseballInstance != null ) {
			
			for ( int baseIndex = 1; baseIndex <= 3; baseIndex++ ) {
				if (app.controller.ballpark.bases[baseIndex].isOccupied) {
					if (baseIndex == 3) {
						targetBase = app.controller.ballpark.bases[0];
						throwTargetPosition = targetBase.baseGameObject.transform;
					} else {
						targetBase = app.controller.ballpark.bases[baseIndex + 1];
						throwTargetPosition = targetBase.baseGameObject.transform;
					}
				} else {
					throwTargetPosition = app.controller.fieldingTeam.lineup[0].fielderInstance.transform;
				}
			}
				
			yield return new WaitForSeconds (0.5f);

			BaseballView baseballView = app.controller.currentBaseballInstance.GetComponent<BaseballView> ();
			baseballView.ThrowBaseballAt (throwTargetPosition, throwStrength);

			yield return new WaitForSeconds (0.2f);

			hasTheBall = false;
		}
	}

	public bool FielderIsInDugout () {
		Vector3 distanceToDugout = app.controller.fieldingTeam.dugoutPosition - transform.position;
		distanceToDugout.y = 0;
		if ( distanceToDugout.magnitude < 1f ) {
			return true;
			Debug.Log (fieldingPositionNumber + " is in the dugout");
		} else {
			return false;
		}
	}

	public bool IsCoveringBase () {
		foreach (Base thisBase in app.controller.ballpark.bases) {
			Vector3 distanceToBase = thisBase.baseGameObject.transform.position - transform.position;
			distanceToBase.y = 0; // horizontal distance only

			if (distanceToBase.magnitude < 1f) {
				return true;
			}
		}
		return false;
	}
		
	public void CoverBases(int fieldingPositionNumber) {
		switch (fieldingPositionNumber) {
		case 1: // Pitcher
			switch (activeFielder.fieldingPositionNumber) {
//			case 2:
//				MoveToward (app.views.homePlate.transform.position);
//				break;
//			case 3:
//				MoveToward (app.views.firstBase.transform.position);
//				break;
//			case 5:
//				MoveToward (app.views.thirdBase.transform.position);
//				break;
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
				MoveToward (idleLocation);
			}
			break;
		case 5: // 3rd
			MoveToward (app.views.thirdBase.transform.position);
			break;
		case 6: // Shortstop
			if (app.controller.currentBaseballInstance.transform.position.x > app.controller.currentBaseballInstance.transform.position.z) {
				MoveToward (app.views.secondBase.transform.position);
			} else {
				MoveToward (idleLocation);
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
