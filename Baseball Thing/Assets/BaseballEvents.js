#pragma strict

function Start () {

}

function Update () {

}

function OnMouseDown() {
	GetComponent.<Rigidbody>().AddForce( (transform.forward + Vector3(0,0.5,0) ) * 500f);
}