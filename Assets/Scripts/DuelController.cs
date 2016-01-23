using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuelController : BaseballElement {

	public DuelGridLocation currentPitchLocation;
	public DuelGridLocation currentSwingLocation;

	private int centerColumn;
	private int centerRow;

	// Use this for initialization
	void Start () {
		centerColumn = Mathf.FloorToInt (DuelGrid.gridColumns / 2);
		centerRow = Mathf.FloorToInt (DuelGrid.gridRows / 2);

		currentPitchLocation = new DuelGridLocation (centerColumn, centerRow);
		currentSwingLocation = new DuelGridLocation (centerColumn, centerRow);

		app.views.duelSwingMarker.SetActive (true);
		app.views.duelPitchMarker.SetActive (false);
		app.views.duelPitchEndMarker.SetActive (false);

		app.views.duelBatterPhase1.SetActive (true);
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (false);
	}

	public void OnConfirmSwing () {
		// hide batter phase 1
		app.views.duelBatterPhase1.SetActive (false);
		app.views.duelSwingMarker.SetActive (false);

		// show pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (true);
		app.views.duelPitchMarker.SetActive (true);
		app.views.duelPitchEndMarker.SetActive (true);

		// (re-)enable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = true;

		// move pitch destination
		app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToCell (currentPitchLocation.column - 1, currentPitchLocation.row + 4);

		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
	}

	public void OnConfirmPitch () {
		// hide pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelPitchEndMarker.SetActive (false);

		// show batter phase 2
		app.views.duelBatterPhase2.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		// disable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = false;

		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);
	}

	public void OnFollowThrough () {
		StartCoroutine (ResolvePitch (true));
	}

	public void OnLayOffPitch () {
		StartCoroutine (ResolvePitch (false));
	}

	IEnumerator ResolvePitch (bool didSwing) {
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (true);

		// move pitch marker according to selected pitch
		app.views.duelPitchMarker.GetComponent<DuelMarker> ().MoveToCell (currentPitchLocation.column - 1, currentPitchLocation.row + 4);

		// determine outcome based on marker proximity
		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);

		if (didSwing) {
			// batter swung
		} else {
			// batter didn't swing
		}


		yield return new WaitForSeconds (5.0f);

		app.views.duelPitchMarker.GetComponent<DuelMarker> ().MoveToCell (centerColumn, centerRow);
		app.views.duelSwingMarker.GetComponent<DuelMarker> ().MoveToCell (centerColumn, centerRow);

		app.views.duelOutcomePhase.SetActive (false);
		app.views.duelBatterPhase1.SetActive (true);
		app.views.duelPitchMarker.SetActive (false);
	}
}
