using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Camera[] allCams;
	public GameObject[] mirrors;
	public Transform camA, camB, camC, camD, camE, camF;
	public Vector3 posA, posB, posC, posD, posE, posF;

	// animate the game object from -1 to +1 and back
	public float minimum = -1.0F;
	public float maximum =  1.0F;

	// starting value for the Lerp    
	static float t = 0.0f;

	// Use this for initialization
	void Start () {
		camA.parent = mirrors [0].transform;
		camB.parent = mirrors [1].transform;
		camC.parent = mirrors [2].transform;
		camD.parent = mirrors [3].transform;
		camE.parent = mirrors [4].transform;
		camF.parent = mirrors [5].transform;
	}
	
	// Update is called once per frame
	void Update () {
		camA.localPosition = posA;
		camB.localPosition = posB;
		camC.localPosition = posC;
		camD.localPosition = posD;
		camE.localPosition = posE;
		camF.localPosition = posF;

		camA.position = new Vector3(0, Mathf.Lerp(minimum, maximum, t), 0);

		// .. and increate the t interpolater
		t += 0.5f * Time.deltaTime;

		// now check if the interpolator has reached 1.0
		// and swap maximum and minimum so game object moves
		// in the opposite direction.
		if (t > 1.0f){
			float temp = maximum;
			maximum = minimum;
			minimum = temp;
			t = 0.0f;
		}

	


	
	
	}
}
