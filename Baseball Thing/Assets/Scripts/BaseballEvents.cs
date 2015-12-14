using UnityEngine;
using System.Collections;

public class BaseballEvents : MonoBehaviour {
	public float hitForce;
	public Vector3 hitDirection = new Vector3(0f,0f,0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space)) {
			HitBaseball ();
		}
	}

	public GameObject baseball;

	public void HitBaseball() {
		hitDirection = Random.onUnitSphere;
		hitDirection.x = Mathf.Abs(hitDirection.x);
		hitDirection.y = Mathf.Abs(hitDirection.y);
		hitDirection.z = Mathf.Abs(hitDirection.z);

		hitForce = Random.Range (500, 1800);

		GameObject baseballInstance = Instantiate (baseball);
		baseballInstance.GetComponent<Rigidbody>().AddForce( hitDirection * hitForce);
	}

}