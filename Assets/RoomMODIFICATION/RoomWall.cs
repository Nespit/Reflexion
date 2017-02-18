using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWall : MonoBehaviour {

    private bool m_pulse;
    private bool m_touchMe;
    private MeshRenderer m_renderer;
    [SerializeField]
    private Camera m_selfCamera;
    /// <summary>
    /// Was I touched?
    /// </summary>
    public bool Touched
    {
        get
        {
            return m_touchMe;
        }
    }

    /// <summary>
    /// Use to update lerp color after toggling lerping
    /// </summary>
    public Color LerpColor
    {
       set
        {
            m_renderer.sharedMaterial.SetColor("_EmissionColor", value);
        }
    }
    // Use this for initialization
    void Start () {

        if (m_selfCamera == null)
            throw new MissingReferenceException("Camera missing on gameObject.");
        m_renderer = GetComponent<MeshRenderer>();
        //clear cameras render texture;
        var rend = m_selfCamera.targetTexture;
        rend.DiscardContents();
        RenderTexture.active = rend;
        GL.Clear(false, false, Color.black);
        RenderTexture.active = null;
        RoomInteractive.instance.SubscribeEvent(this);

    }
	
    /// <summary>
    /// Toggles camera on and off
    /// </summary>
    void ToggleCamera()
    {
        m_selfCamera.enabled = !m_selfCamera.enabled;
    }

    /// <summary>
    /// Toggles pulse on and off
    /// </summary>
    void TogglePulse()
    {
        m_pulse = !m_pulse;
        if (!m_pulse)
            m_renderer.sharedMaterial.SetColor("_EmissionColor", Color.black);

    }

    private void OnCollisionEnter(Collision collision)
    {
        RoomInteractive.instance.TriggerEvent(this,Vector3.one);
    }


    private void OnDisable()
    {
        m_renderer.sharedMaterial.SetColor("_EmissionColor", Color.black);
    }
}
