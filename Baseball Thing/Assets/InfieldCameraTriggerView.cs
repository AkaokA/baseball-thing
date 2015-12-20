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
		app.views.mainCamera.GetComponent<CameraView>().MoveCamera("infield", 1f);
	}
}
