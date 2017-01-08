using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScrambler : MonoBehaviour {

	public GameObject[] heads;
	int active = 0;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

		for(int i = 0; i < heads.Length; i++){
			if (i == active){
				heads [i].SetActive (true);
			}
			else{
				heads [i].SetActive (false);
			}
		}

		if (Input.GetKey(KeyCode.Space)){

			if(active != 3){
				active++;
			}
			else{
				active = 0;
			}
		}
		
	}
}
