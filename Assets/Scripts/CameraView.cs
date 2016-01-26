using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraView : BaseballElement {
	private Vector3 targetCameraPosition;

	public float targetCameraSize = 0f;
	public float targetCameraHeight = 0f;
	public float targetCameraX = 0f;
	public float targetCameraZ = 0f;
	public float targetCameraAngle = 0f;

	public float targetEffectIntensity = 0f;

	private float xVelocity = 0.0f;
	private float zVelocity = 0.0f;
	private float heightVelocity = 0.0f;
	private float sizeVelocity = 0.0f;
	private float angleVelocity = 0.0f;

	public float smoothTime = 0f;

	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		// game starts with overhead camera
		ChangeCameraState ("overhead", 0f);
	}

	public void ChangeCameraState (string state, float time) {
		smoothTime = time;

		switch(state) {
		case "overhead":
			targetCameraSize = 28f;
			targetCameraHeight = 30f;
			targetCameraX = 12f;
			targetCameraZ = 12f;
			targetCameraAngle = 90f;
			break;
		case "atbat":
			targetCameraSize = 2.35f;
			targetCameraHeight = 2f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 20f;
			break;
		case "infield":
			targetCameraSize = 16f;
			targetCameraHeight = 12f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 30f;
			break;
		case "outfield":
			targetCameraSize = 40f;
			targetCameraHeight = 30f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 30f;
			break;
		}

		targetCameraPosition = new Vector3 (targetCameraX, targetCameraHeight, targetCameraZ);

		if (!isMoving) {
			StartCoroutine (MoveCamera ());
		}
	}

	IEnumerator MoveCamera () {
		isMoving = true;
		while ( (targetCameraPosition - transform.localPosition).magnitude > 0.01f ) {

			// set position
			float cameraX = transform.position.x;
			float cameraZ = transform.position.z;
			float cameraHeight = transform.position.y;

			cameraX = Mathf.SmoothDamp(cameraX, targetCameraX, ref xVelocity, smoothTime);
			cameraZ = Mathf.SmoothDamp(cameraZ, targetCameraZ, ref zVelocity, smoothTime);
			cameraHeight = Mathf.SmoothDamp(cameraHeight, targetCameraHeight, ref heightVelocity, smoothTime);

			Vector3 cameraPosition = new Vector3 (cameraX, cameraHeight, cameraZ);
			transform.position = cameraPosition;

			// set size
			float cameraSize = GetComponent<Camera> ().orthographicSize;
			cameraSize = Mathf.SmoothDamp(cameraSize, targetCameraSize, ref sizeVelocity, smoothTime);
			GetComponent<Camera>().orthographicSize = cameraSize;

			// set angle
			float cameraAngle = transform.rotation.eulerAngles.x;
			cameraAngle = Mathf.SmoothDamp(cameraAngle, targetCameraAngle, ref angleVelocity, smoothTime);

			Quaternion tempRotation = transform.rotation;
			Vector3 tempEulerAngles = tempRotation.eulerAngles;
			tempEulerAngles.x = cameraAngle;
			tempRotation.eulerAngles = tempEulerAngles;
			transform.rotation = tempRotation;

			yield return null;
		}
		isMoving = false;
//		Debug.Log ("camera's done moving!"); 
	}

}