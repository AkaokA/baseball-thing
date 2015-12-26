using UnityEngine;
using System.Collections;

public class CameraView : BaseballElement {

//	public AnimationCurve cameraCurve;

	public float finalCameraSize = 0f;
	public float finalCameraHeight = 0f;
	public float finalCameraDiagPosition = 0f;
	public float finalCameraAngle = 0f;

	private float sizeVelocity = 0.0f;
	private Vector3 transformVelocity = new Vector3 (0,0,0);
	private float angleVelocity = 0.0f;

	public float smoothTime = 0f;

	// Use this for initialization
	void Start () {
		
	}

	public void ChangeCameraState (string state, float time) {
		smoothTime = time;

		switch(state) {
		case "overhead":
			finalCameraSize = 15f;
			finalCameraHeight = 16f;
			finalCameraDiagPosition = 10f;
			finalCameraAngle = 90f;
			break;
		case "infield":
			finalCameraSize = 7f;
			finalCameraHeight = 6f;
			finalCameraDiagPosition = 0f;
			finalCameraAngle = 30f;
			break;
		case "outfield":
			finalCameraSize = 13f;
			finalCameraHeight = 13f;
			finalCameraDiagPosition = 0f;
			finalCameraAngle = 30f;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		float cameraSize = GetComponent<Camera> ().orthographicSize;
		float cameraAngle = transform.rotation.eulerAngles.x;

		// set position
		Vector3 targetPosition = new Vector3(finalCameraDiagPosition, finalCameraHeight, finalCameraDiagPosition);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime);

		// change size/angle values with smoothDamp
		cameraSize = Mathf.SmoothDamp(cameraSize, finalCameraSize, ref sizeVelocity, smoothTime);
		cameraAngle = Mathf.SmoothDamp(cameraAngle, finalCameraAngle, ref angleVelocity, smoothTime);

		// set size
		GetComponent<Camera>().orthographicSize = cameraSize;

		// set angle
		Quaternion tempRotation = transform.rotation;
		Vector3 tempEulerAngles = tempRotation.eulerAngles;
		tempEulerAngles.x = cameraAngle;
		tempRotation.eulerAngles = tempEulerAngles;
		transform.rotation = tempRotation;
	}
}