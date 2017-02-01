using UnityEngine;
using System.Collections;

public class CharMovement : MonoBehaviour 
{
	public float speed = 10;
	public float turnSmoothing = 15f;

	private float dashDuration = 1.0f;
	public float dashIntensity = 1000f;
	public string player = "PlayerA";
	public bool isDashing = false;
	public bool canDash = true;
	private WaitForSecondsRealtime waitForDashDuration;
	private Coroutine dash;
	private Quaternion currentRotation;

	public KeyCode dashKey;

	private Vector3 movement;
	private Rigidbody playerRigidBody;

	public Material[] dashFrames;
	public int select = 0;
	int dir = 1;
	float animationTimer = 1;
	MeshRenderer r;

	private string horizontalAxis;
	private string verticalAxis;

	void Start () 
	{
		playerRigidBody = GetComponent<Rigidbody> ();
		r = GetComponent<MeshRenderer> ();
		waitForDashDuration = new WaitForSecondsRealtime (dashDuration);

		if (player == "PlayerA") 
		{
			horizontalAxis = "Horizontal";
			verticalAxis = "Vertical";
		} else 
		{
			horizontalAxis = "Horizontal2";
			verticalAxis = "Vertical2";
		}
	}
		
	void Update () 
	{
		if (isDashing)
		{
			animationTimer += 0.3f;

			if (animationTimer >= 1){
				animationTimer = 0f;
				if(select >= dashFrames.Length-1){
					//select = 0;
					dir *= -1;
				}
				else if (select <= 0){
					select = 1;
					dir *= -1;
				}

				select += dir;
			}				
			r.material = dashFrames [select];
		}
		else
		{
			animationTimer = 1;
			r.material = dashFrames [0];
		}
	}

	void FixedUpdate()
	{
		float lh = Input.GetAxisRaw (horizontalAxis);
		float lv = Input.GetAxisRaw (verticalAxis);

		Move (lh, lv);

		if (Input.GetKeyDown (dashKey) && canDash && dash == null) 
		{
			dash = StartCoroutine (Dash ());
		}
	}

	void Move (float lh, float lv)
	{
		movement.Set (lh, 0f, lv);

		movement = Camera.main.transform.TransformDirection(movement);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidBody.MovePosition (transform.position + movement);

		if (lh != 0f || lv != 0f) 
		{
			currentRotation = playerRigidBody.rotation;
			Rotating(lh, lv);
		}
	}
		
	void Rotating (float lh, float lv)
	{
		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (currentRotation, targetRotation, turnSmoothing * Time.deltaTime);

		playerRigidBody.MoveRotation(newRotation);
	}

	IEnumerator Dash()
	{
		float lh = Input.GetAxisRaw (horizontalAxis);
		float lv = Input.GetAxisRaw (verticalAxis);

		isDashing = true;

		canDash = false;

		movement.Set (lh, 0f, lv);

		movement = Camera.main.transform.TransformDirection(movement);

		movement = movement.normalized * dashIntensity;

		playerRigidBody.AddForce(movement, ForceMode.Force);

		yield return waitForDashDuration;

		isDashing = false;

		dash = null;
	}
}