using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour {

	GameObject instance;
	public bool pulsate;
	public float speed = 0.1f;
	public float min, max;
	float size = 1;
	// Use this for initialization
	void Start () {
		instance = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

		if(pulsate){

			size += speed;

			if(size > max || size < min){
				speed *= -1;				
			}

			instance.transform.localScale = new Vector3 (size, size, 0);
		}
		
	}
}
