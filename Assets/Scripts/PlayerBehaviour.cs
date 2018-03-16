using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public int id
    {
        get
        {
            return _id;
        }
    }

    private int _id = 1;

    void Start ()
	{
	}

	void Update ()
	{
        if (!GameManager.Instance.IsGamePaused())
        {
            // TODO : Check life
        }
    }
}
