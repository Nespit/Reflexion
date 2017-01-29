﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRotation : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] walls;
	private MeshRenderer[] wallRenderers;
	private Camera[] wallCameras;
	private RotationTrigger[] wallTriggers;

	//The element of the walls[] that is the floor.
	public int floor;
	public int formerFloor;

	private WaitWhile waitWhileUntriggered;
	private WaitWhile waitWhileRotating;
	private WaitForSeconds[] rotationIntervals;
	public int numberOfRotationIntervals;
	public float minIntervalSeconds;
	public float maxIntervalSeconds;
	private bool canBeRotated;
	public bool isRotating;
	Vector3 targetRotation;
	private Coroutine rotator;
	private Coroutine rotating;

	void Start()
	{
		rotationIntervals = new WaitForSeconds[numberOfRotationIntervals];
		waitWhileUntriggered = new WaitWhile (() => canBeRotated);
		waitWhileRotating = new WaitWhile (() => isRotating);
		wallRenderers = new MeshRenderer[6];
		wallCameras = new Camera[6];
		wallTriggers = new RotationTrigger[6];

		formerFloor = 0;
		floor = 0;

		for (int i = 0; i < numberOfRotationIntervals; i++)
		{
			rotationIntervals[i] = new WaitForSeconds(Random.Range(minIntervalSeconds, maxIntervalSeconds));
		}

		for (int o = 0; o < walls.Length; o++) 
		{
			wallRenderers [o] = walls [o].GetComponent<MeshRenderer> ();
			wallCameras [o] = walls [o].GetComponentInChildren<Camera> ();
			wallTriggers [o] = walls [o].GetComponentInChildren<RotationTrigger> ();
		}
	}

	void Update () 
	{
		//Constant rotation of the room.
		if(!isRotating)
			transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);

		//Enable interactive rotatability.
		EnableRotatability ();

		//Pulsing wall effect during rotatability.
		if (canBeRotated) 
		{
			Color lerpedColor = Color.Lerp (Color.black, Color.white, Mathf.PingPong (Time.time, 0.3f));

			for (int i = 0; i < walls.Length; i++) 
			{
				if (i != floor && i != (5 - floor)) 
				{
					wallRenderers [i].material.SetColor ("_EmissionColor", lerpedColor);
					Debug.Log(wallRenderers[i] + " is blinking");
				}
			}
		}

		//Check for rotation trigger.
		for (int i = 0; i < walls.Length; i++) 
		{
			if (i != floor && i != (5 - floor)) 
			{
				if (wallTriggers [i].isTriggered) 
				{
					Rotate (i);
					break;
				}
			}
		}

		//Rotate
		if(isRotating)
		{
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler(targetRotation), 3 * Time.deltaTime);

			if (transform.rotation == Quaternion.Euler (targetRotation))
				isRotating = false;
		}
	}

	private void EnableRotatability()
	{
		if (rotator == null)
			rotator = StartCoroutine (BecomeRotatable ());
	}

	IEnumerator BecomeRotatable()
	{
		yield return rotationIntervals [Random.Range (0, rotationIntervals.Length)];

		canBeRotated = true;

		yield return waitWhileUntriggered;
		rotator = null;
	}

	private void Rotate(int i)
	{
		if (rotating == null)
			rotating = StartCoroutine (Rotating (i));
	}

	IEnumerator Rotating(int i)
	{
		canBeRotated = false;
		isRotating = true;

		//Change wall color back to normal.
		for (int o = 0; o < walls.Length; o++) 
		{
			if (o != floor && o != (5 - floor)) 
			{
				wallRenderers [o].material.SetColor ("_EmissionColor", Color.black);
			}
		}

		formerFloor = floor;
		floor = i;

		switch (formerFloor) 
		{
		case 0:
			switch (floor) 
			{
			case 0:
				break;
			case 1:
				targetRotation = new Vector3 (0, 0, -90) + transform.rotation.eulerAngles;
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 4:
				targetRotation = new Vector3 (0, 0, 90) + transform.rotation.eulerAngles;
				break;
			case 5:
				break;
			}
			break;
		case 1:
			switch (floor) {
			case 0:
				targetRotation = new Vector3 (0, 0, 90) + transform.rotation.eulerAngles;
				break;
			case 1:
				break;
			case 2:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 4:
				break;
			case 5:
				targetRotation = new Vector3 (0, 0, -90) + transform.rotation.eulerAngles;
				break;
			}
			break;
		case 2:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 1:
				targetRotation = new Vector3 (90, 90, 90) + transform.rotation.eulerAngles;
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				targetRotation = new Vector3 (90, -90, -90) + transform.rotation.eulerAngles;
				break;
			case 5:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			}
			break;
		case 3:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 1:
				targetRotation = new Vector3 (-90, -90, 90) + transform.rotation.eulerAngles;
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				targetRotation = new Vector3 (-90, 90, -90) + transform.rotation.eulerAngles;
				break;
			case 5:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			}
			break;
		case 4:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (0, 0, -90) + transform.rotation.eulerAngles;
				break;
			case 1:
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 4:
				break;
			case 5:
				targetRotation = new Vector3 (0, 0, 90) + transform.rotation.eulerAngles;
				break;
			}
			break;
		case 5:
			switch (floor) 
			{
			case 0:
				break;
			case 1:
				targetRotation = new Vector3 (0, 0, 90) + transform.rotation.eulerAngles;
				break;
			case 2:
				targetRotation = new Vector3 (-90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (90, 0, 0) + transform.rotation.eulerAngles;
				break;
			case 4:
				targetRotation = new Vector3 (0, 0, -90) + transform.rotation.eulerAngles;
				break;
			case 5:
				break;
			}
			break;
		}

		yield return waitWhileRotating;
		rotating = null;
	}
}