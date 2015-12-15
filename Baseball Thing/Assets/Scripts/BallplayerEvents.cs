using UnityEngine;
using System.Collections;

public class BallplayerEvents : MonoBehaviour {

	public float dragAmount = 2f;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDrag () {
		gameObject.layer = 2;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Casts the ray and get the first game object hit
		Physics.Raycast(ray, out hit);

		//		StartCoroutine (SpawnDragParticles ());

		pushBallplayerToPoint ();
	}

	void OnMouseUp () {
		gameObject.layer = 0;
		StartCoroutine (MoveBallplayer ());
	}

	IEnumerator MoveBallplayer () {
		while (Vector3.Distance (transform.position, hit.point) > 0) {
			pushBallplayerToPoint ();
			yield return null;
		}
	}
		
	void pushBallplayerToPoint () {
		Vector3 heading = hit.point - transform.position;
		GetComponent<Rigidbody> ().AddForce (heading);
		float distanceToPoint = heading.magnitude;
		GetComponent<Rigidbody> ().drag = dragAmount / distanceToPoint;

//		transform.rotation = Quaternion.RotateTowards (transform.rotation, hit.point, 1);
	}

//	public GameObject dragParticles;
//
//	IEnumerator SpawnDragParticles () {
//		GameObject dragParticleInstance = Instantiate (dragParticles);
//		dragParticleInstance.transform.position = hit.point;
//		yield return new WaitForSeconds(0.5f);
//		Destroy (dragParticleInstance);
//		yield return null;
//	}

}
