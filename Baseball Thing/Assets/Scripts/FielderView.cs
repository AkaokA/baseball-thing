using UnityEngine;
using System.Collections;

public class FielderView : MonoBehaviour {

	public Vector3 targetPosition;
	public float smoothTime = 0.1f;
	public float maxSpeed = 100f;

	private Vector3 transformVelocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime, maxSpeed);		
	}

	public void UpdateTargetPosition (Vector3 newTarget) {
		targetPosition = newTarget;
	}

}
