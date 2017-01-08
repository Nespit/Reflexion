using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour {

	GameObject instance;
	SpriteRenderer r;
	public float growth;
	public float blastIntensity;
	// Use this for initialization
	void Start () {
		instance = this.gameObject;
		growth += Random.Range (0f, 0.2f);
		r = GetComponent<SpriteRenderer> ();
		instance.transform.eulerAngles = new Vector3 (90, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

		instance.transform.localScale = new Vector3 (instance.transform.localScale.x + growth, instance.transform.localScale.y  + growth, 0);
		Color color = r.material.color;
		color.a -= blastIntensity;
		r.material.color = color;

		if (color.a <= 0){
			Destroy (instance);
		}

	}
		
}
