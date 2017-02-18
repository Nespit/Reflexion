using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Vector3UnityEvent : UnityEvent<Vector3>
{
    //concrete class for unity event using vector3 as param
}

public class RoomInteractive : MonoBehaviour {

    [SerializeField]
    private float m_rotationDowntime;
    [SerializeField]
    private float m_rotationDuration;
    private bool m_rotationCD;
    private bool m_rotating;


    //list of events
    private Dictionary<RoomWall, Vector3UnityEvent> m_eventDatabase;
    private static RoomInteractive m_instance; //forced singleton
    public static RoomInteractive instance
    {
        get
        {
            return m_instance;
        }
    }
    // Use this for initialization
    void Awake () {

        if (m_instance == null)
            m_instance = this;
        else
            Destroy(this);

        if (m_eventDatabase == null)
            m_eventDatabase = new Dictionary<RoomWall, Vector3UnityEvent>();


    }

    #region Event Manager implementation -- Might get Decoupled from here at a later time
    public void SubscribeEvent(RoomWall eventDesignator)
    {
        Vector3UnityEvent evt = null;
        if (m_eventDatabase.TryGetValue(eventDesignator, out evt))
        {
            Debug.Log("Can't add multiple events to one object");
            return;
        }
        else
        {
            evt = new Vector3UnityEvent();
            evt.AddListener(RoomRotationTrigger);
            m_eventDatabase.Add(eventDesignator, evt);
        }
    }

    public void UnSubscribeEvent(RoomWall eventDesignator)
    {
        if (!m_instance)
            return;

        Vector3UnityEvent evt = null;
        if (m_eventDatabase.TryGetValue(eventDesignator, out evt))
            evt.RemoveListener(RoomRotationTrigger);


    }

    void UnSubscribeAllEvents()
    {
        foreach (RoomWall designator in m_eventDatabase.Keys)
        {
            m_eventDatabase[designator].RemoveAllListeners();
        }
    }

    public void TriggerEvent(RoomWall designator,Vector3 velocity)
    {
        Vector3UnityEvent evt = null;

        if (m_eventDatabase.TryGetValue(designator, out evt))
            evt.Invoke(velocity);
    }
    #endregion

    private void RoomRotationTrigger(Vector3 triggerVelocity)
    {
        Debug.Log(triggerVelocity);
    }

    void OnDisable()
    {
        UnSubscribeAllEvents();
    }

    private void OnDestroy()
    {
        m_instance = null;
    }

}