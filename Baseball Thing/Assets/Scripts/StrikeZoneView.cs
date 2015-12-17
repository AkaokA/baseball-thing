using UnityEngine;
using System.Collections;

public class StrikeZoneView : MonoBehaviour {

	public int strikes = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {

		if (other.tag == "Baseball") {
			strikes++;
		}

	}

}
