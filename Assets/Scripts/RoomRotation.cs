using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRotation : MonoBehaviour 
{
	InteractiveRotation interaciveRotation;

	// Use this for initialization
	void Start () 
	{
		interaciveRotation = GetComponentInChildren<InteractiveRotation> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Constant rotation of the room.
		if(!interaciveRotation.isRotating)
			transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
	}
}
