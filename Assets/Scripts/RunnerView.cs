﻿using UnityEngine;
using System.Collections;

public class RunnerView : BaseballElement {

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	public float maxSpeed;

	public int batterIndex;
	public int currentBaseIndex = 0;
	public Base targetBase;

	public bool hadAnAtBat = false;
	private bool reachedBase = false;
	private bool scoreCounted = false;
	public bool isOut = false;

	// Update is called once per frame
	void Update () {
		// move runner to target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);

		// point runner towards stuff
		transform.LookAt (targetPosition);
	
		TouchingBase ();

		if (hadAnAtBat && RunnerIsInDugout ()) {
			Destroy ( app.controller.battingTeam.lineup[batterIndex].runnerInstance );
		}
	}
		
	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

	public bool RunnerIsInDugout () {
		Vector3 distanceToDugout = app.controller.battingTeam.dugoutPosition - transform.position;
		distanceToDugout.y = 0;
		if ( distanceToDugout.magnitude < 1f ) {
			return true;
		} else {
			return false;
		}
	}

	public void advanceToNextBase () {
		if (currentBaseIndex <= 2) {
			if (currentBaseIndex == 0 && reachedBase) {
				goBackToDugout ();
				return;
			} else {
				targetBase = app.controller.ballpark.bases[currentBaseIndex + 1];
			}
		} else {
			targetBase = app.controller.ballpark.homePlate;
		}

		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void retreatToLastBase () {
		targetBase = app.controller.ballpark.bases[currentBaseIndex];
		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void goBackToDugout () {
		MoveToward (app.controller.battingTeam.dugoutPosition);
	}

	public void TouchingBase () {
		foreach (Base thisBase in app.controller.ballpark.bases) {
			Vector3 distanceToBase = thisBase.baseGameObject.transform.position - transform.position;
			distanceToBase.y = 0; // horizontal distance only

			if (distanceToBase.magnitude < 0.5f) {
				currentBaseIndex = System.Array.IndexOf (app.controller.ballpark.bases, thisBase);
				thisBase.isOccupied = true;

				// crossing the plate
				if ( thisBase == app.controller.ballpark.homePlate ) {
					if (reachedBase && !scoreCounted && !isOut) {
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
