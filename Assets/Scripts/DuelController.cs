using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuelController : BaseballElement {

	public DuelGridCoordinates currentPitchLocation;
	public DuelGridCoordinates currentSwingLocation;

	public Pitch[] pitches;
	public Pitch currentPitch;

	public int centerColumn;
	public int centerRow;

	private bool firstTime = true;

	// game state variables
	private bool batterDidSwing = false;

	// Use this for initialization
	void Start () {
		centerColumn = Mathf.FloorToInt (DuelGrid.gridColumns / 2);
		centerRow = Mathf.FloorToInt (DuelGrid.gridRows / 2);

		currentPitchLocation = new DuelGridCoordinates (centerColumn, centerRow);
		currentSwingLocation = new DuelGridCoordinates (centerColumn, centerRow);
			
		SetUpPitchInventory ();
		StartCoroutine (SetUpNewDuel ());
	}

	void SetUpPitchInventory () {
		pitches = new Pitch[4];
		pitches [0] = new Pitch ("Fastball", 0, 1, app.model.pitchIconFastball);
		pitches [1] = new Pitch ("Curveball", -1, 4, app.model.pitchIconCurveball);
		pitches [2] = new Pitch ("2-Seam", -2, 1, app.model.pitchIcon2Seam);
		pitches [3] = new Pitch ("Slider", 3, 0, app.model.pitchIconSlider);

		// create pitch buttons
		int index = 0;
		foreach (Pitch pitch in pitches) {
			GameObject pitchButton = app.views.pitchInventory.transform.GetChild (index).gameObject;
			Text buttonText = pitchButton.GetComponentInChildren<Text> ();
			Image buttonImage = pitchButton.GetComponentInChildren<Image> ();

			buttonText.text = pitch.name;
			buttonText.color = app.model.fadedWhiteColor;

			buttonImage.sprite = pitch.pitchIcon;
			buttonImage.color = app.model.fadedWhiteColor;

			int localIndex = index;
			pitchButton.GetComponent<Button> ().onClick.AddListener (delegate {
				currentPitch = pitches [localIndex];
				Debug.Log (currentPitch.name);
				Button thisButton = app.views.pitchInventory.transform.GetChild (localIndex).GetComponent<Button> ();

				HighlightCurrentPitchButton ();

				// move pitch destination marker
				if (app.views.duelPitchEndMarker.activeInHierarchy) {
					StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
				}

			});

			index++;
		}
			
		currentPitch = pitches [0];
		Button firstButton = app.views.pitchInventory.transform.GetChild (0).GetComponent<Button> ();
		ColorBlock firstButtonColors = firstButton.colors;
		firstButtonColors.normalColor = app.model.buttonHighlightColor;
		firstButtonColors.highlightedColor = firstButtonColors.normalColor;
		firstButton.colors = firstButtonColors;
	}

	IEnumerator SetUpNewDuel () {
		yield return null;
		currentPitchLocation = new DuelGridCoordinates (centerColumn, centerRow);
		currentSwingLocation = new DuelGridCoordinates (centerColumn, centerRow);

		// show batter phase 1
		app.views.duelBatterPhase1.SetActive (true);
		app.views.duelPitchMarker.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		if (firstTime == false) {
			app.views.duelPitchMarker.GetComponent<DuelMarker> ().ResetPosition ();
			app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().ResetPosition ();
			app.views.duelSwingMarker.GetComponent<DuelMarker> ().ResetPosition ();
		}
		firstTime = false;

		// hide everything else
		app.views.duelPitchMarker.SetActive (false);
		app.views.duelPitchEndMarker.SetActive (false);
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (false);

		// enable raycasts on swing marker
		app.views.duelSwingMarker.GetComponent<Image> ().raycastTarget = true;

	}

	void HighlightCurrentPitchButton () {
		// enable pitch buttons
		foreach (Button button in app.views.pitchInventory.GetComponentsInChildren<Button> () ) {
			button.interactable = true;
			Text buttonText = button.GetComponentInChildren<Text> ();
			Image buttonImage = button.GetComponentInChildren<Image> ();

			buttonText.color = new Color (1, 1, 1);
			buttonImage.color = new Color (1, 1, 1);
		}

		int currentPitchIndex = System.Array.IndexOf (pitches, currentPitch);
		Button selectedButton = app.views.pitchInventory.transform.GetChild (currentPitchIndex).GetComponent<Button> ();
		selectedButton.GetComponentInChildren<Text> ().color = app.model.redTeamColor;
		selectedButton.GetComponentInChildren<Image> ().color = app.model.redTeamColor;
	}

	public void OnConfirmSwing () {
		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);

		// hide batter phase 1
		app.views.duelBatterPhase1.SetActive (false);
		app.views.duelSwingMarker.SetActive (false);

		// show pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (true);
		app.views.duelPitchMarker.SetActive (true);
		app.views.duelPitchEndMarker.SetActive (true);

		// (re-)enable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = true;

		// move pitch destination marker
		if (app.views.duelPitchEndMarker.activeInHierarchy) {
			StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
		}

		HighlightCurrentPitchButton ();
	}

	public void OnConfirmPitch () {
		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);

		// hide pitcher phase 1
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelPitchEndMarker.SetActive (false);

		// show batter phase 2
		app.views.duelBatterPhase2.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		// disable raycasts on pitch marker
		app.views.duelPitchMarker.GetComponent<Image> ().raycastTarget = false;

		// disable pitch buttons
		foreach (Button button in app.views.pitchInventory.GetComponentsInChildren<Button> () ) {
			button.interactable = false;
			Text buttonText = button.GetComponentInChildren<Text> ();
			Image buttonImage = button.GetComponentInChildren<Image> ();

			buttonText.color = app.model.fadedWhiteColor;
			buttonImage.color = app.model.fadedWhiteColor;
		}

	}

	public void OnFollowThrough () {
		StartCoroutine (ResolvePitch (true));
	}

	public void OnLayOffPitch () {
		app.views.duelSwingMarker.SetActive (false);
		StartCoroutine (ResolvePitch (false));
	}

	IEnumerator ResolvePitch (bool didSwing) {
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (true);

		// disable raycasts on swing marker
		app.views.duelSwingMarker.GetComponent<Image> ().raycastTarget = false;

		// move pitch marker according to selected pitch
		if (app.views.duelPitchMarker.activeInHierarchy) {
			StartCoroutine (app.views.duelPitchMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
		}

		// determine outcome based on marker proximity
		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);

		if (didSwing) {
			// batter swung
		} else {
			// batter didn't swing
		}

		yield return new WaitForSeconds (4.0f);
		StartCoroutine (SetUpNewDuel ());
	}
}
