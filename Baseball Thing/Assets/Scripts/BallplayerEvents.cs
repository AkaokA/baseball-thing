using UnityEngine;
using System.Collections;

public class BallplayerEvents : MonoBehaviour {

	public float moveSpeed = 0.5f;
	private RaycastHit hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseUp () {
		gameObject.layer = 0;
	}

	public GameObject dragParticles;

	IEnumerator spawnDragParticles () {
		GameObject dragParticleInstance = Instantiate (dragParticles);
		dragParticleInstance.transform.position = hit.point;
		yield return new WaitForSeconds(0.5f);
		Destroy (dragParticleInstance);
		yield return null;
	}

	void OnMouseDrag () {
		gameObject.layer = 2;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// Casts the ray and get the first game object hit
		Physics.Raycast(ray, out hit);

		StartCoroutine (spawnDragParticles ());

		transform.position = Vector3.MoveTowards (transform.position, hit.point, moveSpeed);
	}
}
