using UnityEngine;
using System.Collections;

public class RunnerView : BaseballElement {

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	public float maxSpeed;

	public int batterIndex;
	public bool isOnBase = false;
	public int currentBaseIndex = 0;
	public Base targetBase;

	public bool hadAnAtBat = false;
	private bool reachedBase = false;
	private bool scoreCounted = false;
	public bool runnerIsOut = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		// move runner to target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);

		// point runner towards stuff
		transform.LookAt (targetPosition);
	
		TouchingBase ();

		if (hadAnAtBat) {
			Vector3 distanceToDugout = app.controller.battingTeam.dugoutPosition - transform.position;
			distanceToDugout.y = 0;
			if ( distanceToDugout.magnitude < 1f ) {
				Destroy ( app.controller.battingTeam.lineup[batterIndex - 1].runnerInstance );
			}
		}
	}
		
	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

	public void advanceToNextBase () {
		if (currentBaseIndex <= 2) {
			if (currentBaseIndex == 0 && reachedBase) {
				goBackToDugout ();
				return;
			} else {
				targetBase = app.controller.currentGame.bases[currentBaseIndex + 1];
			}
		} else {
			targetBase = app.controller.currentGame.homePlate;
		}

		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void retreatToLastBase () {
		targetBase = app.controller.currentGame.bases[currentBaseIndex];
		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void goBackToDugout () {
		MoveToward (app.controller.battingTeam.dugoutPosition);
	}

	public void TouchingBase () {
		foreach (Base thisBase in app.controller.currentGame.bases) {
			Vector3 distanceToBase = thisBase.baseGameObject.transform.position - transform.position;
			distanceToBase.y = 0; // horizontal distance only

			if (distanceToBase.magnitude < 0.5f) {
				currentBaseIndex = System.Array.IndexOf (app.controller.currentGame.bases, thisBase);
				thisBase.isOccupied = true;

				// crossing the plate
				if ( thisBase == app.controller.currentGame.homePlate ) {
					if (reachedBase && !scoreCounted && !runnerIsOut) {
						goBackToDugout ();
						app.controller.IncrementScore ();
						scoreCounted = true;
					}
				} else {
					reachedBase = true;
					hadAnAtBat = true;
				}

			} else {
				thisBase.isOccupied = false;
			}
		}
	}

}
