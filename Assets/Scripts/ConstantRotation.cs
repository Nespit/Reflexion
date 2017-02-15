using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour 
{	
	private float speedIncrease = 0;
	private float counter;
	Coroutine speed;

	void Update () 
	{
		SpeedUp ();
		transform.RotateAround(Vector3.zero, Vector3.up, speedIncrease * Time.deltaTime);
	}

	void SpeedUp()
	{
		if (speed == null)
			speed = StartCoroutine (SpeedControl ());
	}

	IEnumerator SpeedControl()
	{
		if (counter > 20f) 
		{
			yield return new WaitForSeconds (30);
			counter = 0;
		}

		speedIncrease += Time.deltaTime;
		counter += Time.deltaTime;

		speed = null;
	}
}
