using UnityEngine;
using System.Collections;

public class AppController : BaseballElement {

	public BallGame currentGame;
	public Inning currentInning;

	public GameObject Baseball;
	public GameObject currentBaseballInstance;

	public float minPitchSpeed = 8.5f;
	public float maxPitchSpeed = 20f;
	public float pitchAccuracy = 0.5f;

	private Vector3 homeDugoutPosition = new Vector3 (-6f,0f,5f);
	private Vector3 awayDugoutPosition = new Vector3 (5f,0f,-6f);

	private UIDotView ball1Dot;
	private UIDotView ball2Dot;
	private UIDotView ball3Dot;
	private UIDotView strike1Dot;
	private UIDotView strike2Dot;
	private UIDotView out1Dot;
	private UIDotView out2Dot;

	// Use this for initialization
	void Start () {
		// create teams
		currentGame = new BallGame();

		foreach (Player player in currentGame.homeTeam.players) {
			// instantiate each fielder's gameobject
			player.fielderInstance = Instantiate (app.views.fielder);

			// put fielders in the dugout
			Vector3 randomizedStartPosition = app.controller.homeDugoutPosition;
			randomizedStartPosition.x += Random.Range (-8, 0);
			randomizedStartPosition.z += Random.Range (-3, 3);
			player.fielderInstance.transform.position = randomizedStartPosition;

			player.fielderInstance.GetComponent<FielderView> ().UpdateTargetPosition (player.idleLocation);
		}

		// init variables
		ball1Dot = app.views.ball1Dot.GetComponent<UIDotView> ();
		ball2Dot = app.views.ball2Dot.GetComponent<UIDotView> ();
		ball3Dot = app.views.ball3Dot.GetComponent<UIDotView> ();
		strike1Dot = app.views.strike1Dot.GetComponent<UIDotView> ();
		strike2Dot = app.views.strike2Dot.GetComponent<UIDotView> ();
		out1Dot = app.views.out1Dot.GetComponent<UIDotView> ();
		out2Dot = app.views.out2Dot.GetComponent<UIDotView> ();

		// draw scoreboard
		UpdateScoreboard ();

		// Intro animations
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.sun.GetComponent<SunView> ().BeginSunrise ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// fielder AI
		foreach (Player fielder in currentGame.homeTeam.players) {
			FielderView fielderView = fielder.fielderInstance.GetComponent<FielderView> ();

			// while the ball is in play
			if (app.controller.currentGame.currentInning.ballIsInPlay) {
				Vector3 ballPosition = app.controller.currentBaseballInstance.transform.position;
				Vector3 distanceToBall = ballPosition - fielder.fielderInstance.transform.position;

				if (fielderView.hasTheBall ) {
					Debug.Log (fielder.fieldingPosition);
					currentBaseballInstance.GetComponent<Rigidbody> ().velocity = new Vector3(0,0,0);
					fielderView.UpdateTargetPosition (ballPosition);
				} else {
					if (distanceToBall.magnitude < 4f) {
						// follow ball lol
						Vector3 targetPosition = ballPosition + (currentBaseballInstance.GetComponent<Rigidbody> ().velocity /4);

						fielderView.UpdateTargetPosition (targetPosition);
					} else {
						// go back to idle position
						fielderView.UpdateTargetPosition (fielder.idleLocation);
					}
				}
			} else {
				// go back to idle position
				fielderView.UpdateTargetPosition (fielder.idleLocation);
			}
		}
			
		// P: throw pitch
		if (Input.GetKeyDown (KeyCode.P)) {
			if (app.controller.currentGame.currentInning.ballIsInPlay == false) {
				currentBaseballInstance = Instantiate (Baseball);

				float randomPitchSpeed = Random.Range (minPitchSpeed, maxPitchSpeed);
				currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (app.views.strikeZone.transform, randomPitchSpeed, pitchAccuracy);
			}
		}

		if (currentBaseballInstance) {

			// SPACE: hit pitch
			if (Input.GetKeyDown (KeyCode.Space)) {
				currentBaseballInstance.GetComponent<BaseballView> ().HitBaseball ();
				currentBaseballInstance.GetComponent<BaseballView> ().heightIndicator.SetActive (true);
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
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				currentBaseballInstance.GetComponent<BaseballView> ().ThrowBaseballAt (app.views.homePlate.transform);
			}

			// ESC: reset gamestate
			if (Input.GetKeyDown (KeyCode.Escape)) {
				ResetPlay ();
			}
		}
	}

	public void ResetPlay () {
		Destroy (currentBaseballInstance);
		currentGame.currentInning.ballIsInPlay = false;
		app.views.mainCamera.GetComponent<CameraView>().ChangeCameraState ("infield", 1f);
		app.views.baseballLandingPoint.SetActive (false);
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
		
	void ResetCount () {
		currentGame.currentInning.currentAtBat.balls = 0;
		currentGame.currentInning.currentAtBat.strikes = 0;
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
			ResetCountUI ();
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
			ResetCountUI ();

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
			ResetCount ();
			ResetCountUI ();
			currentGame.currentInning.outs = 0;
			out1Dot.StartCoroutine (out1Dot.changeColor (out1Dot.disabledColor));
			out2Dot.StartCoroutine (out2Dot.changeColor (out2Dot.disabledColor));

			if (currentGame.currentInning.half == "top") {
				currentGame.currentInning.half = "bot";
			} else {
				currentGame.currentInning.half = "top";
				currentGame.currentInning.inningNumber++;
			}

			UpdateInningLabel ();
			break;
		}
	}
}
