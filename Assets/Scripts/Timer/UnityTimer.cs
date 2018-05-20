using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UnityTimer {

    private float m_expectedTime;
	public void Start (float duration) {

        m_expectedTime = Time.time + duration;
	}

    public bool isDone
    {
        get
        {
            if (m_expectedTime <= Time.time)
            {
                return true;
            }
            else
                return false;
        }
    }
	
	
}
