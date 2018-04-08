using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour {

    private Color _defaultColor = Color.white;

    private static Debugger _instance;
    public static Debugger Instance
    {
        get
        {

            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(Debugger)) as Debugger;

                if (!_instance)
                {
                    Debug.LogError("There needs to be one active Debugger script on a GameObject in your scene.");
                }
            }

            return _instance;
        }
    }

    public void DrawCross(Vector2 origin, Color ? color = null)
    {
        Debug.DrawLine(new Vector2(origin.x - 0.5f, origin.y + 0.5f), new Vector2(origin.x + 0.5f, origin.y - 0.5f), color ?? _defaultColor);
        Debug.DrawLine(new Vector2(origin.x + 0.5f, origin.y + 0.5f), new Vector2(origin.x - 0.5f, origin.y - 0.5f), color ?? _defaultColor);
    }
}
