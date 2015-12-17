using UnityEngine;
using System.Collections;

public class OutfieldCameraTriggerView : BaseballElement {

	public float cameraTransitionTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerExit (Collider other) {
		if (other.transform.position.x > 0 && other.transform.position.z > 0) {
			GameObject.Find ("Main Camera").GetComponent<CameraView>().MoveCamera("outfield", cameraTransitionTime);			
		}
	}

}
