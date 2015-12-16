using UnityEngine;
using System.Collections;

public class OutfieldCameraTriggerView : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit (Collider other) {
		GameObject.Find ("Main Camera").GetComponent<CameraView>().MoveCamera("outfield");
	}

}
