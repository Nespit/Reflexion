using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrigger : MonoBehaviour {

	public bool isTriggered;

	void Start () 
	{
		isTriggered = false;
	}
	
	void OnTriggerEnter()
	{
		isTriggered = true;
	}

	void OnTriggerExit()
	{
		isTriggered = false;
	}
}
