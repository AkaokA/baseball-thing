using UnityEngine;
using System.Collections;

public class AppViews : BaseballElement {

	public GameObject fieldCamera;
	public GameObject duelCamera;
	public GameObject sun;

	public GameObject baseball;

	public GameObject fielder;
	public GameObject runner;

	public GameObject firstBase;
	public GameObject secondBase;
	public GameObject thirdBase;
	public GameObject homePlate;

//	public GameObject strikeZone;
	public GameObject pitchRegister;
	public GameObject baseballLandingPoint;
	public GameObject infieldCameraTrigger;

	// UI & Scoreboard
	public GameObject mainMenu;

	public GameObject scoreboard;
	public GameObject scoreboardBases;
	public GameObject scoreboardBalls;
	public GameObject scoreboardStrikes;
	public GameObject scoreboardOuts;

	public GameObject awayNameLabel;
	public GameObject homeNameLabel;
	public GameObject awayScoreLabel;
	public GameObject homeScoreLabel;
	public GameObject inningLabel;

	// Duel Grid
	public GameObject duelGridCanvas;
	public GameObject duelGrid;
	public GameObject duelGridCell;

	public GameObject duelPitchMarker;
	public GameObject duelPitchEndMarker;
	public GameObject duelSwingMarker;

	public GameObject duelBatterPhase1;
	public GameObject duelPitcherPhase1;
	public GameObject duelBatterPhase2;
	public GameObject duelOutcomePhase;

	public GameObject pitchInventory;
}
