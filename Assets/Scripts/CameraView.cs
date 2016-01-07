using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraView : BaseballElement {

	public float targetCameraSize = 0f;
	public float targetCameraHeight = 0f;
	public float targetCameraX = 0f;
	public float targetCameraZ = 0f;
	public float targetCameraAngle = 0f;

	public float targetEffectIntensity = 0f;

	private float sizeVelocity = 0.0f;
	private Vector3 transformVelocity = new Vector3 (0,0,0);
	private float angleVelocity = 0.0f;
	private float effectVelocity = 0.0f;

	public float smoothTime = 0f;

	// Use this for initialization
	void Start () {
		
	}

	public void ChangeCameraState (string state, float time) {
		smoothTime = time;

		switch(state) {
		case "overhead":
			targetCameraSize = 30f;
			targetCameraHeight = 12.5f;
			targetCameraX = 0f;
			targetCameraZ = 25f;
			targetCameraAngle = 90f;
			targetEffectIntensity = 1f;
			break;
		case "atbat":
			targetCameraSize = 3.25f;
			targetCameraHeight = 2.75f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 30f;
			targetEffectIntensity = 0f;
			break;
		case "infield":
			targetCameraSize = 5.5f;
			targetCameraHeight = 5f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 30f;
			targetEffectIntensity = 0f;
			break;
		case "outfield":
			targetCameraSize = 13f;
			targetCameraHeight = 12f;
			targetCameraX = 0f;
			targetCameraZ = 0f;
			targetCameraAngle = 30f;
			targetEffectIntensity = 0f;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		float cameraSize = GetComponent<Camera> ().orthographicSize;
		float cameraAngle = transform.rotation.eulerAngles.x;
//		float effectIntensity = GetComponent<BloomOptimized> ().intensity;

		// set position
		Vector3 targetPosition = new Vector3(targetCameraX, targetCameraHeight, targetCameraZ);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref transformVelocity, smoothTime);

		// change size/angle values with smoothDamp
		cameraSize = Mathf.SmoothDamp(cameraSize, targetCameraSize, ref sizeVelocity, smoothTime);
		cameraAngle = Mathf.SmoothDamp(cameraAngle, targetCameraAngle, ref angleVelocity, smoothTime);
//		effectIntensity = Mathf.SmoothDamp(effectIntensity, targetEffectIntensity, ref effectVelocity, smoothTime);

		// set size
		GetComponent<Camera>().orthographicSize = cameraSize;

		// set angle
		Quaternion tempRotation = transform.rotation;
		Vector3 tempEulerAngles = tempRotation.eulerAngles;
		tempEulerAngles.x = cameraAngle;
		tempRotation.eulerAngles = tempEulerAngles;
		transform.rotation = tempRotation;

		// set effect intensity
//		GetComponent<BloomOptimized> ().intensity = effectIntensity;
	}
}