using UnityEngine;
using System.Collections;

public class HeightMarker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempPosition = transform.localPosition;
		tempPosition.y = -transform.parent.position.y*2 + 0.002f;
		transform.localPosition = tempPosition;

		Quaternion tempRotation = transform.rotation;
		tempRotation = Quaternion.Euler(90, 0, 0);
		transform.rotation = tempRotation;
	}
}
