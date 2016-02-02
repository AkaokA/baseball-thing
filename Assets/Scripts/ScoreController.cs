using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : BaseballElement {

	public GameObject[] baseIcons;
	public GameObject[] ballIcons;
	public GameObject[] strikeIcons;
	public GameObject[] outIcons;

	void Start () {

		// set up scoreboard views
		int index = 0;
		baseIcons = new GameObject[3];
		foreach (Transform baseIcon in app.views.scoreboardBases.transform) {
			baseIcons [index] = baseIcon.gameObject;
			index++;
		}

		// set up count bar views
		index = 0;
		ballIcons = new GameObject[3];
		foreach (Transform ballIcon in app.views.scoreboardBalls.transform) {
			if (ballIcon.gameObject.name != "Text") {
				ballIcons [index] = ballIcon.gameObject;
				index++;
			}
		}

		index = 0;
		strikeIcons = new GameObject[2];
		foreach (Transform strikeIcon in app.views.scoreboardStrikes.transform) {
			if (strikeIcon.gameObject.name != "Text") {
				strikeIcons [index] = strikeIcon.gameObject;
				index++;
			}
		}

		index = 0;
		outIcons = new GameObject[2];
		foreach (Transform outIcon in app.views.scoreboardOuts.transform) {
			if (outIcon.gameObject.name != "Text") {
				outIcons [index] = outIcon.gameObject;
				index++;
			}
		}

	}

	void Awake () {
		UpdateScoreboard ();
	}

	public void UpdateScoreboard () {
		if (app.controller.currentGame != null) {
			// text labels
			app.views.awayNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.controller.currentGame.awayTeam.teamName.ToUpper ();
			app.views.homeNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.controller.currentGame.homeTeam.teamName.ToUpper ();
			app.views.awayScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.controller.currentGame.awayTeam.score.ToString ();
			app.views.homeScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.controller.currentGame.homeTeam.score.ToString ();
			app.views.inningLabel.GetComponent<UnityEngine.UI.Text> ().text = app.controller.currentGame.currentInning.half.ToUpper () + " " + app.controller.currentGame.currentInning.inningNumber.ToString ();

			// count bar icons
			int index = 0;
			foreach (GameObject icon in ballIcons) {
				Image iconImage = icon.GetComponent<Image> ();

				if (index < app.controller.currentGame.currentInning.currentAtBat.balls) {
					iconImage.sprite = app.model.scoreboardDotActive;
					iconImage.color = app.model.goldColor;
				} else {
					iconImage.sprite = app.model.scoreboardDotEmpty;
					iconImage.color = app.model.whiteColor;
				}
				index++;
			}

			index = 0;
			foreach (GameObject icon in strikeIcons) {
				Image iconImage = icon.GetComponent<Image> ();

				if (index < app.controller.currentGame.currentInning.currentAtBat.strikes) {
					iconImage.sprite = app.model.scoreboardDotActive;
					iconImage.color = app.model.goldColor;
				} else {
					iconImage.sprite = app.model.scoreboardDotEmpty;
					iconImage.color = app.model.whiteColor;
				}
				index++;
			}

			index = 0;
			foreach (GameObject icon in outIcons) {
				Image iconImage = icon.GetComponent<Image> ();

				if (index < app.controller.currentGame.currentInning.outs) {
					iconImage.sprite = app.model.scoreboardDotActive;
					iconImage.color = app.model.redColor;
				} else {
					iconImage.sprite = app.model.scoreboardDotEmpty;
					iconImage.color = app.model.whiteColor;
				}
				index++;
			}


		}
	}

}
