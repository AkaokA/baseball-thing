using UnityEngine;
using System.Collections;

public class InfieldCameraTriggerView : BaseballElement {

	private bool triggerIsActive = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit () {
		triggerIsActive = true;
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (triggerIsActive) {
			app.views.mainCamera.GetComponent<CameraView>().MoveCamera("infield", 0.5f);
		}

	}
}
