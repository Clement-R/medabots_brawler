using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private PlayerBehaviour _player;
    private bool _canJump = true;

	void Start ()
	{
        _player = GetComponent<PlayerBehaviour>();
    }

	void Update ()
	{
        if (!GameManager.Instance.IsGamePaused())
        {
            // TODO : Check input
            if(Input.GetAxisRaw("R_XAxis_" + _player.id) > 0f)
            {

            }

            if(Input.GetButtonDown("A_" + _player.id))
            {
                if(_canJump)
                {
                    Jump();
                }
            }
        }
    }

    private void Jump()
    {

    }
}
