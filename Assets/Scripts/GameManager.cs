using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance {
        get {
            if (Instance == null)
            {
                return new GameManager();
            }
            else
            {
                return Instance;
            }
        }
    }

    private bool _gamePaused;

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

    public bool IsGamePaused()
    {
        return _gamePaused;
    }
}
