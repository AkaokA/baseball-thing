#pragma strict

var hitForce : float = 500f;
var hitDirection = Vector3(0,0.5,0);

function Start () {

}

function Update () {

}

function OnMouseDown() {
	GetComponent.<Rigidbody>().AddForce( hitDirection.normalized * hitForce);
}