using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

	private MeshRenderer meshRenderer;

	[SerializeField]
	private RenderTexture activeTexture;
	private Texture2D inactiveTexture;

	[SerializeField]
	private Material activeMat;
	private Material inactiveMat;

	[SerializeField]
	private GameObject renderCamera;

	private Coroutine m_activateRenderTextureMat;
	private Coroutine m_activateInactiveTextureMat;

	public bool isActive;
	private bool isReady = false;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		activeTexture.DiscardContents ();
		inactiveTexture = new Texture2D(activeTexture.width, activeTexture.height);
		inactiveMat = new Material (Shader.Find("Standard"));
		inactiveMat.mainTexture = inactiveTexture;

		if (isActive) 
		{
			renderCamera.SetActive (true);
			ActivateRenderTexture ();
			isReady = true;

		} else 
		{
			renderCamera.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive && !isReady) 
		{
			activeTexture.DiscardContents ();
			renderCamera.SetActive (true);
			ActivateRenderTexture ();
			isReady = true;
		} else if(!isActive && isReady)
		{
			renderCamera.SetActive (false);

			isReady = false;
		}	
	}

	#region Coroutines
	private void ActivateRenderTexture()
	{
		if (m_activateRenderTextureMat == null) 
			m_activateRenderTextureMat = StartCoroutine (Activate ());
		
	}

	IEnumerator Activate()
	{
		yield return new WaitForEndOfFrame ();
		meshRenderer.material = activeMat;
	}

	private void DeactivateRenderTextureMat()
	{
		if(m_activateInactiveTextureMat == null)
			m_activateInactiveTextureMat = StartCoroutine (Deactivate ());
	}

	IEnumerator Deactivate()
	{
		yield return new WaitForEndOfFrame ();
		meshRenderer.material = inactiveMat;
	}
	#endregion
}
