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
	private Coroutine dash;
	private Quaternion currentRotation;
	private ParticleSystem pSystem;
	private ParticleSystem.EmissionModule em;
	private ParticleSystemRenderer rm;
	[SerializeField]
	private float particleGainRate; 
	private Coroutine m_dash;
	private Coroutine m_collision;
	public KeyCode dashKey;
	public KeyCode jumpKey;
	public bool isDashing;
	private Vector3 movement;
	private Rigidbody playerRigidBody;

	public Material[] dashFrames;
	int animationFrame = 1;
	int dir = 1;
	float animationTimer = 1;
	MeshRenderer r;

	private string horizontalAxis;
	private string verticalAxis;
	private float horizontalInput;
	private float verticalInput;

	//jumping
	public Vector3 jump;
	public float jumpForce = 1.0f;
    public float groundDetectionRange = 0.5f;
	public bool isGrounded
    {
        get
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, groundDetectionRange))
            {
                return true;
            }
            else
                return false;
        }
    }

	void Start () 
	{
        playerRigidBody = GetComponent<Rigidbody> ();
		r = GetComponent<MeshRenderer> ();
		pSystem = GetComponentInChildren<ParticleSystem> ();
		em = pSystem.emission;
		rm = pSystem.GetComponent<ParticleSystemRenderer>();
		jump = new Vector3(0.0f, 3.0f, 0.0f);

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
        Vector3 testEnd = transform.position;
        testEnd.y -= groundDetectionRange;
        Debug.DrawLine(transform.position, testEnd);
		//Get input values
		horizontalInput = Input.GetAxisRaw (horizontalAxis);
		verticalInput = Input.GetAxisRaw (verticalAxis);

		//Get current particle emission rate
		emRate = em.rateOverTime.constantMax;

		//If the emission rate is below the maximum limit, increase it by particleGainRate
		if (emRate < pSystem.main.maxParticles) 
		{
			emRate += particleGainRate;
			em.rateOverTime = emRate;
		}

		//Change the appearance of the the player model according to the current particle emission rate
		if (emRate < 100) 
		{
			rm.enabled = false;
			r.material = dashFrames [0];
		} 
		else if (emRate < 300) 
		{
			rm.enabled = true;
			r.material = dashFrames [1];
		}
		else if (emRate < 600)
			r.material = dashFrames [2];
		else if (emRate < 900)
			r.material = dashFrames [3];
		else if (emRate < 1200)
			r.material = dashFrames [4];
		else if (emRate < 1500)
			r.material = dashFrames [5];
		else if (emRate >= pSystem.main.maxParticles)
		{
			animationTimer += 0.3f;

			if (animationTimer >= 1){
				animationTimer = 0f;
				if(animationFrame >= dashFrames.Length-1){
					dir *= -1;
				}
				else if (animationFrame <= 1){
					animationFrame = 1;
					dir *= -1;
				}

				animationFrame += dir;
			}				
			r.material = dashFrames [animationFrame];
		}

		//Dash input
		if (Input.GetKeyDown (dashKey) && emRate > 300 && m_dash == null && (verticalInput != 0f || horizontalInput != 0f)) 
		{
			m_dash = StartCoroutine(Dash (horizontalInput, verticalInput));
		}

		if (Input.GetKeyDown (jumpKey) && isGrounded) 
		{
			playerRigidBody.AddForce (jump * jumpForce, ForceMode.Impulse);
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
		}
	}
		
	void Rotating (float lh, float lv)
	{
		Quaternion targetRotation = Quaternion.LookRotation (movement, Vector3.up);

		Quaternion newRotation = Quaternion.Lerp (currentRotation, targetRotation, turnSmoothing * Time.deltaTime);

		playerRigidBody.MoveRotation(newRotation);
	}

	IEnumerator Dash(float lh, float lv)
	{
		yield return new WaitForFixedUpdate ();

		isDashing = true;

		movement.Set (lh, 0f, lv);

		movement = Camera.main.transform.TransformDirection(movement);

		movement = movement.normalized * emRate;

		playerRigidBody.AddForce(movement, ForceMode.Force);

		em.rateOverTime = 100;

		m_dash = null;
	}

	IEnumerator Collision()
	{
		yield return new WaitForSeconds (0.3f);

		if (isDashing)
			isDashing = false;
		
		m_collision = null;
	}

	//Check if the player was hit by a dashing opponent
	void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 10 && !isDashing) 
		{
			emRate = 0;
			em.rateOverTime = emRate;
		}

		if (m_collision == null)
			m_collision = StartCoroutine (Collision ());	
	}


}