using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRenderTextures : MonoBehaviour {

	[SerializeField]
	private RenderTexture tileRenderTexture;

	// Use this for initialization
	void Start () {
		RenderTexture.active = tileRenderTexture;
		GL.Clear(false, true, Color.clear);
        RenderTexture.active = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	void OnPreRender ()
//	{
//		RenderTexture currentActiveRT;
//
//		currentActiveRT = RenderTexture.active;
//		RenderTexture.active = tileRenderTexture;
//		GL.Clear(false, true, Color.clear);
//		RenderTexture.active = currentActiveRT;
//	}
}
