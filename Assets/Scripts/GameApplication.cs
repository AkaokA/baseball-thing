﻿using UnityEngine;
using System.Collections;

public class BaseballElement : MonoBehaviour
{
	// Gives access to the application and all instances.
	public GameApplication app { get { return GameObject.FindObjectOfType<GameApplication>(); }}
}

public class GameApplication : MonoBehaviour
{
	// Reference to the root instances of the MVC.
	public AppModel model;
	public AppViews views;
	public GridViews gridViews;
	public AppController controller;

	// Init things here
	void Start() {
		
	}

	void Update () {

	}
}
