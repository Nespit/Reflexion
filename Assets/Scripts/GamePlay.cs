using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {

	[SerializeField]
	private RenderTexture tileRenderTexture;
	private RenderTexture currentActiveRT;
	private Texture2D tileTexture2D;
	private Color[] tileColors;

	public Color playerColorA;
	public Color playerColorB;

	public int pixelCountA = 0;
	public int pixelCountB = 0;

	public Text scoreA;
	public Text scoreB;

	Coroutine coroutine;
	WaitForEndOfFrame waitEndOfFrame;

	void Start () {
		waitEndOfFrame = new WaitForEndOfFrame ();
	}
		
	void Update () 
	{
		//Read the pixels of the VideoPlayer and store their color values in the vpFrameColors[]
		if(coroutine==null)
		{
			if (tileTexture2D == null)
				tileTexture2D = new Texture2D(tileRenderTexture.width, tileRenderTexture.height);

			coroutine = StartCoroutine (GetPixelsVP ());
		}

		//Get the scores.
		if (tileColors != null) 
		{
			for (int i = 0; i < tileColors.Length; i++) 
			{
				if (tileColors [i] == playerColorA)
					pixelCountA+=1;
				else if(tileColors [i] == playerColorB)
					pixelCountB+=1;
			}
		}

		scoreA.text = pixelCountA.ToString();
		scoreB.text = pixelCountB.ToString();
	}

	IEnumerator GetPixelsVP()
	{
		yield return waitEndOfFrame;

		pixelCountA = 0;
		pixelCountB = 0;

		// Remember currently active render texture
		currentActiveRT = RenderTexture.active;

		// Set the supplied RenderTexture as the active one
		RenderTexture.active = tileRenderTexture;

		// Create a new Texture2D and read the RenderTexture image into it
		tileTexture2D.ReadPixels(new Rect(0, 0, tileTexture2D.width, tileTexture2D.height), 0, 0);

		//Read the pixels
		tileColors = tileTexture2D.GetPixels ();

		// Restore previously active render texture
		RenderTexture.active = currentActiveRT;

		coroutine = null;
	}
}
