using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RoomUnityEvent : UnityEvent<RoomWall, Vector3>
{
    //concrete class for unity event using vector3 as param
}

public class RoomInteractive : MonoBehaviour {

    private const int k_waitFrameCount = 3;
    private const float k_angleRotation = 90.0f;
    [SerializeField]
    private float m_collisionMagnitude = 1.0f;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private float m_rotationDowntime = 1.0f;
    private WaitForSeconds m_rotationTick;
    private UnityTimer m_rotationRefresh;
    private bool m_rotating = false;
    [SerializeField]
    private RoomWall m_currentWall;
    //list of events
    private Dictionary<RoomWall, RoomUnityEvent> m_eventDatabase;
    private static RoomInteractive m_instance; //forced singleton
    public static RoomInteractive instance
    {
        get
        {
            return m_instance;
        }
    }

    public RoomWall CurrentWall
    {
        get
        {
            return m_currentWall;
        }
    }

    // Use this for initialization
    void Awake() {

        if (m_instance == null)
            m_instance = this;
        else
            Destroy(this);

        if (m_eventDatabase == null)
            m_eventDatabase = new Dictionary<RoomWall, RoomUnityEvent>();
        m_rotationTick = new WaitForSeconds(Time.deltaTime);
        m_rotationRefresh = new UnityTimer();
        m_rotationRefresh.Start(0); // make timer load up with 0 wait
        StartCoroutine(SetupRoom());

    }


    IEnumerator SetupRoom()
    {

        for (int i = 0; i < k_waitFrameCount; ++i)
			yield return new WaitForEndOfFrame();
        
        foreach(KeyValuePair<RoomWall,RoomUnityEvent> pair in m_eventDatabase)
        {
            if (pair.Key.ID != m_currentWall.ID)
                pair.Key.ToggleCamera();
        }
    }


    #region Event Manager implementation -- Might get Decoupled from here at a later time
    public void Subscribe(RoomWall eventDesignator)
    {
        RoomUnityEvent evt = null;
        if (m_eventDatabase.TryGetValue(eventDesignator, out evt))
        {
            Debug.Log("Can't add multiple events to one object");
            return;
        }
        else
        {
            evt = new RoomUnityEvent();
            evt.AddListener(RoomRotationTrigger);
            m_eventDatabase.Add(eventDesignator, evt);
        }
    }

    public void UnSubscribeEvent(RoomWall eventDesignator)
    {
        if (!m_instance)
            return;

        RoomUnityEvent evt = null;
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

    public void TriggerEvent(RoomWall designator, Vector3 velocity)
    {
        RoomUnityEvent evt = null;

        if (m_eventDatabase.TryGetValue(designator, out evt))
            evt.Invoke(designator,velocity);
    }
    #endregion

    private void RoomRotationTrigger(RoomWall hitWall,Vector3 triggerVelocity)
    {
        if (hitWall.ID != m_currentWall.ID && m_rotationRefresh.isDone && !m_rotating)
        {
            if(triggerVelocity.sqrMagnitude > m_collisionMagnitude) // trigger the rotation if we hit hard enough
            {
                m_rotating = true;
                var rotDir = GetRotation(m_currentWall.ID, hitWall.ID);
                StartCoroutine(RotateRoom(rotDir)); // no need to cache as it gets auto collected
                m_currentWall.ToggleCamera();
                m_currentWall = hitWall;
                m_currentWall.ToggleCamera();
               
            }

        }

    }
    //Derp method to get rotation offset
    Vector3 GetRotation(int cChild, int tChild)
    {
        Vector3 targetRotation = Vector3.zero;
        switch (cChild)
        {
            case 0:
                switch (tChild)
                {
                    case 0:
                        break;
                    case 1:
                        targetRotation = new Vector3(0, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 2:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 3:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 4:
                        targetRotation = new Vector3(0, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 5:
                        break;
                }
                break;
            case 1:
                switch (tChild)
                {
                    case 0:
                        targetRotation = new Vector3(0, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 1:
                        break;
                    case 2:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 3:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 4:
                        break;
                    case 5:
                        targetRotation = new Vector3(0, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                }
                break;
            case 2:
                switch (tChild)
                {
                    case 0:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 1:
                        targetRotation = new Vector3(-k_angleRotation, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        targetRotation = new Vector3(-k_angleRotation, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 5:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                }
                break;
            case 3:
                switch (tChild)
                {
                    case 0:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 1:
                        targetRotation = new Vector3(k_angleRotation, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        targetRotation = new Vector3(k_angleRotation, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 5:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                }
                break;
            case 4:
                switch (tChild)
                {
                    case 0:
                        targetRotation = new Vector3(0, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 1:
                        break;
                    case 2:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 3:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 4:
                        break;
                    case 5:
                        targetRotation = new Vector3(0, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                }
                break;
            case 5:
                switch (tChild)
                {
                    case 0:
                        break;
                    case 1:
                        targetRotation = new Vector3(0, 0, k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 2:
                        targetRotation = new Vector3(k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 3:
                        targetRotation = new Vector3(-k_angleRotation, 0, 0) + transform.localRotation.eulerAngles;
                        break;
                    case 4:
                        targetRotation = new Vector3(0, 0, -k_angleRotation) + transform.localRotation.eulerAngles;
                        break;
                    case 5:
                        break;
                }
                break;
        }
        return targetRotation;
    }

    IEnumerator RotateRoom(Vector3 nRotEuler)
    {
        Quaternion targetRotation = Quaternion.Euler(nRotEuler);
        m_rotationRefresh.Start(m_rotationSpeed);
        float elapsedTime = 0;
        while (!m_rotationRefresh.isDone)
        {
            elapsedTime += Time.deltaTime; //add time passed
            float rotationPercentage = elapsedTime / m_rotationSpeed;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, rotationPercentage);
            yield return m_rotationTick;
        }
        transform.localRotation = targetRotation; // getting rid of .000xx errors in the rotation;

        m_rotationRefresh.Start(m_rotationDowntime);
        m_rotating = false;
        yield break;



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