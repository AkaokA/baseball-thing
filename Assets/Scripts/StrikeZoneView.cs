using UnityEngine;
using System.Collections;

public class StrikeZoneView : BaseballElement {
	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.tag == "Baseball") {
			app.controller.currentGame.currentInning.currentPitchIsStrike = true;
		}
	}

}
