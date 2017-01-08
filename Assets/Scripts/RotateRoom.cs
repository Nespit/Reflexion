﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoom : MonoBehaviour {
	Vector3 pivot = new Vector3(0, 0, -15);

	// Update is called once per frame
	void Update () {
		transform.RotateAround(pivot, Vector3.up, 20 * Time.deltaTime);
	}
}
