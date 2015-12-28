using UnityEngine;
using System.Collections;

// Contains all data related to the app.
public class AppModel : BaseballElement {
	public BallGame currentGame = new BallGame();

	public Vector3 homeDugoutPosition = new Vector3 (-6f,0f,5f);
	public Vector3 awayDugoutPosition = new Vector3 (5f,0f,-6f);
}

public class BallGame {
	public int awayScore = 0;
	public int homeScore = 0;

	public Team awayTeam = new Team("Chicago", false);
	public Team homeTeam = new Team("Toronto", true);

	public Inning currentInning = new Inning(1);

}

public class Team {
	public bool isHomeTeam;
	public string teamName;

	public Team (string name, bool isHome) {
		teamName = name;
		isHomeTeam = isHome;
	}

	public Player[] players = new Player[9];
}

public class Player {
	public int fieldingPosition; // 1 through 9
	public Vector3 idleLocation;

	public float runningSpeed;

	public Player ( int positionNumber ) {
		fieldingPosition = positionNumber;
		runningSpeed = 6f;

		switch (fieldingPosition) {
		case 1: // pitcher
			idleLocation = new Vector3 (5f, 0f, 5f);
			break;
		case 2: // catcher
			idleLocation = new Vector3 (-1f, 0f, -1f);
			break;
		case 3: // 1st base
			idleLocation = new Vector3 (12.5f, 0f, 2.5f);
			break;
		case 4: // 2nd base
			idleLocation = new Vector3 (12.5f, 0f, 8f);
			break;
		case 5: // 3rd base
			idleLocation = new Vector3 (2.5f, 0f, 12.5f);
			break;
		case 6: // shortstop
			idleLocation = new Vector3 (8f, 0f, 12.5f);
			break;
		case 7: // left field
			idleLocation = new Vector3 (7f, 0f, 25f);
			break;
		case 8: // center field
			idleLocation = new Vector3 (20f, 0f, 20f);
			break;
		case 9: // right field
			idleLocation = new Vector3 (25f, 0f, 7f);
			break;
		}

	}
}

public class Inning {
	public int inningNumber = 0;
	public string half = "top"; // "top" or "bot"

	public int outs = 0;
	public int runsScored = 0;

	public bool ballIsInPlay = false;
	public bool currentPitchIsStrike = false;

	public Base firstBase;
	public Base secondBase;
	public Base thirdBase;

	public Inning(int number) {
		inningNumber = number;
	}

	public AtBat currentAtBat = new AtBat();
}

public class AtBat {
	public int balls = 0;
	public int strikes = 0;
}

public class Base {
	public bool isOccupied = false;
}