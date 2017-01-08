using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualiserA : MonoBehaviour {

	public Material[] dashFrames;
	public int select = 0;
	int dir = 1;
	float timer = 1;
	MeshRenderer r;
	CharMovementA cm;

	void Start () {
		r = GetComponent<MeshRenderer> ();
		cm = GetComponent<CharMovementA> ();

	}
	

	void Update () {

		if (cm.dashing){
			timer += 0.3f;

			if (timer >= 1){
				timer = 0f;
				if(select >= dashFrames.Length-1){
					//select = 0;
					dir *= -1;
				}
				else if (select <= 0){
					select = 1;
					dir *= -1;
				}

				select += dir;
			}				
			r.material = dashFrames [select];

		}
		else{
			timer = 1;
			r.material = dashFrames [0];
		}



	}
}
