using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuelGridController : BaseballElement {

	// Use this for initialization
	void Start () {
		app.views.duelBatterPhase1.SetActive (true);
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelBatterPhase2.SetActive (false);
	}

	public void OnConfirmSwing () {
		// hide batter phase 1
		app.views.duelBatterPhase1.SetActive (false);
		app.views.duelSwingMarker.SetActive (false);

		// show pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (true);
		app.views.duelPitchMarker.SetActive (true);
	}

	public void OnConfirmPitch () {
		// hide pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (false);

		// show batter phase 2
		app.views.duelBatterPhase2.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		// disable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = false;
	}

	public void OnFollowThrough () {
		
	}

	public void OnLayOffPitch () {

	}


}
