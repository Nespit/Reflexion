using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame {

	public Vector3[] positions;

	public Frame(int part, Vector3 pos){
		positions [part] = pos;
	}
}
