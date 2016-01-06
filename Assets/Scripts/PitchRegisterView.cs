using UnityEngine;
using System.Collections;

public class PitchRegisterView : BaseballElement {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.tag == "Baseball" && app.controller.currentGame.currentInning.ballIsInPlay == false) {
			app.controller.RegisterPitch ();
		}
	}
}
