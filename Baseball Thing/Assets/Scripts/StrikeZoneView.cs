using UnityEngine;
using System.Collections;

public class StrikeZoneView : BaseballElement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Baseball") {
			app.controller.UpdateCount ("strike");
		}
	}

}
