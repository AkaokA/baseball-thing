using UnityEngine;
using System.Collections;

public class AppController : BaseballElement {

	public Inning currentInning;

	public GameObject Baseball;

	private GameObject currentBaseballInstance;
	private bool allowCameraMovement = true;

	public float minPitchSpeed = 8.5f;
	public float maxPitchSpeed = 20f;
	public float pitchAccuracy = 0.5f;

	// Use this for initialization
	void Start () {
		UpdateScoreboard ();

		// Intro animations
		app.views.mainCamera.GetComponent<CameraView>().MoveCamera ("infield", 3f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}
	
	// Update is called once per frame
	void Update () {

		// P: throw pitch
		if (Input.GetKeyDown (KeyCode.P)) {
			currentBaseballInstance = Instantiate (Baseball);
			currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (app.views.strikeZone.transform, Random.Range (minPitchSpeed, maxPitchSpeed), pitchAccuracy);
			allowCameraMovement = true;
		}

		// ARROW KEYS: Throw to base
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			currentBaseballInstance.GetComponent<BaseballView> ().ThrowBaseballAt (app.views.firstBase.transform);
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			currentBaseballInstance.GetComponent<BaseballView> ().ThrowBaseballAt (app.views.secondBase.transform);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			currentBaseballInstance.GetComponent<BaseballView> ().ThrowBaseballAt (app.views.thirdBase.transform);
		}

		// SPACE: hit pitch
		if (Input.GetKeyDown (KeyCode.Space)) {
			currentBaseballInstance.GetComponent<BaseballView>().HitBaseball ();
		}

		// go back to infield camera when latest ball slows down
		if (currentBaseballInstance && allowCameraMovement) {
			float currentBaseballSpeed = currentBaseballInstance.GetComponent<Rigidbody> ().velocity.magnitude;
			
			if ( currentBaseballSpeed < 1f) {
				app.views.mainCamera.GetComponent<CameraView>().MoveCamera ("infield", 3f);
				allowCameraMovement = false;
			}
		}
	}

	public void IncrementCount (string pitchOutcome) {
		switch(pitchOutcome) {
		case "ball":
			if (app.model.currentGame.currentInning.currentAtBat.balls == 3) {
				ResetCount ();
			}
			app.model.currentGame.currentInning.currentAtBat.balls++;
			break;
		case "strike":
			Debug.Log ("strike!");

			if (app.model.currentGame.currentInning.currentAtBat.strikes == 2) {
				ResetCount ();
				IncrementOuts ();
			}
			app.model.currentGame.currentInning.currentAtBat.strikes++;
			app.views.strike1Dot.GetComponent<UIDotView> ().changeColor (app.views.strike1Dot.GetComponent<UIDotView> ().strikeDotColor);

			break;
		case "foul":
			if (app.model.currentGame.currentInning.currentAtBat.strikes < 2) {
				app.model.currentGame.currentInning.currentAtBat.strikes++;				
			}
			break;
		case "in play":
			ResetCount ();
			break;
		}

		UpdateScoreboard ();
	}
		
	void ResetCount () {
		app.model.currentGame.currentInning.currentAtBat.balls = 0;
		app.model.currentGame.currentInning.currentAtBat.strikes = 0;

		UpdateScoreboard ();
	}

	void IncrementOuts () {
		if (app.model.currentGame.currentInning.outs == 2) {
			app.model.currentGame.currentInning.outs = 0;

			if (app.model.currentGame.currentInning.half == "top") {
				app.model.currentGame.currentInning.half = "bottom";
			} else {
				app.model.currentGame.currentInning.half = "top";
				app.model.currentGame.currentInning = new Inning (app.model.currentGame.currentInning.inningNumber++);
			}

		} else {
			app.model.currentGame.currentInning.outs++;
		}

		UpdateScoreboard ();
	}

	public void UpdateScoreboard () {
		// text labels
		app.views.awayTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.awayTeam.teamName.ToUpper ();
		app.views.homeTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.homeTeam.teamName.ToUpper ();
		app.views.awayScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.awayScore.ToString ();
		app.views.homeScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.homeScore.ToString ();
		app.views.inningLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.currentInning.half.ToUpper () + " " + app.model.currentGame.currentInning.inningNumber.ToString ();
	}
}
