using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DuelController : BaseballElement {

	public DuelGridCoordinates currentPitchLocation;
	public DuelGridCoordinates currentSwingLocation;

	public Pitch[] pitches;
	public Pitch currentPitch;

	private int centerColumn;
	private int centerRow;

	// game state variables
	private bool batterDidSwing = false;

	// Use this for initialization
	void Start () {
		centerColumn = Mathf.FloorToInt (DuelGrid.gridColumns / 2);
		centerRow = Mathf.FloorToInt (DuelGrid.gridRows / 2);
			
		SetUpPitchInventory ();
		SetUpNewDuel ();
	}

	void SetUpPitchInventory () {
		pitches = new Pitch[6];
		pitches [0] = new Pitch ("Fastball", 0, 1);
		pitches [1] = new Pitch ("Curveball", 0, 4);
		pitches [2] = new Pitch ("Slider", 3, 1);
		pitches [3] = new Pitch ("2-Seam", -2, 0);
		pitches [4] = new Pitch ("Riser", 0, -2);
		pitches [5] = new Pitch ("Slurve", 2, 3);

		// create pitch buttons
		int index = 0;
		foreach (Pitch pitch in pitches) {
			GameObject pitchButton = Instantiate (app.views.pitchSelectButton);
			pitchButton.transform.SetParent (app.views.pitchInventory.transform);
			pitchButton.GetComponent<Text> ().text = pitch.name;

			RectTransform buttonRect = pitchButton.GetComponent<RectTransform> ();

			Vector2 buttonPosition = new Vector2 (20f, -44f * index);
			buttonRect.localPosition = buttonPosition;
			buttonRect.localScale = new Vector3 (1, 1, 1);

			int localIndex = index;
			pitchButton.GetComponent<Button> ().onClick.AddListener (delegate {
				currentPitch = pitches [localIndex];
				Debug.Log (currentPitch.name);

				// highlight selected pitch button
				ColorBlock allButtonColors = app.views.pitchInventory.GetComponentInChildren<Button> ().colors;
				allButtonColors.normalColor = app.model.redTeamColor;
				allButtonColors.highlightedColor = allButtonColors.normalColor;

				foreach (Button button in app.views.pitchInventory.GetComponentsInChildren<Button> ()) {
					button.colors = allButtonColors;
				}

				Button thisButton = app.views.pitchInventory.transform.GetChild (localIndex).GetComponent<Button> ();
				ColorBlock thisButtonColors = thisButton.colors;
				thisButtonColors.normalColor = app.model.buttonHighlightColor;
				thisButtonColors.highlightedColor = thisButtonColors.normalColor;
				thisButton.colors = thisButtonColors;

				// move pitch destination marker
				StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());
			});

			index++;
		}

		// set scrollview content height
		RectTransform pitchInventoryRect = app.views.pitchInventory.GetComponent<RectTransform> ();
		pitchInventoryRect.sizeDelta = new Vector2(0, 44 * pitches.Length);
		app.views.pitchInventoryScrollView.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1;

		currentPitch = pitches [0];
		Button firstButton = app.views.pitchInventory.transform.GetChild (0).GetComponent<Button> ();
		ColorBlock firstButtonColors = firstButton.colors;
		firstButtonColors.normalColor = app.model.buttonHighlightColor;
		firstButtonColors.highlightedColor = firstButtonColors.normalColor;
		firstButton.colors = firstButtonColors;
	}

	public void SetUpNewDuel () {
		currentPitchLocation = new DuelGridCoordinates (centerColumn, centerRow);
		currentSwingLocation = new DuelGridCoordinates (centerColumn, centerRow);

		// show batter phase 1
		app.views.duelBatterPhase1.SetActive (true);
		app.views.duelSwingMarker.SetActive (true);

		app.views.duelPitchMarker.GetComponent<DuelMarker> ().ResetPosition ();
		app.views.duelSwingMarker.GetComponent<DuelMarker> ().ResetPosition ();

		// hide everything else
		app.views.duelPitchMarker.SetActive (false);
		app.views.duelPitchEndMarker.SetActive (false);
		app.views.duelPitcherPhase1.SetActive (false);
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (false);

		// enable raycasts on swing marker
		app.views.duelSwingMarker.GetComponent<Image> ().raycastTarget = true;
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

		// move pitch destination marker
		StartCoroutine (app.views.duelPitchEndMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());

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
		app.views.duelSwingMarker.SetActive (false);
		StartCoroutine (ResolvePitch (false));
	}

	IEnumerator ResolvePitch (bool didSwing) {
		app.views.duelBatterPhase2.SetActive (false);
		app.views.duelOutcomePhase.SetActive (true);

		// disable raycasts on swing marker
		app.views.duelSwingMarker.GetComponent<Image> ().raycastTarget = false;

		// move pitch marker according to selected pitch
		StartCoroutine (app.views.duelPitchMarker.GetComponent<DuelMarker> ().MoveToPitchDestination ());

		// determine outcome based on marker proximity
		Debug.Log ("Swing: " + currentSwingLocation.column + ", " + currentSwingLocation.row);
		Debug.Log ("Pitch: " + currentPitchLocation.column + ", " + currentPitchLocation.row);

		if (didSwing) {
			// batter swung
		} else {
			// batter didn't swing
		}

		yield return new WaitForSeconds (5.0f);
		SetUpNewDuel ();
	}
}
