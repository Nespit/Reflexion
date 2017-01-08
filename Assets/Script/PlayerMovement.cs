using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	GameObject player;
	public GameObject target;
	public KeyCode up, down, left, right, dash;
	public float movementSpeed;
	public bool dashing = false;
	public float timer = 0;
	public float staticSpeed, dashIntensity;
	// Use this for initialization
	void Start () {
		player = this.gameObject;
		movementSpeed = staticSpeed;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(up)){
			target.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z + movementSpeed);
		}

		if(Input.GetKey(down)){
			target.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z - movementSpeed);
		}

		if(Input.GetKey(left)){
			target.transform.position = new Vector3 (target.transform.position.x - movementSpeed, target.transform.position.y, target.transform.position.z);
		}

		if(Input.GetKey(right)){
			target.transform.position = new Vector3 (target.transform.position.x + movementSpeed, target.transform.position.y, target.transform.position.z);
		}

		if(Input.GetKeyDown(dash)){
			timer = dashIntensity;
			dashing = true;
		}

		if (timer > 0){
			timer -= 0.1f;
			movementSpeed = timer;
		}
		else if(timer <= 0) {
			movementSpeed = staticSpeed;
			dashing = false;
		}
		
	}
}
