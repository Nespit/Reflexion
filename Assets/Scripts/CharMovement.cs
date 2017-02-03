using UnityEngine;
using System.Collections;

public class CharMovement : MonoBehaviour 
{
	public float speed;
	public float turnSmoothing;
	public float dashCooldown;
	public float dashIntensity;
	public string player = "PlayerA";
	public bool canDash = true;
	private WaitForSecondsRealtime waitForDashCooldown;
	private Coroutine dash;
	private Quaternion currentRotation;

	public KeyCode dashKey;

	private Vector3 movement;
	private Vector3 lastMovement;
	private Rigidbody playerRigidBody;

	public Material[] dashFrames;
	public int select = 0;
	int dir = 1;
	float animationTimer = 1;
	MeshRenderer r;

	private string horizontalAxis;
	private string verticalAxis;
	private float horizontalInput;
	private float verticalInput;

	void Start () 
	{
		playerRigidBody = GetComponent<Rigidbody> ();
		r = GetComponent<MeshRenderer> ();

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
		horizontalInput = Input.GetAxisRaw (horizontalAxis);
		verticalInput = Input.GetAxisRaw (verticalAxis);

		if (canDash)
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

		if (Input.GetKeyDown (dashKey) && canDash && dash == null) 
		{
			dash = StartCoroutine (Dash ());
		}
	}

	void FixedUpdate()
	{
		Move (horizontalInput, verticalInput);
	}

	void Move (float lh, float lv)
	{
		lastMovement = movement;

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
		canDash = false;

		yield return new WaitForFixedUpdate ();

		movement.Set (horizontalInput, 0f, verticalInput);

		movement = Camera.main.transform.TransformDirection(movement);

		movement = movement.normalized * dashIntensity;

		playerRigidBody.AddForce(movement, ForceMode.Force);

		yield return new WaitForSeconds(dashCooldown);

		canDash = true;

		dash = null;
	}
}