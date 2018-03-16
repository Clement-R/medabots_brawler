using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance {
        get {

            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!instance)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }

            return instance;
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
