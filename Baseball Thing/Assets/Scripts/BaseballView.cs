using UnityEngine;
using System.Collections;

public class BaseballView : MonoBehaviour {
	public float hitForceMin = 500;
	public float hitForceMax = 1200;
	public float hitForce;
	public Vector3 hitDirection = new Vector3(0f,0f,0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HitBaseball() {
		hitDirection = Random.onUnitSphere;
		hitDirection.x = Mathf.Abs(hitDirection.x);
		hitDirection.z = Mathf.Abs(hitDirection.z);

		hitForce = Random.Range (hitForceMin, hitForceMax);

		GetComponent<Rigidbody>().AddForce( hitDirection * hitForce);
	}

}