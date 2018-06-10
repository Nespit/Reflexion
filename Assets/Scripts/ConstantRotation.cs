using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour 
{
    [SerializeField]
    private float m_SpeedIncrease = 0;

	void Update () 
	{
		transform.RotateAround(Vector3.zero, Vector3.up, m_SpeedIncrease * Time.deltaTime);
	}
}
