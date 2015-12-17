using UnityEngine;
using System.Collections;

public class BaseballGroundEffectsView: BaseballElement {

	public float markerScaleFactor = 6.4f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempPosition = transform.parent.position;
		tempPosition.y = 0.005f;
		transform.position = tempPosition;

		Quaternion tempRotation = transform.rotation;
		tempRotation = Quaternion.Euler(90, 0, 0);
		transform.rotation = tempRotation;

		float markerScale = transform.parent.position.y * markerScaleFactor;

		if (transform.parent.position.y >= 0) {
			Vector3 tempScale;
			tempScale.x = markerScale;
			tempScale.y = markerScale;
			tempScale.z = 1f;
			transform.localScale = tempScale;
		} else {
			transform.localScale = new Vector3 (0f, 0f, 0f);
		}
	}
}
