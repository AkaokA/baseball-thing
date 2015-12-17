using UnityEngine;
using System.Collections;

public class CameraView : BaseballElement {

	public AnimationCurve cameraCurve;

	private float initialSize;
	private float initialHeight;
	private float initialDiagPosition;
	private float initialAngle;

	// Use this for initialization
	void Start () {
		initialSize = GetComponent<Camera> ().orthographicSize;
		initialHeight = transform.position.y;
		initialDiagPosition = transform.position.x;
		initialAngle = transform.rotation.eulerAngles.x;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void MoveCamera (string state, float time) {
		StopCoroutine ("SwitchCameraState");

		initialSize = GetComponent<Camera> ().orthographicSize;
		initialHeight = transform.position.y;
		initialDiagPosition = transform.position.x;
		initialAngle = transform.rotation.eulerAngles.x;

		float finalCameraSize = 0f;
		float finalCameraHeight = 0f;
		float finalCameraDiagPosition = 0f;
		float finalCameraAngle = 0f;

		switch(state) {
		case "overhead":
			finalCameraSize = 16f;
			finalCameraHeight = 2.5f;
			finalCameraDiagPosition = 8f;
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

		StartCoroutine (SwitchCameraState (finalCameraSize, finalCameraHeight, finalCameraDiagPosition, finalCameraAngle, time));
	}

	IEnumerator SwitchCameraState (float finalCameraSize, float finalCameraHeight, float finalCameraDiagPosition, float finalCameraAngle, float time) {
		float currentLerpTime;

		for ( currentLerpTime = 0f; currentLerpTime <= time; currentLerpTime += Time.deltaTime ) {

			float perc = currentLerpTime / time;
			float cameraSize;
			float cameraHeight;
			float cameraDiagPosition;
			float cameraAngle;

			cameraSize = Mathf.LerpUnclamped(initialSize, finalCameraSize, cameraCurve.Evaluate (perc));
			cameraHeight = Mathf.LerpUnclamped(initialHeight, finalCameraHeight, cameraCurve.Evaluate (perc));
			cameraDiagPosition = Mathf.LerpUnclamped(initialDiagPosition, finalCameraDiagPosition, cameraCurve.Evaluate (perc));
			cameraAngle = Mathf.LerpUnclamped(initialAngle, finalCameraAngle, cameraCurve.Evaluate (perc));

			GetComponent<Camera>().orthographicSize = cameraSize;

			Vector3 tempPosition = transform.position;
			tempPosition.x = cameraDiagPosition;
			tempPosition.y = cameraHeight;
			tempPosition.z = cameraDiagPosition;
			transform.position = tempPosition;

			Quaternion tempRotation = transform.rotation;
			Vector3 tempEulerAngles = tempRotation.eulerAngles;
			tempEulerAngles.x = cameraAngle;
			tempRotation.eulerAngles = tempEulerAngles;
			transform.rotation = tempRotation;

			yield return null;
		}
	}
}
