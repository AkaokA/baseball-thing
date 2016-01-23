using UnityEngine;
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

		ballpark = new Ballpark ();
		ballpark.firstBase.baseGameObject = app.views.firstBase;
		ballpark.secondBase.baseGameObject = app.views.secondBase;
		ballpark.thirdBase.baseGameObject = app.views.thirdBase;
		ballpark.homePlate.baseGameObject = app.views.homePlate;

//		NewGame (); // DEBUG: automatically start a new game
	}

	public void ActivateGrid () {
		// blur camera
		Camera.main.GetComponent<Blur> ().enabled = true;

		// hide main menu
		StartCoroutine ( HideMainMenu (app.views.mainMenu) );

		// show grid canvas
		app.views.duelGridCanvas.SetActive (true);

		// Intro animations
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}

	public void NewGame () {
		// create ballgame
		SetUpBallgame ();

		// show scoreboard
		ShowUI (app.views.scoreboard);

		// update scoreboard
		UpdateScoreboard ();

		// create fielders
		SetUpFielders ();

		// create first batter
		NewBatter ();

		// hide main menu
		StartCoroutine ( HideMainMenu (app.views.mainMenu) );

		// Intro animations
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}

	public void ShowUI (GameObject guiCanvas) {
		if (guiCanvas.activeInHierarchy == false) {
			guiCanvas.SetActive (true);
		}
	}

	public IEnumerator HideMainMenu (GameObject guiCanvas) {
		if (guiCanvas.activeInHierarchy == true) {
			float time = 0.5f;
			float initialYAngle = 0f;
			float finalYAngle = -112f;
				
			float currentLerpTime;

			for ( currentLerpTime = 0f; currentLerpTime <= time; currentLerpTime += Time.deltaTime ) {

				foreach (GameObject menuElement in GameObject.FindGameObjectsWithTag ("Menu Element")) {
					float perc = currentLerpTime / time;
					float angle;
					angle = Mathf.LerpUnclamped(initialYAngle, finalYAngle, easingCurve.Evaluate (perc));

					Quaternion newRotation = menuElement.transform.rotation;
					Vector3 newEulerAngles = newRotation.eulerAngles;
					newEulerAngles.y = angle;
					newRotation.eulerAngles = newEulerAngles;
					menuElement.transform.rotation = newRotation;

				}
				yield return null;
			}

			guiCanvas.SetActive (false);
			app.views.duelGrid.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update () {

		if (currentBatter != null) {
			
			// mouse/tap controls
			if (Input.GetMouseButtonDown (0) && currentBatter != null) {
				if (currentGame.currentInning.ballIsInPlay == false && currentBaseballInstance == null) {
					currentBaseballInstance = Instantiate (app.views.baseball);
					currentBaseballInstance.transform.parent = GameObject.Find ("Ballpark").transform;

					float randomPitchSpeed = Random.Range (minPitchSpeed, maxPitchSpeed);
					currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (app.views.strikeZone.transform, randomPitchSpeed, pitchAccuracy);

				} else {

					if (currentBaseballInstance && currentGame.currentInning.ballIsInPlay == false) {
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
					} else {
						ResetPlay ();
						NewBatter ();
					}

				}
			}


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
		currentGame.homeTeam = new Team ("Toronto", app.model.blueFemaleTexture, app.model.blueMaleTexture, true);
		currentGame.awayTeam = new Team ("Boston", app.model.redFemaleTexture, app.model.redMaleTexture, false);

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
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("outfield", 1f);

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
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);

		SetUpFielders ();
		NewBatter ();
		yield return null;
	}

	public void ResetPlay () {
		Destroy (currentBaseballInstance);
		currentGame.currentInning.ballIsInPlay = false;
		app.views.infieldCameraTrigger.SetActive (false);
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
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
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
	}

	void ResetCount () {
		currentGame.currentInning.currentAtBat.balls = 0;
		currentGame.currentInning.currentAtBat.strikes = 0;

		// update scoreboard count
		ball1Dot.StartCoroutine (ball1Dot.changeColor (ball1Dot.disabledColor));
		ball2Dot.StartCoroutine (ball2Dot.changeColor (ball2Dot.disabledColor));
		ball3Dot.StartCoroutine (ball3Dot.changeColor (ball3Dot.disabledColor));
		strike1Dot.StartCoroutine (strike1Dot.changeColor (strike1Dot.disabledColor));
		strike2Dot.StartCoroutine (strike2Dot.changeColor (strike2Dot.disabledColor));
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
		case 2:
			Debug.Log ("CHANGE!");
			// reset B/S/O
			ResetCount ();
			currentGame.currentInning.outs = 0;
			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.disabledColor));
			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.disabledColor));

			StartCoroutine (ClearTheField ());
			break;
		}
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
		UpdateScoreboard ();
	}

	public void IncrementScore () {
		if ( battingTeam == currentGame.homeTeam ) {
			currentGame.homeTeam.score++;
		} else {
			currentGame.awayTeam.score++;
		}

		UpdateScoreboard ();
	}

	public void UpdateScoreboard () {
		// text labels
		app.views.awayTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.awayTeam.teamName.ToUpper ();
		app.views.homeTeamNameLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.homeTeam.teamName.ToUpper ();
		app.views.awayScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.awayTeam.score.ToString ();
		app.views.homeScoreLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.homeTeam.score.ToString ();
		app.views.inningLabel.GetComponent<UnityEngine.UI.Text> ().text = currentGame.currentInning.half.ToUpper () + " " + currentGame.currentInning.inningNumber.ToString ();
	}
		
}
