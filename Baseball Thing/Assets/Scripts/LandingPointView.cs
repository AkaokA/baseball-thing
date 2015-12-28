﻿using UnityEngine;
using System.Collections;

public class LandingPointView : BaseballElement {

	public float yOffset = 0.005f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector3 tempPosition = transform.position;
		tempPosition.y = yOffset;
		transform.position = tempPosition;
	}

}
