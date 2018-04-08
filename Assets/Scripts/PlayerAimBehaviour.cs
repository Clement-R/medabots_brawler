using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimBehaviour : MonoBehaviour {

    [Range(1f, 10f)]
    public float aimSensitivity = 5f;

    private float _theta = 0f;
    private Vector3 _aimPosition = Vector3.zero;
    private PlayerBehaviour _player;
    private float _rotation;

    void Start()
    {
        _player = GetComponent<PlayerBehaviour>();
        _aimPosition = _player.transform.position + new Vector3(2f, 0f);
    }

    void Update ()
	{
        _aimPosition = _player.transform.position + new Vector3(2f, 0f);

        float yAxis = Input.GetAxisRaw("R_YAxis_" + _player.id);

        if (yAxis != 0f)
        {
            if(yAxis > 0)
            {
                _theta -= aimSensitivity;
            }
            else
            {
                _theta += aimSensitivity;
            }
        }

        _aimPosition = Quaternion.Euler(0, 0, _theta) * (_aimPosition - _player.transform.position) + _player.transform.position;

        Debugger.Instance.DrawCross(_aimPosition, Color.red);
    }
}
