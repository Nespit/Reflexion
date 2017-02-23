using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWall : MonoBehaviour {

    private bool m_pulse;
    private MeshRenderer m_renderer;
    [SerializeField]
    private Camera m_selfCamera;
    [SerializeField]
    private int m_ID;

    /// <summary>
    /// Get wall ID.
    /// </summary>
    public int ID
    {
        get
        {
            return m_ID;
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
        GL.Clear(false, true, Color.black);
        RenderTexture.active = null;
        RoomInteractive.instance.Subscribe(this);

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
        RoomInteractive.instance.TriggerEvent(this, collision.relativeVelocity);
    }


    private void OnDisable()
    {
        var rend = m_selfCamera.targetTexture;
        rend.DiscardContents();
        RenderTexture.active = rend;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = null;
        m_renderer.sharedMaterial.SetColor("_EmissionColor", Color.black);
    }
}
