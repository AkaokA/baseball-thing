using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	public GameObject Baseball;
	private GameObject StrikeZone;
	private GameObject MainCamera;
	private GameObject Sun;

	private GameObject currentBaseballInstance;
	private bool allowCameraMovement = true;

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

		// P: throw pitch
		if (Input.GetKeyDown (KeyCode.P)) {
			currentBaseballInstance = Instantiate (Baseball);
			currentBaseballInstance.GetComponent<BaseballView>().PitchBaseballWithSpeed (StrikeZone.transform, Random.Range (9f, 18f), 0.8f);
			allowCameraMovement = true;
		}

		// SPACE: hit pitch
		if (Input.GetKeyDown (KeyCode.Space)) {
			currentBaseballInstance.GetComponent<BaseballView>().HitBaseball ();
		}

		// go back to infield camera when latest ball slows down
		if (currentBaseballInstance && allowCameraMovement) {
			float currentBaseballSpeed = currentBaseballInstance.GetComponent<Rigidbody> ().velocity.magnitude;
			
			if ( currentBaseballSpeed < 1f) {
				MainCamera.GetComponent<CameraView>().MoveCamera ("infield", 3f);
				allowCameraMovement = false;
			}
		}

	}
}
