using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTimer {

    private float m_expectedTime = 0;
    private bool m_running;
	public void Start (float duration) {

        m_expectedTime = Time.time + duration;
        m_running = true;
	}

    public void Stop()
    {
        m_running = false;
    }

    public bool isDone
    {
        get
        {
            if (m_expectedTime <= Time.time && m_running)
            {
                m_running = false;
                return true;
            }
            else
                return false;
        }
    }
	
	
}
