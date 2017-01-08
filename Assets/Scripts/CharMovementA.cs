using UnityEngine;
using System.Collections;

public class CharMovementA : MonoBehaviour 
{
	public float speed = 2;
	public float staticSpeed;
	public float runSpeed = 5f;
	public float turnSmoothing = 15f;

	float timer;
	public float dashIntensity;
	public bool dashing = false;
	public KeyCode dash;

	private Vector3 movement;
	private Rigidbody playerRigidBody;

	void Awake()
	{
		playerRigidBody = GetComponent<Rigidbody> ();
		speed = staticSpeed;
	}

	void FixedUpdate()
	{
		float lh = Input.GetAxisRaw ("Horizontal");
		float lv = Input.GetAxisRaw ("Vertical");

		Move (lh, lv);
	}


	void Move (float lh, float lv)
	{
		movement.Set (lh, 0f, lv);
		movement = Camera.main.transform.TransformDirection(movement);


		if (Input.GetKey (KeyCode.LeftShift))
		{
			movement = movement.normalized * runSpeed * Time.deltaTime;
		} 
		else 
		{
			movement = movement.normalized * speed * Time.deltaTime;
		}

		playerRigidBody.MovePosition (transform.position + movement);


		if(Input.GetKeyDown(dash)){
			timer = dashIntensity;
			dashing = true;
		}

		if (timer > 0){
			timer -= 0.1f;
			speed = timer * 20;
		}
		else if(timer <= 0) {
			speed = staticSpeed;
			dashing = false;
		}


		if (lh != 0f || lv != 0f) 
		{
			Rotating(lh, lv);
		}
	}


	void Rotating (float lh, float lv)
	{
		Vector3 targetDirection = new Vector3 (lh, 0f, lv);

		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody> ().rotation, targetRotation, turnSmoothing * Time.deltaTime);

		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}

}