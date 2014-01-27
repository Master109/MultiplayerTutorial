#pragma strict

var followingPlayer = true;
var runOnce = false;
var MOVE_SPEED = 0.125;
var ZOOM_SPEED = 0.125;

function Start ()
{
	
}

function Update ()
{
	if (Input.GetAxis("Zoom") == 1 && camera.orthographicSize < 40)
		camera.orthographicSize += ZOOM_SPEED;
	else if (Input.GetAxis("Zoom") == -1 && camera.orthographicSize > 5)
		camera.orthographicSize -= ZOOM_SPEED;
	if (Input.GetAxis("Camera Follow") == 1 && !followingPlayer && runOnce)
	{
		GetComponent(SmoothFollow2D).target = GameObject.Find("Human Peasant").transform;
		followingPlayer = true;
		runOnce = false;
	}
	else if (Input.GetAxis("Camera Follow") == 1 && followingPlayer && runOnce)
	{
		GetComponent(SmoothFollow2D).target = GameObject.Find("Target").transform;
		followingPlayer = false;
		runOnce = false;
	}
	else if (Input.GetAxis("Camera Follow") == 0)
		runOnce = true;
	if (Input.GetAxis("Camera Horizontal") == 1)
		transform.position.x += MOVE_SPEED;
	else if (Input.GetAxis("Camera Horizontal") == -1)
		transform.position.x -= MOVE_SPEED;
	if (Input.GetAxis("Camera Vertical") == 1)
		transform.position.y += MOVE_SPEED;
	else if (Input.GetAxis("Camera Vertical") == -1)
		transform.position.y -= MOVE_SPEED;
}