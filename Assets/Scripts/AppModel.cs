using UnityEngine;
using System.Collections;

// Contains all data related to the app.
public class AppModel : BaseballElement {

}

public class BallGame {
	public int awayScore = 0;
	public int homeScore = 0;

	public Team awayTeam = new Team("Chicago", false);
	public Team homeTeam = new Team("Toronto", true);

	public Inning currentInning = new Inning(1);

	public Vector3 homeDugoutPosition = new Vector3 (-6f,0f,5f);
	public Vector3 awayDugoutPosition = new Vector3 (5f,0f,-6f);

	public Vector3 leftBattersBox = new Vector3 (-0.6f, 0f, 0.6f);
	public Vector3 rightBattersBox = new Vector3 (0.6f, 0f, -0.6f);

	public Base[] bases = new Base[4];
	public Base firstBase = new Base();
	public Base secondBase = new Base();
	public Base thirdBase = new Base();
	public Base homePlate = new Base();
}

public class Team {
	// gameplay variables
	public bool isHomeTeam;
	public string teamName;
	public int currentBatterNumber;

	// players
	public Player[] players = new Player[9];

	// constructor
	public Team (string name, bool isHome) {
		teamName = name;
		isHomeTeam = isHome;
		currentBatterNumber = 1;

		// temporary attributes for all players on team
		float runningSpeed = 5f;
		float throwStrength = 15f;

		// generate players
		for (int position = 1; position <= 9; position++) {
			Player playerInstance = new Player ("person " + position, position, runningSpeed, throwStrength);
			players [position-1] = playerInstance;
			// Debug.Log (teamName + " " + playerInstance.name);
		}
	}
}

public class Player {
	// gameplay variables
	public GameObject fielderInstance;
	public GameObject runnerInstance;
	public Base currentBase;

	// individual player attributes
	public string name;
	public int fieldingPositionNumber; // 1 through 9
	public float runningSpeed;
	public float throwStrength;

	public float pitchStrength;
	public float pitchAccuracy;

	// constructor
	public Player ( string playerName, int positionNumber , float speed, float throwing) {
		name = playerName;
		fieldingPositionNumber = positionNumber;
		runningSpeed = speed;
		throwStrength = throwing;

		// pitcher attributes; not used yet
		if (positionNumber == 1)	 {
			pitchStrength = 20f;
			pitchAccuracy = 0.5f;
		}
	}
}

public class Inning {
	public int inningNumber = 0;
	public string half = "top"; // "top" or "bot"

	public int outs = 0;
	public int runsScored = 0;

	public bool ballIsInPlay;
	public bool currentPitchIsStrike = false;

	// constructor
	public Inning(int number) {
		inningNumber = number;
		ballIsInPlay = false;
	}

	public AtBat currentAtBat = new AtBat();
}

public class AtBat {
	public int balls = 0;
	public int strikes = 0;
}

public class Base {
	public GameObject baseGameObject;
	public bool isOccupied = false;
}