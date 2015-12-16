using UnityEngine;
using System.Collections;

public class CameraStates : MonoBehaviour {

	public float lerpTime = 2f;
	public AnimationCurve cameraCurve;

	private float initialSize;
	private float initialHeight;

	// Use this for initialization
	void Start () {
		initialSize = GetComponent<Camera> ().orthographicSize;
		initialHeight = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void MoveCamera (string state) {
		StopCoroutine ("SwitchCameraState");
		initialSize = GetComponent<Camera> ().orthographicSize;
		initialHeight = transform.position.y;

		float finalCameraSize = 10f;
		float finalCameraHeight = 10f;

		switch(state) {
		case "plate":
			finalCameraSize = 2.5f;
			finalCameraHeight = 2.5f;
			break;
		case "infield":
			finalCameraSize = 7f;
			finalCameraHeight = 6f;
			break;
		case "outfield":
			finalCameraSize = 13f;
			finalCameraHeight = 13f;
			break;
		}

		StartCoroutine (SwitchCameraState (finalCameraSize, finalCameraHeight));
	}

	IEnumerator SwitchCameraState (float finalCameraSize, float finalCameraHeight) {
		float currentLerpTime;

		for ( currentLerpTime = 0f; currentLerpTime <= lerpTime; currentLerpTime += Time.deltaTime ) {

			float perc = currentLerpTime / lerpTime;
			float cameraSize;
			float cameraHeight;

			cameraSize = Mathf.LerpUnclamped(initialSize, finalCameraSize, cameraCurve.Evaluate (perc));
			cameraHeight = Mathf.LerpUnclamped(initialHeight, finalCameraHeight, cameraCurve.Evaluate (perc));

			GetComponent<Camera>().orthographicSize = cameraSize;

			Vector3 temp = transform.position;
			temp.y = cameraHeight;
			transform.position = temp;

			yield return null;
		}
	}
}
