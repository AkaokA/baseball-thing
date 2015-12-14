using UnityEngine;
using System.Collections;

public class InfieldCameraTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		GameObject.Find ("Main Camera").GetComponent<CameraStates>().MoveCamera("plate");
	}
	void OnTriggerExit (Collider other) {
		GameObject.Find ("Main Camera").GetComponent<CameraStates>().MoveCamera("infield");
	}

}
