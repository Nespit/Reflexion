using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRotation : MonoBehaviour 
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
	Quaternion startRotation;
	private Coroutine rotator;
	private Coroutine rotating;

	public float transitionTime; 
	private float targetTime;


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
					if (wallTriggers [i].isTriggered) 
					{
						Rotate (i);
						break;
					}
					wallRenderers [i].material.SetColor ("_EmissionColor", lerpedColor);
				}
			}
		}
			
		//Rotate
		if(isRotating)
		{
			float timePassed = targetTime - Time.time;
			float lerpPercentage = (transitionTime - timePassed) / transitionTime;

			transform.localRotation = Quaternion.Lerp (startRotation, Quaternion.Euler(targetRotation), lerpPercentage);

			if (timePassed <= 0)
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
		yield return waitWhileRotating;
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

		targetTime = Time.time + transitionTime;
		startRotation = transform.localRotation;

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

		wallCameras [formerFloor].enabled = false;
		wallCameras [floor].enabled = true;

		switch (formerFloor) 
		{
		case 0:
			switch (floor) 
			{
			case 0:
				break;
			case 1:
				targetRotation = new Vector3 (0, 0, -90) + transform.localRotation.eulerAngles;
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 4:
				targetRotation = new Vector3 (0, 0, 90) + transform.localRotation.eulerAngles;
				break;
			case 5:
				break;
			}
			break;
		case 1:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (0, 0, 90) + transform.localRotation.eulerAngles;
				break;
			case 1:
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 4:
				break;
			case 5:
				targetRotation = new Vector3 (0, 0, -90) + transform.localRotation.eulerAngles;
				break;
			}
			break;
		case 2:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 1:
				targetRotation = new Vector3 (-90, 0, -90) + transform.localRotation.eulerAngles;
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				targetRotation = new Vector3 (-90, 0, 90) + transform.localRotation.eulerAngles;
				break;
			case 5:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			}
			break;
		case 3:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 1:
				targetRotation = new Vector3 (90, 0, -90) + transform.localRotation.eulerAngles;
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				targetRotation = new Vector3 (90, 0, 90) + transform.localRotation.eulerAngles;
				break;
			case 5:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			}
			break;
		case 4:
			switch (floor) 
			{
			case 0:
				targetRotation = new Vector3 (0, 0, -90) + transform.localRotation.eulerAngles;
				break;
			case 1:
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 4:
				break;
			case 5:
				targetRotation = new Vector3 (0, 0, 90) + transform.localRotation.eulerAngles;
				break;
			}
			break;
		case 5:
			switch (floor) 
			{
			case 0:
				break;
			case 1:
				targetRotation = new Vector3 (0, 0, 90) + transform.localRotation.eulerAngles;
				break;
			case 2:
				targetRotation = new Vector3 (90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 3:
				targetRotation = new Vector3 (-90, 0, 0) + transform.localRotation.eulerAngles;
				break;
			case 4:
				targetRotation = new Vector3 (0, 0, -90) + transform.localRotation.eulerAngles;
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
