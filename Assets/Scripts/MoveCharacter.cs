using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour 
{
	public float movementSpeed = 5;
	public float turnSmoothing = 15f;
	public float dashIntensity;
	public float dashDuration;
	public float dashSpeed;

	public KeyCode moveUpKey;
	public KeyCode moveDownKey;
	public KeyCode moveLeftKey;
	public KeyCode moveRightKey;
	public KeyCode dashKey;

	private bool isMovingUp;
	private bool isMovingDown;
	private bool isMovingLeft;
	private bool isMovingRight;
	private bool canDash = false;

	private Vector3 movement;
	private Rigidbody playerRigidBody;

	private Coroutine dash;
	private Coroutine move;
	private WaitForSecondsRealtime waitForDashDuration;

	void Start () 
	{
		playerRigidBody = GetComponent<Rigidbody> ();
		waitForDashDuration = new WaitForSecondsRealtime (dashDuration);
	}

	void Update () 
	{
		float lh = Input.GetAxisRaw ("Horizontal");
		float lv = Input.GetAxisRaw ("Vertical");

		if(Input.GetKeyDown(dashKey) && dash == null && canDash)
		{
			dash = StartCoroutine (Dash ());
		}

		if (move == null)
		{
			move = StartCoroutine (Move (lh, lv));
		}
	}

	IEnumerator Move(float lh, float lv)
	{
		yield return new WaitForFixedUpdate();

		movement.Set (lh, 0f, lv);
		movement = movement.normalized * movementSpeed * Time.deltaTime;
		playerRigidBody.MovePosition (transform.position + movement);

		if (lh != 0f || lv != 0f) 
		{
			Rotating(lh, lv);
		}

		move = null;
	}

	IEnumerator Dash()
	{
		yield return new WaitForFixedUpdate();

		dash = null;
	}

	void Rotating (float lh, float lv)
	{
		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody> ().rotation, targetRotation, turnSmoothing * Time.deltaTime);

		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}
}
