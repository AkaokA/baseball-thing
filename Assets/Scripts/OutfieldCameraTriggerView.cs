using UnityEngine;
using System.Collections;

public class OutfieldCameraTriggerView : BaseballElement {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerExit (Collider otherCollider) {
		if (otherCollider.tag == "Baseball") {
			app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("outfield", 0.25f);			
		}
	}
}
