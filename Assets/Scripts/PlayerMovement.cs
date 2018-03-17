using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float horizontalSpeed = 10f;
    public float jumpVelocity = 10f;

    private Rigidbody2D rb2d;
    private PlayerBehaviour _player;
    private bool _canJump = true;

    private float _lastJumpTime = 0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start ()
	{
        _player = GetComponent<PlayerBehaviour>();
    }

	void Update ()
	{
        if (!GameManager.Instance.gamePaused)
        {
            // Jump control
            if(Input.GetButtonDown("A_" + _player.id))
            {
                if(_canJump)
                {
                    Jump();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gamePaused)
        {
            // Reset speed and apply new speed
            rb2d.velocity = Vector2.zero;

            float xAxis = Input.GetAxisRaw("L_XAxis_" + _player.id);

            if (xAxis != 0f)
            {
                rb2d.velocity = new Vector2(horizontalSpeed * xAxis, 0);
            }

            // Check if the player is in air
            if (!_canJump && Time.time >= _lastJumpTime + 0.05f)
            {
                // Debug raycast
                if (GameManager.Instance.debugMode)
                {
                    Debug.DrawLine(transform.position, transform.position + new Vector3(0, -0.55f), Color.red);
                }

                // Check if the player touch the ground
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 0.55f);
                foreach (var hit in hits)
                {
                    if (hit.collider != null && !hit.collider.gameObject.CompareTag(GameManager.Instance.playerTag))
                    {
                        Debug.Log(hit.collider.name);
                        _canJump = true;

                        // Debug contact point
                        if (GameManager.Instance.debugMode)
                        {
                            Debugger.Instance.DrawCross(hit.point);
                        }
                    }
                }
            }
        }
    }

    private void Jump()
    {
        _canJump = false;
        rb2d.AddForce(new Vector2(0, jumpVelocity));
        _lastJumpTime = Time.time;
    }
}
