using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	public GameObject target;
	public bool follow;
	GameObject instance;
	// Use this for initialization
	void Start () {
		instance = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {

		//y-axis is ignored
		if(follow){
			instance.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
		}
		
		
	}
}
