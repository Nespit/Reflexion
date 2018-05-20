using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject
{
    private Rigidbody m_PlayerRigidBody;
    private ParticleSystem m_PlayerParticleSystem;

    public PlayerSettings PlayerSettings;
    public PlayerAnimationSettings PlayerAnimationSettings;
    public PlayerControlMapping Controls;


    public void SetPlayerObject(GameObject player)
    {
        m_PlayerRigidBody = player.GetComponent<Rigidbody>();
        m_PlayerParticleSystem = player.GetComponentInChildren<ParticleSystem>();
    }

    public void TickPlayer()
    {
        if (m_PlayerRigidBody)
        {
            ProcessMovement();
            if (m_PlayerParticleSystem)
                ProcessParticleSystemState();
        }


    }

    private void ProcessMovement()
    {
        
    }

    private void ProcessParticleSystemState()
    {

    }

}
