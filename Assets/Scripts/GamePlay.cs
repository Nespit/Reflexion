using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {

    internal sealed class ColorScore
    {
        public int pixelScoreA = 0;
        public int pixelScoreB = 0;
    }

    private Dictionary<int, ColorScore> m_wallTextures;
    private RenderTexture m_currentTexture;
	private Texture2D tileTexture2D;

	public Color playerColorA;
	public Color playerColorB;

	private int pixelCountA = 0;
	private int pixelCountB = 0;
    [SerializeField]
    private float countDownValue;

	public Text scoreA;
	public Text scoreB;
    public Text countDownText;

	void Start () {
        m_wallTextures = new Dictionary<int, ColorScore>();
    }
		
	void Update () 
	{
        if(countDownValue > 0)
        {
            if(RoomInteractive.instance.CurrentWall != null)
            {
                var currentWall = RoomInteractive.instance.CurrentWall;
                ColorScore currentScore = null;
                if(!m_wallTextures.TryGetValue(currentWall.ID,out currentScore))
                {
                    currentScore = new ColorScore();
                    m_wallTextures.Add(currentWall.ID, currentScore);
                }

                GetPixelsVP(currentWall.wallTexture, ref currentScore);
                pixelCountA = pixelCountB = 0;
                foreach(KeyValuePair<int,ColorScore> pair in m_wallTextures)
                {
                    pixelCountA += pair.Value.pixelScoreA;
                    pixelCountB += pair.Value.pixelScoreB;
                }
            }
            if (scoreA != null)
                scoreA.text = pixelCountA.ToString();
            if (scoreB != null)
                scoreB.text = pixelCountB.ToString();
            highlightHighestPixelCount();

            countDownValue -= Time.deltaTime;
            countDownText.text = Mathf.RoundToInt(countDownValue).ToString();
         }
    }

    private void GetPixelsVP(RenderTexture tex, ref ColorScore editableScore)
    {
        // Set the supplied RenderTexture as the active one
        RenderTexture.active = tex;

        if (tileTexture2D == null)
            tileTexture2D = new Texture2D(tex.width, tex.height);
        else if (tileTexture2D.width != tex.width || tileTexture2D.height != tex.height)
            tileTexture2D = new Texture2D(tex.width, tex.height);

        // Create a new Texture2D and read the RenderTexture image into it
        tileTexture2D.ReadPixels(new Rect(0, 0, tileTexture2D.width, tileTexture2D.height), 0, 0);

		//Read the pixels
		var tileColors = tileTexture2D.GetPixels ();
        
		// Restore previously active render texture
		RenderTexture.active = null;
        editableScore.pixelScoreA = 0;
        editableScore.pixelScoreB = 0;
        foreach(Color pixel in tileColors)
        {
            if (pixel == playerColorA)
                editableScore.pixelScoreA++;
            else if (pixel == playerColorB)
                editableScore.pixelScoreB++;
        }
	}

    private void highlightHighestPixelCount()
    {
        if(pixelCountA>pixelCountB)
        {
            Debug.Log("Score A > B");
            scoreA.fontSize = 24;
            scoreA.fontStyle = FontStyle.Bold;
            scoreB.fontSize = 14;
            scoreB.fontStyle = FontStyle.Normal;
        } else if (pixelCountB>pixelCountA)
        {
            Debug.Log("Score B > A");
            scoreB.fontSize = 24;
            scoreB.fontStyle = FontStyle.Bold;
            scoreA.fontSize = 14;
            scoreA.fontStyle = FontStyle.Normal;
        } else
            Debug.Log("Score A == B");
            scoreA.fontSize = 14;
            scoreA.fontStyle = FontStyle.Normal;
            scoreB.fontSize = 14;
            scoreA.fontStyle = FontStyle.Normal;
    }
}
