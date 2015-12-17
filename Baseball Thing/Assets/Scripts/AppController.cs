using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	public GameObject Baseball;
	private GameObject StrikeZone;

	// Use this for initialization
	void Start () {

		// Intro animations
		GameObject.Find ("Main Camera").GetComponent<CameraView>().MoveCamera ("infield", 3f);
		GameObject.Find ("Sun").GetComponent<SunView> ().BeginSunrise ();
		StrikeZone = GameObject.Find ("Strike Zone");
	}
	
	// Update is called once per frame
	void Update () {

		// create and launch a baseball when Space is pressed
		if (Input.GetKeyDown (KeyCode.Space)) {
//			GameObject baseballInstance = Instantiate (Baseball);
//			baseballInstance.GetComponent<BaseballView>().HitBaseball ();


		}

		if (Input.GetKeyDown (KeyCode.P)) {
			GameObject baseballInstance = Instantiate (Baseball);
			baseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (StrikeZone.transform, Random.Range (9f, 18f));
		}
	}
}
