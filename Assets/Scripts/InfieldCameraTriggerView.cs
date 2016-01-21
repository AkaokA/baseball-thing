using UnityEngine;
using System.Collections;

public class InfieldCameraTriggerView : BaseballElement {
	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.tag == "Baseball") {
			app.views.mainCamera.GetComponent<CameraView> ().ChangeCameraState ("infield", 0.5f);
		}
	}
}
