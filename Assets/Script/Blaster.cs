using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour {

	public GameObject parent;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		
	}

	public void DoBlast (){
		var go = Instantiate (prefab, parent.transform.position, Quaternion.identity);

	}
}
