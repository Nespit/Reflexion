using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class PlayerAnimationSettings : ScriptableObject {

    [System.Serializable]
    private struct TresholdedTextures
    {
        public int Threshold;
        public Texture Texture;
    }

    [SerializeField]
    private TresholdedTextures[] m_Textures;

    [SerializeField]
    private float m_AnimationTime;

}
