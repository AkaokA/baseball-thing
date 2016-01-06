using UnityEngine;
using System.Collections;

public class RunnerView : BaseballElement {

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	public float maxSpeed;

	public bool isOnBase = false;
	public int currentBaseIndex = 0;
	public Base targetBase;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		// move runner to target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);

		// point runner towards stuff
		transform.LookAt (targetPosition);

		TouchBase ();
	}
		
	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

	public void advanceToNextBase () {
		if (currentBaseIndex < 3) {
			targetBase = app.controller.currentGame.bases[currentBaseIndex + 1];
		} else {
			targetBase = app.controller.currentGame.homePlate;
		}

		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void retreatToLastBase () {
		targetBase = app.controller.currentGame.bases[currentBaseIndex];
		MoveToward (targetBase.baseGameObject.transform.position);
	}

	public void TouchBase () {
		foreach (Base thisBase in app.controller.currentGame.bases) {
			Vector3 distanceToBase = thisBase.baseGameObject.transform.position - transform.position;
			if (distanceToBase.magnitude < 1f) {
				currentBaseIndex = System.Array.IndexOf (app.controller.currentGame.bases, thisBase);

				// this doesn't work yet
//				if ( currentBaseIndex != 0 && thisBase == app.controller.currentGame.homePlate ) {
//					MoveToward (app.controller.currentGame.awayDugoutPosition);
//				}
			}
		}
	}

}
