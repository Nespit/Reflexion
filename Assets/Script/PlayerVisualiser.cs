﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualiser : MonoBehaviour {

	public Material[] mats;
	public int select = 0;
	int dir = 1;
	float timer = 1;
	MeshRenderer r;
	public Color c;
	PlayerMovement pm;

	void Start () {
		r = GetComponent<MeshRenderer> ();
		pm = GetComponent<PlayerMovement> ();

	}
	

	void Update () {

		if (pm.dashing){
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
