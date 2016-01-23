using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuelController : BaseballElement {

	public DuelGridLocation currentPitchLocation;
	public DuelGridLocation currentSwingLocation;

	// Use this for initialization
	void Start () {
		currentPitchLocation = new DuelGridLocation ();
		currentSwingLocation = new DuelGridLocation ();

		app.views.duelSwingMarker.SetActive (true);
		app.views.duelPitchMarker.SetActive (false);

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

		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
	}

	public void OnConfirmPitch () {
		// hide pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (false);

		// show batter phase 2
		app.views.duelBatterPhase2.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		// disable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = false;

		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);
	}

	public void OnFollowThrough () {
		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);
	}

	public void OnLayOffPitch () {

	}


}
