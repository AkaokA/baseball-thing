using UnityEngine;
using System.Collections;

public class FielderView : BaseballElement {

	public Vector3 targetPosition;
	private Vector3 transformVelocity;
	private float smoothTime = 0.25f;
	private float maxSpeed = 8f;

	public bool hasTheBall = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);		
	}

	public void UpdateTargetPosition (Vector3 newTarget) {
		newTarget.y = 0;
		targetPosition = newTarget;
	}

}
