using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool debugMode = false;
    public string playerTag = "Player";
    public bool gamePaused;

    private static GameManager _instance;
    public static GameManager Instance {
        get {

            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!_instance)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }

            return _instance;
        }
    }

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}
}
