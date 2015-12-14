using UnityEngine;
using System.Collections;

public class BaseballEvents : MonoBehaviour {
	public float hitForce = 500f;
	public Vector3 hitDirection = new Vector3(0f,0.5f,0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown() {
		GetComponent<Rigidbody>().AddForce( hitDirection.normalized * hitForce);
	}

}