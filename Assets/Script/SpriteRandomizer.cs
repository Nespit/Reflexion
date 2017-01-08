using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour {

	public Material[] mats;
	public int select = 0;
	int dir = 1;
	float timer = 1;
	MeshRenderer r;

	void Start () {
		r = GetComponent<MeshRenderer> ();
		
	}
	

	void Update () {

		if (Input.GetKey(KeyCode.Space)){
			timer += 0.3f;

			if (timer >= 1){
				timer = 0f;
				if(select >= mats.Length-1){
					//select = 0;
					dir *= -1;
				}
				else if (select <= 0){
					select = 1;
					dir *= -1;
				}

				select += dir;
			}




			r.material = mats [select];

		}
		else{
			timer = 1;
			r.material = mats [0];
		}

	}
}
