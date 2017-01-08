using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	GameObject player;
	public GameObject target;
	public KeyCode up, down, left, right;
	public float movementSpeed;
	// Use this for initialization
	void Start () {
		player = this.gameObject;
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
		
	}
}
