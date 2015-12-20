using UnityEngine;
using System.Collections;

public class AppController : BaseballElement {

	public Inning currentInning;

	public GameObject Baseball;
	public GameObject currentBaseballInstance;

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
			app.views.infieldCameraTrigger.SetActive (false); // prevent the baseball instance from triggering the camera
			currentBaseballInstance = Instantiate (Baseball);
			currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (app.views.strikeZone.transform, Random.Range (minPitchSpeed, maxPitchSpeed), pitchAccuracy);
		}

		if (currentBaseballInstance) {

			// SPACE: hit pitch
			if (Input.GetKeyDown (KeyCode.Space)) {
				currentBaseballInstance.GetComponent<BaseballView> ().HitBaseball ();
				currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (true);
//				currentBaseballInstance.GetComponent<BaseballView> ().SetLandingPoint ();
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
		}
	}

	public void IncrementCount (string pitchOutcome) {
		switch(pitchOutcome) {
		case "ball":
			IncrementBalls ();
			break;
		case "strike":
			IncrementStrikes ();
			break;
		case "foul":
			Debug.Log ("foul!");
			if (app.model.currentGame.currentInning.currentAtBat.strikes < 2) {
				IncrementStrikes ();				
			}
			break;
		case "in play":
			Debug.Log ("in play!");
			break;
		}
	}
		
	void ResetCount () {
		app.model.currentGame.currentInning.currentAtBat.balls = 0;
		app.model.currentGame.currentInning.currentAtBat.strikes = 0;
	}

	public void UpdateScoreboard () {
		// text labels
		app.views.awayTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.awayTeam.teamName.ToUpper ();
		app.views.homeTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.homeTeam.teamName.ToUpper ();
		app.views.awayScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.awayScore.ToString ();
		app.views.homeScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.homeScore.ToString ();
		UpdateInningLabel ();
	}

	public void UpdateInningLabel () {
		app.views.inningLabel.GetComponent<UnityEngine.UI.Text> ().text = app.model.currentGame.currentInning.half.ToUpper () + " " + app.model.currentGame.currentInning.inningNumber.ToString ();
	}

	public void IncrementBalls () {
		UIDotView ball1Dot = app.views.ball1Dot.GetComponent<UIDotView> ();
		UIDotView ball2Dot = app.views.ball2Dot.GetComponent<UIDotView> ();
		UIDotView ball3Dot = app.views.ball3Dot.GetComponent<UIDotView> ();

		switch(app.model.currentGame.currentInning.currentAtBat.balls) {
		case 0:
			Debug.Log ("ball 1!");
			app.model.currentGame.currentInning.currentAtBat.balls++;
			ball1Dot.StartCoroutine (ball1Dot.changeColor (ball1Dot.ballDotColor));
			break;
		case 1:
			Debug.Log ("ball 2!");
			app.model.currentGame.currentInning.currentAtBat.balls++;
			ball2Dot.StartCoroutine (ball2Dot.changeColor (ball2Dot.ballDotColor));
			break;
		case 2:
			Debug.Log ("ball 3!");
			app.model.currentGame.currentInning.currentAtBat.balls++;
			ball3Dot.StartCoroutine (ball3Dot.changeColor (ball3Dot.ballDotColor));
			break;
		case 3:
			Debug.Log ("walk!");
			ResetCount ();
			ball1Dot.StartCoroutine (ball1Dot.changeColor (ball1Dot.disabledColor));
			ball2Dot.StartCoroutine (ball2Dot.changeColor (ball2Dot.disabledColor));
			ball3Dot.StartCoroutine (ball3Dot.changeColor (ball3Dot.disabledColor));
			break;
		}
	}

	public void IncrementStrikes () {
		UIDotView strike1Dot = app.views.strike1Dot.GetComponent<UIDotView> ();
		UIDotView strike2Dot = app.views.strike2Dot.GetComponent<UIDotView> ();

		switch(app.model.currentGame.currentInning.currentAtBat.strikes) {
		case 0:
			Debug.Log ("strike 1!");
			app.model.currentGame.currentInning.currentAtBat.strikes++;
			strike1Dot.StartCoroutine (strike1Dot.changeColor (strike1Dot.strikeDotColor));
			break;
		case 1:
			Debug.Log ("strike 2!");
			app.model.currentGame.currentInning.currentAtBat.strikes++;
			strike2Dot.StartCoroutine (strike2Dot.changeColor (strike2Dot.strikeDotColor));
			break;
		case 2:
			Debug.Log ("strikeout!");
			ResetCount ();
			strike1Dot.StartCoroutine (strike1Dot.changeColor (strike1Dot.disabledColor));
			strike2Dot.StartCoroutine (strike2Dot.changeColor (strike2Dot.disabledColor));

			IncrementOuts ();
			break;
		}
	}

	public void IncrementOuts () {
		UIDotView out1Dot = app.views.out1Dot.GetComponent<UIDotView> ();
		UIDotView out2Dot = app.views.out2Dot.GetComponent<UIDotView> ();

		switch(app.model.currentGame.currentInning.outs) {
		case 0:
			Debug.Log ("1 out!");
			app.model.currentGame.currentInning.outs++;
			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.outDotColor));
			break;
		case 1:
			Debug.Log ("2 outs!");
			app.model.currentGame.currentInning.outs++;
			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.outDotColor));
			break;
		case 2:
			Debug.Log ("CHANGE!");
			ResetCount ();
			app.model.currentGame.currentInning.outs = 0;
			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.disabledColor));
			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.disabledColor));

			if (app.model.currentGame.currentInning.half == "top") {
				app.model.currentGame.currentInning.half = "bot";
			} else {
				app.model.currentGame.currentInning.half = "top";
				app.model.currentGame.currentInning.inningNumber++;
			}

			UpdateInningLabel ();

			break;
		}
	}
}
