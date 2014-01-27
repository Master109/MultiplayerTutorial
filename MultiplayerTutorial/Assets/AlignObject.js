#pragma strict

function Start ()
{
	transform.position.x = Mathf.RoundToInt(transform.position.x);
	transform.position.y = Mathf.RoundToInt(transform.position.y);
	transform.position.z = Mathf.RoundToInt(transform.position.z);
	//transform.rotation.x = Mathf.RoundToInt(transform.rotation.x);
	//transform.rotation.y = Mathf.RoundToInt(transform.rotation.y);
	//transform.rotation.z = Mathf.RoundToInt(transform.rotation.z);
}

function Update ()
{
	
}