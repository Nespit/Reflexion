using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomRotatorState
{
    Invalid,
    Idle = 1,
    ConstantRotationOnly = 2,
    ExecuteCombinedRotation = 4
}

public enum FaceDirection
{
    None,
    Forward,
    Left,
    Right,
    Backward
}

/// <summary>
/// Rotator class for dealing with the current rotation state of the room, keep it as a updateless monobehavior for ease of use
/// </summary>
public class RoomRotator : MonoBehaviour {


    private const float kCombinedRotationAngle = 90.0f;

    [SerializeField]
    private float m_InitialConstantRotation;
    [SerializeField]
    private float m_RotationIncreasePerSecond;
    private float m_CurrentRotationPerSecond;
    
    //Dummy transform used to pre calculate the target rotation
    private Transform m_DummyTransform;

    public RoomRotatorState State { get; private set; }
    private FaceDirection m_CurrentFaceDirection;
    private UnityTimer m_CombinedRotationTimer = new UnityTimer();
    private float m_CurrentLerpFactor = 0.0f;

    public bool SetState(RoomRotatorState state, FaceDirection direction = FaceDirection.None)
    {
        if ((State & RoomRotatorState.ExecuteCombinedRotation) != 0)
            return false;

        if ((state & RoomRotatorState.Idle) != 0)
        {
            State = state;
            return true;
        }

        State |= state;
        if ((State & RoomRotatorState.ExecuteCombinedRotation) != 0 && m_CombinedRotationTimer.isDone)
        {
            m_CurrentFaceDirection = direction;
            //Pre rotate the dummy target transform to next Y 90 degree multiple then apply desired rotation
            //float currentYRot = m_DummyTransform.rotation.eulerAngles.y;
            //bool isNeg = currentYRot < 0;
            //currentYRot = kCombinedRotationAngle * (((isNeg ? Mathf.Abs(currentYRot) : currentYRot) - 1) / kCombinedRotationAngle + 1);
            //m_DummyTransform.Rotate(0, (isNeg ? -1 * currentYRot : currentYRot) - m_DummyTransform.rotation.eulerAngles.y, 0, Space.World);

            switch (m_CurrentFaceDirection)
            {
                case FaceDirection.Forward:
                    m_DummyTransform.Rotate(kCombinedRotationAngle, 0, 0, Space.World);
                    break;
                case FaceDirection.Backward:
                    m_DummyTransform.Rotate(-kCombinedRotationAngle, 0, 0, Space.World);

                    break;
                case FaceDirection.Left:
                    m_DummyTransform.Rotate(0, 0, kCombinedRotationAngle, Space.World);
                    break;
                case FaceDirection.Right:
                    m_DummyTransform.Rotate(0, 0, -kCombinedRotationAngle, Space.World);
                    break;
            }
            m_CombinedRotationTimer.Start(1.0f);
        }

        return true;
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;

        if (m_DummyTransform == null)
        {
            m_DummyTransform = new GameObject("RoomRotator Dummy Transform").transform;
            m_DummyTransform.position = transform.position;
            m_DummyTransform.rotation = transform.rotation;
        }
    }

    private void OnDisable()
    {
        if(m_DummyTransform != null)
            Destroy(m_DummyTransform.gameObject);
    }

    public void Rotate(float deltaTime)
    {
        RoomRotatorState rsState = State & RoomRotatorState.ExecuteCombinedRotation;
        if (rsState == RoomRotatorState.Invalid)
            rsState = State & RoomRotatorState.ConstantRotationOnly;

        switch (rsState)
        {
            case RoomRotatorState.ConstantRotationOnly:
                transform.Rotate(0, m_CurrentRotationPerSecond * deltaTime, 0, Space.World);
                m_DummyTransform.Rotate(0, m_CurrentRotationPerSecond * deltaTime, 0, Space.World);

                break;
            case RoomRotatorState.ExecuteCombinedRotation:
                if (m_CombinedRotationTimer.isDone)
                {
                    State &= ~RoomRotatorState.ExecuteCombinedRotation;

                    m_CurrentFaceDirection = FaceDirection.None;
                    m_CurrentLerpFactor = 0.0f;
                    //do one correction lerp to ensure we have no interpolation errors due to the varrying value of deltaTime
                    transform.rotation = Quaternion.Lerp(transform.rotation, m_DummyTransform.rotation, 1.0f);
                    return;
                }
                m_CurrentLerpFactor += deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, m_DummyTransform.rotation, m_CurrentLerpFactor);
                break;
            default:
                //idle state do nothing;
                break;
        }

    }

    //TESTING ONLY -- THIS FUNCTION WILL BE REMOVED ONCE SCRIPT IS DONE
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SetState(RoomRotatorState.ConstantRotationOnly);
            m_CurrentRotationPerSecond = m_InitialConstantRotation;
            Debug.Log("State switch");
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            State = RoomRotatorState.ConstantRotationOnly;
            SetState(RoomRotatorState.ExecuteCombinedRotation, FaceDirection.Forward);
            Debug.Log("State switch");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            State = RoomRotatorState.ConstantRotationOnly;
            SetState(RoomRotatorState.ExecuteCombinedRotation, FaceDirection.Backward);
            Debug.Log("State switch");
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            State = RoomRotatorState.ConstantRotationOnly;
            SetState(RoomRotatorState.ExecuteCombinedRotation, FaceDirection.Left);
            Debug.Log("State switch");
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            State = RoomRotatorState.ConstantRotationOnly;
            SetState(RoomRotatorState.ExecuteCombinedRotation, FaceDirection.Right);
            Debug.Log("State switch");
        }
        Rotate(Time.deltaTime);
    }
}
