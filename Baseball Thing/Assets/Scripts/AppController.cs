using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>().MoveCamera ("infield", 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
