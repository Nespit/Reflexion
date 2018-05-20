using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    public Player[] Players;

    private string[] m_CurrentJoystickCount;

    // Use this for initialization
	void Awake ()
    {

	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;

        PoolJoystickChange();
    }

	// Update is called once per frame
	void Update () {

        PoolJoystickChange();
        ProcessPlayers();
	}

    void PoolJoystickChange()
    {
        m_CurrentJoystickCount = Input.GetJoystickNames();
    }

    void ProcessPlayers()
    {
        foreach (Player player in Players)
            player.TickPlayer();
    }
}
