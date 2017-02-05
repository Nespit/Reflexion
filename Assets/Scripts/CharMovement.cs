using UnityEngine;
using System.Collections;

public class CharMovement : MonoBehaviour 
{
	public float speed;
	public float turnSmoothing;
	public float dashCooldown;
	private float dashIntensity;
	private float emRate;
	public string player = "PlayerA";
	private WaitForSecondsRealtime waitForDashCooldown;
	private Coroutine dash;
	private Quaternion currentRotation;
	private ParticleSystem pSystem;
	private ParticleSystem.EmissionModule em;
	[SerializeField]
	private float particleGainRate; 

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
	private float horizontalInput;
	private float verticalInput;

	void Start () 
	{
		playerRigidBody = GetComponent<Rigidbody> ();
		r = GetComponent<MeshRenderer> ();
		pSystem = GetComponentInChildren<ParticleSystem> ();
		em = pSystem.emission;

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

		emRate = em.rateOverTime.constantMax;

		if (emRate < pSystem.main.maxParticles) 
		{
			emRate += particleGainRate;
			em.rateOverTime = emRate;
		}

		if (emRate < 300)
			r.material = dashFrames [0];
		else if (emRate < 600)
			r.material = dashFrames [1];
		else if (emRate < 900)
			r.material = dashFrames [2];
		else if (emRate < 1200)
			r.material = dashFrames [3];
		else if (emRate < 1500)
			r.material = dashFrames [4];
		else if (emRate >= pSystem.main.maxParticles)
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
	}

	void FixedUpdate()
	{
		Move (horizontalInput, verticalInput);
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

			if (Input.GetKeyDown (dashKey) && emRate > 300) 
			{
				Dash (horizontalInput, verticalInput);
			}
		}
	}
		
	void Rotating (float lh, float lv)
	{
		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (currentRotation, targetRotation, turnSmoothing * Time.deltaTime);

		playerRigidBody.MoveRotation(newRotation);
	}

	void Dash(float lh, float lv)
	{
		movement.Set (lh, 0f, lv);

		movement = Camera.main.transform.TransformDirection(movement);

		movement = movement.normalized * emRate;

		playerRigidBody.AddForce(movement, ForceMode.Force);

		em.rateOverTime = 100;
	}
}