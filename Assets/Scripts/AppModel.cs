﻿using UnityEngine;
using System.Collections;

// Contains all data related to the app.
public class AppModel : BaseballElement {

}

public class Ballpark {
	public Vector3 leftBattersBox = new Vector3 (-0.6f, 0f, 0.6f);
	public Vector3 rightBattersBox = new Vector3 (0.6f, 0f, -0.6f);

	public Base[] bases = new Base[4];
	public Base firstBase = new Base();
	public Base secondBase = new Base();
	public Base thirdBase = new Base();
	public Base homePlate = new Base();

	// constructor
	public Ballpark () {
		// set up base instances
		bases [0] = homePlate;
		bases [1] = firstBase;
		bases [2] = secondBase;
		bases [3] = thirdBase;
	}
}

public class BallGame {
	public Team awayTeam;
	public Team homeTeam;

	public Inning currentInning = new Inning(1);
}

public class Team {
	// gameplay variables
	public int score = 0;

	public bool isHomeTeam;
	public string teamName;
	public int currentBatterIndex;
	public Vector3 dugoutPosition;

	// players
	public Player[] lineup = new Player[9];

	// constructor
	public Team (string name, bool isHome) {
		teamName = name;
		isHomeTeam = isHome;
		currentBatterIndex = -1; // NewBatter() increments this; first batter should be index 0

		// hard-coded dugout locations
		Vector3 homeDugoutPosition = new Vector3 (-6f,0f,5f);
		Vector3 awayDugoutPosition = new Vector3 (5f,0f,-6f);

		if (isHomeTeam) {
			dugoutPosition = homeDugoutPosition;
		} else {
			dugoutPosition = awayDugoutPosition;
		}

		// temporary attributes for all players on team
		float throwStrength = 15f;

		// generate players
		for (int position = 1; position <= 9; position++) {
			float runningSpeed = Random.Range (4f, 6f);
			Player playerInstance = new Player ("person " + position, position, runningSpeed, throwStrength);
			lineup [position-1] = playerInstance;
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
	public float hittingPower;

	public float pitchStrength;
	public float pitchAccuracy;

	// constructor
	public Player ( string playerName, int positionNumber , float speed, float throwing) {
		name = playerName;
		fieldingPositionNumber = positionNumber;
		runningSpeed = speed;
		throwStrength = throwing;
		hittingPower = 1200f;
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