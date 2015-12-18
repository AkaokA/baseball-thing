using UnityEngine;
using System.Collections;

// Contains all data related to the app.
public class AppModel : BaseballElement {
	public BallGame currentGame = new BallGame();
}

public class BallGame {
	public int awayScore = 0;
	public int homeScore = 0;

	public Team awayTeam = new Team("Boston", false);
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
	public int position; // 1 through 9
}

public class Inning {
	public int inningNumber = 0;
	public string half = "top"; // "top" or "bot"

	public int outs = 0;
	public int runsScored = 0;

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
	public bool inPlay = false;
}

public class Base {
	public bool isOccupied = false;
}