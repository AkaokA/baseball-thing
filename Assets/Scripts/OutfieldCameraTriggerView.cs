using UnityEngine;
using System.Collections;

public class OutfieldCameraTriggerView : BaseballElement {
	void OnTriggerExit (Collider otherCollider) {
		if (otherCollider.tag == "Baseball") {
			app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("outfield", 0.25f);			
		}
	}
}
