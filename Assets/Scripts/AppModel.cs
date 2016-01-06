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

	public Base firstBase = new Base();
	public Base secondBase = new Base();
	public Base thirdBase = new Base();
	public Base homePlate = new Base();
}

public class Team {
	// gameplay variables
	public bool isHomeTeam;
	public string teamName;
	public Player[] players = new Player[9];

	// constructor
	public Team (string name, bool isHome) {
		teamName = name;
		isHomeTeam = isHome;

		float runningSpeed = 5f;

		// generate players
		for (int position = 1; position <= 9; position++) {
			Player playerInstance = new Player (position, runningSpeed);
			players [position-1] = playerInstance;
		}
	}
}

public class Player {
	// gameplay variables
	public GameObject fielderInstance;
	public GameObject runnerInstance;
	public Base currentBase;

	// player attributes
	public int fieldingPositionNumber; // 1 through 9
	public float runningSpeed;

	// constructor
	public Player ( int positionNumber , float speed) {
		fieldingPositionNumber = positionNumber;
		runningSpeed = speed;
	}
}

public class Inning {
	public int inningNumber = 0;
	public string half = "top"; // "top" or "bot"

	public int outs = 0;
	public int runsScored = 0;

	public bool ballIsInPlay;
	public bool currentPitchIsStrike = false;

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