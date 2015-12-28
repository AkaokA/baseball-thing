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
			if (otherCollider.transform.position.x > 0 && otherCollider.transform.position.z > 0) {
				app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("outfield", 0.5f);			
			}
		}

	}
}
