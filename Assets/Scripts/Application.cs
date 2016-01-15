using UnityEngine;
using System.Collections;

public class BaseballElement : MonoBehaviour
{
	// Gives access to the application and all instances.
	public Application app { get { return GameObject.FindObjectOfType<Application>(); }}
}

public class Application : MonoBehaviour
{
	// Reference to the root instances of the MVC.
	public AppModel model;
	public AppViews views;
	public GridViews gridViews;
	public AppController controller;

	// Init things here
	void Start() {
		
	}
}

