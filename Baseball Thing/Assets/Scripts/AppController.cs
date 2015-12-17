using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	public GameObject Baseball;
	private GameObject StrikeZone;
	private GameObject MainCamera;
	private GameObject Sun;

	// Use this for initialization
	void Start () {
		MainCamera = GameObject.Find ("Main Camera");
		Sun = GameObject.Find ("Sun");
		StrikeZone = GameObject.Find ("Strike Zone");

		// Intro animations
		MainCamera.GetComponent<CameraView>().MoveCamera ("infield", 3f);
		Sun.GetComponent<SunView> ().BeginSunrise ();
	}
	
	// Update is called once per frame
	void Update () {

		// create and launch a baseball when Space is pressed
		if (Input.GetKeyDown (KeyCode.Space)) {
//			GameObject baseballInstance = Instantiate (Baseball);
//			baseballInstance.GetComponent<BaseballView>().HitBaseball ();


		}

		if (Input.GetKeyDown (KeyCode.P)) {
			MainCamera.GetComponent<CameraView>().MoveCamera ("infield", 0.3f);

			GameObject baseballInstance = Instantiate (Baseball);
			baseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (StrikeZone.transform, Random.Range (9f, 18f), 0.8f);
		}
	}
}
