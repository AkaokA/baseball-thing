using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FielderView : MonoBehaviour {

	private RaycastHit hit;
	Hashtable ballplayerTweenParams = new Hashtable ();
	private Vector3 destination;
	public float movementSpeed = 10;

	private List<Vector3> movementPath = new List<Vector3>();

	// Use this for initialization
	void Start () {
		// iTween parameters
		ballplayerTweenParams.Add ("position", destination);
		ballplayerTweenParams.Add ("speed", movementSpeed);
		ballplayerTweenParams.Add ("easetype", "easeInOut");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown () {

		// Highlight selected ballplayer
	}

	void OnMouseDrag () {
		// Hide the ballplayer from raycasting
		gameObject.layer = 2;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// Casts the ray and get the first game object hit
		Physics.Raycast(ray, out hit);

		// Draw path for ballplayer to follow

		movementPath.Add(hit.point);
	}

	void OnMouseUp () {
		// re-enable raycasting on ballplayer
		gameObject.layer = 0;

		destination = hit.point;
		ballplayerTweenParams ["position"] = destination;
			
		// move ballplayer along path
		iTween.MoveTo(gameObject, ballplayerTweenParams);
	}

}
