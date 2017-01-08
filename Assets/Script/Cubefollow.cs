using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubefollow : MonoBehaviour {

	GameObject instance;
	public GameObject character;
	// Use this for initialization
	void Start () {
		instance = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		instance.transform.position = character.transform.position;
	}
}
