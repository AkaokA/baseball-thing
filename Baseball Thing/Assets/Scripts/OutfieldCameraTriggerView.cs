using UnityEngine;
using System.Collections;

public class OutfieldCameraTriggerView : BaseballElement {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerExit (Collider other) {
		if (other.transform.position.x > 0 && other.transform.position.z > 0) {
			app.views.mainCamera.GetComponent<CameraView>().MoveCamera("outfield", 1.5f);			
			app.views.infieldCameraTrigger.SetActive (true);
		}
	}

}
