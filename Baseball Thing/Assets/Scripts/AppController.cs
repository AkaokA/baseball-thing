using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	public GameObject baseball;

	// Use this for initialization
	void Start () {

		// Intro animations
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>().MoveCamera ("infield", 3f);
		GameObject.Find ("Sun").GetComponent<SunView> ().BeginSunrise ();
	}
	
	// Update is called once per frame
	void Update () {

		// create and launch a baseball when Space is pressed
		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject baseballInstance = Instantiate (baseball);
			baseballInstance.GetComponent<BaseballView>().HitBaseball ();
		}

	}
}
