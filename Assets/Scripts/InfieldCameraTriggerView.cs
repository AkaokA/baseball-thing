using UnityEngine;
using System.Collections;

public class InfieldCameraTriggerView : BaseballElement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.tag == "Baseball") {
			app.views.mainCamera.GetComponent<CameraView> ().ChangeCameraState ("infield", 0.5f);
		}
	}
}
