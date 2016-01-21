using UnityEngine;
using System.Collections;

public class PitchRegisterView : BaseballElement {
	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.tag == "Baseball" && app.controller.currentGame.currentInning.ballIsInPlay == false) {
			app.controller.RegisterPitch ();
		}
	}
}
