using UnityEngine;
using System.Collections;

public class RunnerView : BaseballElement {

	private Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	private float maxSpeed = 6f;

	public bool isOnBase = false;
	public int currentBase = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		// move runner to target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);

		// point runner towards stuff
		transform.LookAt (targetPosition);
	}
		
	public void MoveToward (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

	public void advanceToNextBase () {
		switch (currentBase) {
		case 0:
			MoveToward (app.controller.currentGame.firstBase.baseGameObject.transform.position);
			break;
		case 1:
			MoveToward (app.controller.currentGame.secondBase.baseGameObject.transform.position);
			break;
		case 2:
			MoveToward (app.controller.currentGame.thirdBase.baseGameObject.transform.position);
			break;
		case 3:
			MoveToward (app.controller.currentGame.homePlate.baseGameObject.transform.position);
			break;
		}
	}
}
