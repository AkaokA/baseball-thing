﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class AppController : BaseballElement {
	public AnimationCurve easingCurve;

	public Ballpark ballpark;
	public BallGame currentGame;
	public Team fieldingTeam;
	public Team battingTeam;

	public GameObject currentBaseballInstance;
	public Player currentBatter;

	public bool betweenInnings = false;
	public bool playersAreOnTheField = false;

	public float minPitchSpeed = 8.5f;
	public float maxPitchSpeed = 20f;
	public float pitchAccuracy = 0.5f;
	public float throwSpeed = 15f;


	// Use this for initialization
	void Start () {
		app.views.mainMenu.SetActive (true);
		app.views.fieldCamera.GetComponent<Blur> ().enabled = false;
		app.views.duelGridCanvas.SetActive (false);
		app.views.scoreboard.SetActive (false);

		ballpark = new Ballpark ();
		ballpark.firstBase.baseGameObject = app.views.firstBase;
		ballpark.secondBase.baseGameObject = app.views.secondBase;
		ballpark.thirdBase.baseGameObject = app.views.thirdBase;
		ballpark.homePlate.baseGameObject = app.views.homePlate;

//		NewGame (); // DEBUG: automatically start a new game
//		ActivateDuelGrid (); // DEBUG: automatically go to duel grid
	}

	public void ActivateDuelGrid () {
		// create ballgame
		SetUpBallgame ();

		// create fielders
		SetUpFielders ();

		// create first batter
		NewBatter ();

		// blur camera
		app.views.fieldCamera.GetComponent<Blur> ().enabled = true;

		// hide main menu
		HideMainMenu ();

		// show grid canvas
		app.views.duelGridCanvas.SetActive (true);

		// show scoreboard
		app.scoreController.UpdateScoreboard ();
		app.views.scoreboard.SetActive (true);

		// Intro animations
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}

	public void NewGame () {
		// create ballgame
		SetUpBallgame ();

		// create fielders
		SetUpFielders ();

		// create first batter
		NewBatter ();

		// hide main menu
		HideMainMenu ();

		// show scoreboard
		app.views.scoreboard.SetActive (true);

		// Intro animations
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}

	public void HideMainMenu () {
		app.views.mainMenu.SetActive (false);
		app.views.duelGrid.SetActive (true);
	}

	// Update is called once per frame
	void Update () {

		if (currentBatter != null) {
			
//			// mouse/tap controls
//			if (Input.GetMouseButtonDown (0) && currentBatter != null) {
//				if (currentGame.currentInning.ballIsInPlay == false && currentBaseballInstance == null) {
//					currentBaseballInstance = Instantiate (app.views.baseball);
//					currentBaseballInstance.transform.parent = GameObject.Find ("Ballpark").transform;
//
//					float randomPitchSpeed = Random.Range (minPitchSpeed, maxPitchSpeed);
//					currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (randomPitchSpeed, pitchAccuracy);
//
//				} else {
//
//					if (currentBaseballInstance && currentGame.currentInning.ballIsInPlay == false) {
//						currentGame.currentInning.ballIsInPlay = true;
//						currentBaseballInstance.GetComponent<BaseballView> ().HitBaseball (currentBatter.hittingPower);
//						currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (true);
//						app.views.infieldCameraTrigger.SetActive (true);
//						currentBatter.runnerInstance.GetComponent<RunnerView> ().hadAnAtBat = true;
//
//						// advance all runners
//						foreach (Player runner in battingTeam.lineup) {
//							if (runner.runnerInstance) {
//								runner.runnerInstance.GetComponent<RunnerView> ().advanceToNextBase ();
//							}
//						}
//					} else {
//						ResetPlay ();
//						NewBatter ();
//					}
//
//				}
//			}


			// C: DEBUG: clear the field
			if (Input.GetKeyUp (KeyCode.C)) {
				StartCoroutine (ClearTheField ());
			}


			// ESCAPE: toggle main menu
			if (Input.GetKeyUp (KeyCode.Escape)) {

			}

			// P: throw pitch
			if (Input.GetKeyDown (KeyCode.P)) {
				if (currentGame.currentInning.ballIsInPlay == false) {
					currentBaseballInstance = Instantiate (app.views.baseball);
					currentBaseballInstance.transform.parent = GameObject.Find ("Ballpark").transform;

					float randomPitchSpeed = Random.Range (minPitchSpeed, maxPitchSpeed);
					currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (randomPitchSpeed, pitchAccuracy);
				}
			}

			if (currentBaseballInstance) {

				// SPACE: hit pitch
				if (Input.GetKeyDown (KeyCode.Space)) {
					currentGame.currentInning.ballIsInPlay = true;
					currentBaseballInstance.GetComponent<BaseballView> ().HitBaseball (currentBatter.hittingPower);
					currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (true);
					app.views.infieldCameraTrigger.SetActive (true);
					currentBatter.runnerInstance.GetComponent<RunnerView> ().hadAnAtBat = true;

					// advance all runners
					foreach (Player runner in battingTeam.lineup) {
						if (runner.runnerInstance) {
							runner.runnerInstance.GetComponent<RunnerView> ().advanceToNextBase ();
						}
					}
				}

				// BACKSPACE: reset gamestate (for ease of testing)
				if (Input.GetKeyDown (KeyCode.Backspace)) {
					ResetPlay ();
					NewBatter ();
				}

			}

		}

	}

	void SetUpBallgame () {
		// create game
		currentGame = new BallGame();
	
		// assign teams
		currentGame.homeTeam = new Team ("TOR", app.model.blueFemaleTexture, app.model.blueMaleTexture, true);
		currentGame.awayTeam = new Team ("BOS", app.model.redFemaleTexture, app.model.redMaleTexture, false);

		// home team on the field first
		fieldingTeam = currentGame.homeTeam;
		battingTeam = currentGame.awayTeam;

	}

	void SetUpFielders () {
		foreach (Player fielder in fieldingTeam.lineup) {
			// instantiate each fielder's gameobject
			fielder.fielderInstance = Instantiate (app.views.fielder);
			fielder.fielderInstance.transform.parent = GameObject.Find ("Players").transform;
			FielderView fielderView = fielder.fielderInstance.GetComponent<FielderView> ();

			// assign team texture to model
			switch (fielder.gender) {
			case "female":
				fielder.fielderInstance.GetComponentsInChildren<MeshFilter> () [0].mesh = app.model.femaleMesh;
				fielder.fielderInstance.GetComponentsInChildren<MeshRenderer> ()[0].material.mainTexture = fieldingTeam.teamTextureFemale;
				break;
			case "male":
				fielder.fielderInstance.GetComponentsInChildren<MeshFilter> () [0].mesh = app.model.maleMesh;
				fielder.fielderInstance.GetComponentsInChildren<MeshRenderer> () [0].material.mainTexture = fieldingTeam.teamTextureMale;
				break;
			}

			// assign positions and attributes from model
			fielderView.AssignFieldingPosition (fielder.fieldingPositionNumber);
			fielderView.maxSpeed = fielder.runningSpeed;
			fielderView.throwStrength = fielder.throwStrength;

			// put fielders in the dugout
			Vector3 randomizedStartPosition = fieldingTeam.dugoutPosition;
			randomizedStartPosition.x += Random.Range (-1, 0);
			randomizedStartPosition.z += Random.Range (-3, 3);
			fielder.fielderInstance.transform.position = randomizedStartPosition;

			// move fielders out to their positions
			fielderView.Idle ();
//			fielder.fielderInstance.transform.position = fielderView.idleLocation; // DEBUG: skip running out to the field
		}
		playersAreOnTheField = true;
	}
		
	public void NewBatter () {
		ResetCount ();

		// cycle lineup after 9th batter
		if (battingTeam.currentBatterIndex < 8) {
			battingTeam.currentBatterIndex++;			
		} else {
			battingTeam.currentBatterIndex = 0;
		}

		// create instance
		currentBatter = battingTeam.lineup [battingTeam.currentBatterIndex];
		currentBatter.runnerInstance = Instantiate (app.views.runner);
		currentBatter.runnerInstance.transform.parent = GameObject.Find ("Players").transform;

		currentBatter.runnerInstance.transform.position = battingTeam.dugoutPosition;
		RunnerView batterView = currentBatter.runnerInstance.GetComponent<RunnerView> ();
		batterView.batterIndex = battingTeam.currentBatterIndex;

		// assign team texture to model
		switch (currentBatter.gender) {
		case "female":
			currentBatter.runnerInstance.GetComponentsInChildren<MeshFilter> () [0].mesh = app.model.femaleMesh;
			currentBatter.runnerInstance.GetComponentsInChildren<MeshRenderer> ()[0].material.mainTexture = battingTeam.teamTextureFemale;
			break;
		case "male":
			currentBatter.runnerInstance.GetComponentsInChildren<MeshFilter> () [0].mesh = app.model.maleMesh;
			currentBatter.runnerInstance.GetComponentsInChildren<MeshRenderer> () [0].material.mainTexture = battingTeam.teamTextureMale;
			break;
		}

		// set runner attributes
		batterView.maxSpeed = currentBatter.runningSpeed;

		// walk up to the plate
		batterView.MoveToward (ballpark.leftBattersBox);
	}

	IEnumerator ClearTheField () {
		betweenInnings = true;
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("outfield", 1f);

		while (playersAreOnTheField) {
			playersAreOnTheField = false;
			foreach ( Player runner in battingTeam.lineup ) {
				if (runner.runnerInstance != null) {
					RunnerView runnerView = runner.runnerInstance.GetComponent<RunnerView> ();
					runnerView.MoveToward (battingTeam.dugoutPosition);
					playersAreOnTheField = true;

					if (runnerView.RunnerIsInDugout ()) {
						Destroy (runner.runnerInstance);
					}
				}
			}

			foreach (Player fielder in fieldingTeam.lineup) {
				if (fielder.fielderInstance != null) {
					FielderView fielderView = fielder.fielderInstance.GetComponent<FielderView> ();
					fielderView.MoveToward (fieldingTeam.dugoutPosition);
					playersAreOnTheField = true;

					if (fielderView.FielderIsInDugout ()) {
						Destroy (fielder.fielderInstance);
					}
				}
			}
			yield return null;
		}

		Debug.Log ("players are off the field.");
		ChangeSides ();
		betweenInnings = false;
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);

		SetUpFielders ();
		NewBatter ();
		yield return null;
	}

	public void ResetPlay () {
		Destroy (currentBaseballInstance);
		currentGame.currentInning.ballIsInPlay = false;
		app.views.infieldCameraTrigger.SetActive (false);
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.baseballLandingPoint.SetActive (false);

		foreach (Player player in fieldingTeam.lineup) {
			FielderView fielderView = player.fielderInstance.GetComponent<FielderView> ();
			fielderView.hasTheBall = false;
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
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 2f);
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
		app.views.fieldCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
	}

	public void ResetCount () {
		currentGame.currentInning.currentAtBat.balls = 0;
		currentGame.currentInning.currentAtBat.strikes = 0;
	}

	public void IncrementBalls () {
		switch(app.controller.currentGame.currentInning.currentAtBat.balls) {
		case 0:
			Debug.Log ("ball 1!");
			currentGame.currentInning.currentAtBat.balls++;
			break;
		case 1:
			Debug.Log ("ball 2!");
			currentGame.currentInning.currentAtBat.balls++;
			break;
		case 2:
			Debug.Log ("ball 3!");
			currentGame.currentInning.currentAtBat.balls++;
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
			break;
		case 1:
			Debug.Log ("strike 2!");
			currentGame.currentInning.currentAtBat.strikes++;
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
			break;
		case 1:
			Debug.Log ("2 outs!");
			currentGame.currentInning.outs++;
			break;
		case 2:
			Debug.Log ("CHANGE!");
			// reset B/S/O
			ResetCount ();
			currentGame.currentInning.outs = 0;

//			StartCoroutine (ClearTheField ());
			break;
		}

		app.scoreController.UpdateScoreboard ();
	}

	public void ChangeSides () {
		// switch batting/fielding teams and set inning indicator
		if (currentGame.currentInning.half == "top") {
			currentGame.currentInning.half = "bot";
			fieldingTeam = app.controller.currentGame.awayTeam;
			battingTeam = app.controller.currentGame.homeTeam;
		} else {
			currentGame.currentInning.half = "top";
			fieldingTeam = app.controller.currentGame.homeTeam;
			battingTeam = app.controller.currentGame.awayTeam;
			currentGame.currentInning.inningNumber++;
		}

		// update scoreboard
		app.scoreController.UpdateScoreboard ();
	}

	public void IncrementScore () {
		if ( battingTeam == currentGame.homeTeam ) {
			currentGame.homeTeam.score++;
		} else {
			currentGame.awayTeam.score++;
		}

		app.scoreController.UpdateScoreboard ();
	}
		
}
