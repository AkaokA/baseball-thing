﻿using UnityEngine;
using System.Collections;

public class AppController : BaseballElement {
	
	public BallGame currentGame;
	public Team fieldingTeam;
	public Team battingTeam;

	public GameObject currentBaseballInstance;
	public Player currentBatter;

	public float minPitchSpeed = 8.5f;
	public float maxPitchSpeed = 20f;
	public float pitchAccuracy = 0.5f;
	public float throwSpeed = 15f;

	private UIDotView ball1Dot;
	private UIDotView ball2Dot;
	private UIDotView ball3Dot;
	private UIDotView strike1Dot;
	private UIDotView strike2Dot;
	private UIDotView out1Dot;
	private UIDotView out2Dot;

	// Use this for initialization
	void Start () {
		// init scoreboard variables (scoreboard stuff should go in its own class)
		ball1Dot = app.views.ball1Dot.GetComponent<UIDotView> ();
		ball2Dot = app.views.ball2Dot.GetComponent<UIDotView> ();
		ball3Dot = app.views.ball3Dot.GetComponent<UIDotView> ();
		strike1Dot = app.views.strike1Dot.GetComponent<UIDotView> ();
		strike2Dot = app.views.strike2Dot.GetComponent<UIDotView> ();
		out1Dot = app.views.out1Dot.GetComponent<UIDotView> ();
		out2Dot = app.views.out2Dot.GetComponent<UIDotView> ();

		// create ballgame
		SetUpBallgame ();

		// draw scoreboard
		UpdateScoreboard ();

		// create fielders
		SetUpFielders ();

		// create first batter
		NewBatter ();

		// Intro animations
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("atbat", 2f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}
	
	// Update is called once per frame
	void Update () {
					
		// P: throw pitch
		if (Input.GetKeyDown (KeyCode.P)) {
			if (app.controller.currentGame.currentInning.ballIsInPlay == false) {
				currentBaseballInstance = Instantiate (app.views.baseball);

				float randomPitchSpeed = Random.Range (minPitchSpeed, maxPitchSpeed);
				currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (app.views.strikeZone.transform, randomPitchSpeed, pitchAccuracy);
			}
		}

		if (currentBaseballInstance) {

			// SPACE: hit pitch
			if (Input.GetKeyDown (KeyCode.Space)) {
				currentGame.currentInning.ballIsInPlay = true;
				currentBaseballInstance.GetComponent<BaseballView> ().HitBaseball (currentBatter.hittingPower);
				currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (true);
				app.views.infieldCameraTrigger.SetActive (true);

				// advance all runners
				foreach (Player runner in battingTeam.lineup) {
					if (runner.runnerInstance) {
						runner.runnerInstance.GetComponent<RunnerView> ().advanceToNextBase ();
					}
				}
			}

			// ESC: reset gamestate (for ease of testing)
			if (Input.GetKeyDown (KeyCode.Escape)) {
				ResetPlay ();
				NewBatter ();
			}
		}
	}

	void SetUpBallgame () {
		// create game
		currentGame = new BallGame();
	
		// set up base instances
		currentGame.bases [0] = currentGame.homePlate;
		currentGame.bases [1] = currentGame.firstBase;
		currentGame.bases [2] = currentGame.secondBase;
		currentGame.bases [3] = currentGame.thirdBase;

		currentGame.firstBase.baseGameObject = app.views.firstBase;
		currentGame.secondBase.baseGameObject = app.views.secondBase;
		currentGame.thirdBase.baseGameObject = app.views.thirdBase;
		currentGame.homePlate.baseGameObject = app.views.homePlate;

		// assign teams
		currentGame.homeTeam = new Team ("Toronto", true);
		currentGame.awayTeam = new Team("Chicago", false);

		fieldingTeam = currentGame.homeTeam;
		battingTeam = currentGame.awayTeam;
	}

	void SetUpFielders () {
		foreach (Player player in fieldingTeam.lineup) {
			// instantiate each fielder's gameobject
			player.fielderInstance = Instantiate (app.views.fielder);
			FielderView fielderView = player.fielderInstance.GetComponent<FielderView> ();

			// assign positions and attributes from model
			fielderView.AssignFieldingPosition (player.fieldingPositionNumber);
			fielderView.maxSpeed = player.runningSpeed;
			fielderView.throwStrength = player.throwStrength;

			// put fielders in the dugout
			Vector3 randomizedStartPosition = fieldingTeam.dugoutPosition;
			randomizedStartPosition.x += Random.Range (-8, 0);
			randomizedStartPosition.z += Random.Range (-3, 3);
			player.fielderInstance.transform.position = randomizedStartPosition;

			// move fielders out to their positions
			fielderView.Idle ();
		}
	}

	public void NewBatter () {
		// cycle lineup after 9th batter
		if (battingTeam.currentBatterNumber < 9) {
			battingTeam.currentBatterNumber++;			
		} else {
			battingTeam.currentBatterNumber = 1;
		}

		currentBatter = battingTeam.lineup [battingTeam.currentBatterNumber - 1];

		currentBatter.runnerInstance = Instantiate (app.views.runner);
		currentBatter.runnerInstance.transform.position = battingTeam.dugoutPosition;
		RunnerView batterView = currentBatter.runnerInstance.GetComponent<RunnerView> ();

		batterView.batterIndex = battingTeam.currentBatterNumber;
		batterView.maxSpeed = currentBatter.runningSpeed;

		batterView.MoveToward (currentGame.leftBattersBox);

		ResetCount ();
	}

	public void ResetPlay () {
		Destroy (currentBaseballInstance);
		currentGame.currentInning.ballIsInPlay = false;
		app.views.infieldCameraTrigger.SetActive (false);
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("atbat", 1f);
		app.views.baseballLandingPoint.SetActive (false);

		if ( currentBaseballInstance != null ) {
			currentBaseballInstance.GetComponent<BaseballView> ().ballIsRolling = false;
		}
			
		if (battingTeam.currentBatterNumber < 9) {
			battingTeam.currentBatterNumber++;
		} else {
			battingTeam.currentBatterNumber = 1;
		}
	}

	public void RegisterPitch () {
		if (currentGame.currentInning.currentPitchIsStrike) {
			PitchOutcome ("strike");
		} else {
			PitchOutcome ("ball");				
		}
		Destroy (currentBaseballInstance);
		currentGame.currentInning.currentPitchIsStrike = false;
	}

	public void PitchOutcome (string pitchOutcome) {
		switch(pitchOutcome) {
		case "ball":
			IncrementBalls ();
			break;
		case "strike":
			IncrementStrikes ();
			break;
		case "foul":
			Debug.Log ("foul!");
			if (currentGame.currentInning.currentAtBat.strikes < 2) {
				IncrementStrikes ();				
			}
			break;
		case "in play":
			Debug.Log ("in play!");
			break;
		}
	}

	public void HomeRun () {
		ResetPlay ();
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 2f);
		StartCoroutine (CircleTheBases ());
	}

	IEnumerator CircleTheBases () {
		bool runnersOnBase = true;
		while (runnersOnBase) {
			runnersOnBase = false;
			foreach ( Player runner in battingTeam.lineup ) {
				if ( runner.runnerInstance != null ) {
					runner.runnerInstance.GetComponent<RunnerView> ().advanceToNextBase ();
					runnersOnBase = true;
				}
			}
			yield return null;
		}
		NewBatter ();
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("atbat", 1f);
	}
		
	void ResetCount () {
		currentGame.currentInning.currentAtBat.balls = 0;
		currentGame.currentInning.currentAtBat.strikes = 0;
		ResetCountUI ();
	}

	void ResetCountUI () {
		ball1Dot.StartCoroutine (ball1Dot.changeColor (ball1Dot.disabledColor));
		ball2Dot.StartCoroutine (ball2Dot.changeColor (ball2Dot.disabledColor));
		ball3Dot.StartCoroutine (ball3Dot.changeColor (ball3Dot.disabledColor));
		strike1Dot.StartCoroutine (strike1Dot.changeColor (strike1Dot.disabledColor));
		strike2Dot.StartCoroutine (strike2Dot.changeColor (strike2Dot.disabledColor));
	}

	public void UpdateScoreboard () {
		// text labels
		app.views.awayTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.awayTeam.teamName.ToUpper ();
		app.views.homeTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.homeTeam.teamName.ToUpper ();
		app.views.awayScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.awayScore.ToString ();
		app.views.homeScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.homeScore.ToString ();
		UpdateInningLabel ();
	}

	public void UpdateInningLabel () {
		app.views.inningLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.currentInning.half.ToUpper () + " " + currentGame.currentInning.inningNumber.ToString ();
	}

	public void IncrementBalls () {
		switch(app.controller.currentGame.currentInning.currentAtBat.balls) {
		case 0:
			Debug.Log ("ball 1!");
			currentGame.currentInning.currentAtBat.balls++;
			ball1Dot.StartCoroutine (ball1Dot.changeColor (ball1Dot.ballDotColor));
			break;
		case 1:
			Debug.Log ("ball 2!");
			currentGame.currentInning.currentAtBat.balls++;
			ball2Dot.StartCoroutine (ball2Dot.changeColor (ball2Dot.ballDotColor));
			break;
		case 2:
			Debug.Log ("ball 3!");
			currentGame.currentInning.currentAtBat.balls++;
			ball3Dot.StartCoroutine (ball3Dot.changeColor (ball3Dot.ballDotColor));
			break;
		case 3:
			Debug.Log ("walk!");
			ResetCount ();
			break;
		}
	}

	public void IncrementStrikes () {

		switch(currentGame.currentInning.currentAtBat.strikes) {
		case 0:
			Debug.Log ("strike 1!");
			currentGame.currentInning.currentAtBat.strikes++;
			strike1Dot.StartCoroutine (strike1Dot.changeColor (strike1Dot.strikeDotColor));
			break;
		case 1:
			Debug.Log ("strike 2!");
			currentGame.currentInning.currentAtBat.strikes++;
			strike2Dot.StartCoroutine (strike2Dot.changeColor (strike2Dot.strikeDotColor));
			break;
		case 2:
			Debug.Log ("strikeout!");
			ResetCount ();
			IncrementOuts ();
			break;
		}
	}

	public void IncrementOuts () {
		switch(currentGame.currentInning.outs) {
		case 0:
			Debug.Log ("1 out!");
			currentGame.currentInning.outs++;
			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.outDotColor));
			break;
		case 1:
			Debug.Log ("2 outs!");
			currentGame.currentInning.outs++;
			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.outDotColor));
			break;
//		case 2:
//			Debug.Log ("CHANGE!");
//			ResetCount ();
//			currentGame.currentInning.outs = 0;
//			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.disabledColor));
//			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.disabledColor));
//
//			if (currentGame.currentInning.half == "top") {
//				currentGame.currentInning.half = "bot";
//				fieldingTeam = app.controller.currentGame.awayTeam;
//				battingTeam = app.controller.currentGame.homeTeam;
//			} else {
//				currentGame.currentInning.half = "top";
//				fieldingTeam = app.controller.currentGame.homeTeam;
//				battingTeam = app.controller.currentGame.awayTeam;
//				currentGame.currentInning.inningNumber++;
//			}
//
//			UpdateInningLabel ();
//			break;
		}
	}

	public void IncrementScore () {
		if ( battingTeam == currentGame.homeTeam ) {
			currentGame.homeScore++;
		} else {
			currentGame.awayScore++;
		}

		UpdateScoreboard ();
	}
}