using UnityEngine;

public enum PlayerControlMappingNames
{
    Movement,
    Jump,
    Dash
}

[System.Serializable]
public struct PlayerControlMapping
{
    public string movementAxis;
    public string jumpKey;
    public string dashKey;

    public float GetAxisValue(PlayerControlMappingNames axis)
    {
        switch (axis)
        {
            case PlayerControlMappingNames.Movement:
                return Input.GetAxis(movementAxis);
            case PlayerControlMappingNames.Jump:
                return Input.GetAxis(jumpKey);
            case PlayerControlMappingNames.Dash:
                return Input.GetAxis(dashKey);
            default:
                return 0; // return neutral value in case casted int values are passed to the method
        }
    } 
}
