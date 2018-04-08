using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Ground movement")]
    public float horizontalSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;

    [Header("Wall slide")]
    public float verticalSpeed = 2.5f;
    public float wallJumpHeight = 2f;
    public float wallJumpHorizontalForce = 0.25f;

    [Header("Dash")]
    public float dashHorizontalForce = 2f;
    public float dashVerticalForce = 2f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 0.25f;

    [Header("Gravity")]
    public float jumpGravityScale = 0f;
    public float fallGravityScale = 4f;

    private float _playerWidth = 0f;
    private float _playerHeight = 0f;
    private Rigidbody2D _rb2d;
    private PlayerBehaviour _player;
    private bool _grounded = true;
    private bool _canJump = true;
    private bool _canDash = true;
    private bool _wallDetected = false;
    private Side _touchingSide = Side.none;
    private PhysicState _physicState = PhysicState.ground;

    private float _lastJumpTime = 0f;
    private float _lastWallJumpTime = 0f;

    private float _lastDashTime = 0f;

    private enum Side
    {
        right,
        left,
        none
    }

    private enum PhysicState
    {
        ground,
        jump,
        fall,
        slide,
        dash
    }

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _playerHeight = GetComponent<SpriteRenderer>().bounds.size.y;
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
                else
                {
                    // If the player is in air, touching a wall, they can jump
                    if (_wallDetected && !_grounded)
                    {
                        WallJump(_touchingSide);
                    }
                }
            }

            // Dash control
            if (Input.GetButtonDown("B_" + _player.id))
            {
                if(_canDash)
                {
                    bool directionFound = false;

                    float xAxis = Input.GetAxisRaw("L_XAxis_" + _player.id);
                    float yAxis = -Input.GetAxisRaw("L_YAxis_" + _player.id);

                    if(xAxis != 0)
                    {
                        if (xAxis >= 0.5)
                        {
                            Dash(new Vector2(1, 0));
                            directionFound = true;
                        }
                        else if (xAxis <= -0.5)
                        {
                            Dash(new Vector2(-1, 0));
                            directionFound = true;
                        }
                    }

                    if(yAxis != 0)
                    {
                        if (yAxis >= 0.5)
                        {
                            Dash(new Vector2(0, 1));
                            directionFound = true;
                        }
                        else if (yAxis <= -0.5)
                        {
                            Dash(new Vector2(0, -1));
                            directionFound = true;
                        }
                    }

                    // TODO : Change that for facing direction
                    // If no direction is given we dash to the left
                    if (!directionFound)
                    {
                        Dash(new Vector2(1, 0));
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gamePaused)
        {
            // Update physic state
            if(Time.time <= _lastDashTime + dashDuration && !_canDash)
            {
                _physicState = PhysicState.dash;
            }
            else if (_rb2d.velocity.y < 0 && _touchingSide == Side.none)
            {
                _physicState = PhysicState.fall;
            }
            else if (_rb2d.velocity.y > 0)
            {
                _physicState = PhysicState.jump;
            }
            else if (_touchingSide != Side.none)
            {
                _physicState = PhysicState.slide;
            }
            else
            {
                _physicState = PhysicState.ground;
            }

            // Update gravity scale according to the phyisc state
            switch (_physicState)
            {
                case PhysicState.ground:
                    _rb2d.gravityScale = 1;
                    break;

                case PhysicState.jump:
                    _rb2d.gravityScale = jumpGravityScale;
                    break;

                case PhysicState.fall:
                    _rb2d.gravityScale = fallGravityScale;
                    break;

                case PhysicState.slide:
                    _rb2d.gravityScale = 0;
                    break;

                case PhysicState.dash:
                    _rb2d.gravityScale = 0;
                    break;

                default:
                    _rb2d.gravityScale = 1;
                    break;
            }

            // Check dash cooldown
            if (Time.time > _lastDashTime + dashDuration + dashCooldown && !_canDash)
            {
                _canDash = true;
            }

            // Get axis inputs
            float xAxis = Input.GetAxisRaw("L_XAxis_" + _player.id);
            float yAxis = -Input.GetAxisRaw("L_YAxis_" + _player.id);

            // Reset speed and apply new speed
            if (Time.time >= _lastWallJumpTime + 0.05f && _physicState != PhysicState.dash)
            {
                _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
                if (xAxis != 0f)
                {
                    _rb2d.velocity = new Vector2(horizontalSpeed * xAxis, _rb2d.velocity.y);
                }
            }
            else
            {
                _rb2d.velocity = new Vector2(_rb2d.velocity.x, _rb2d.velocity.y);
            }

            if (_physicState == PhysicState.slide)
            {
                _rb2d.velocity = new Vector2(horizontalSpeed * xAxis, (_rb2d.velocity.y) + verticalSpeed * yAxis);
            }

            // Check if the player collide with a wall on one side
            _wallDetected = false;
            _touchingSide = Side.none;

            List<RaycastHit2D> rightSideHits = new List<RaycastHit2D>();
            rightSideHits.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)), new Vector2(1, 0), (_playerWidth / 2f) + 0.2f));
            rightSideHits.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)), new Vector2(1, 0), (_playerWidth / 2f) + 0.2f));

            foreach (var hit in rightSideHits)
            {
                if (hit.collider != null && !hit.collider.gameObject.CompareTag(GameManager.Instance.playerTag))
                {
                    _touchingSide = Side.right;
                    _wallDetected = true;
                    // Debug contact point
                    if (GameManager.Instance.debugMode)
                    {
                        Debugger.Instance.DrawCross(hit.point);
                    }
                }
            }

            // Debug raycast
            if (GameManager.Instance.debugMode)
            {
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)),
                               new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)) + new Vector2((_playerWidth / 2f) + 0.2f, 0f),
                               Color.red);
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)),
                               new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)) + new Vector2((_playerWidth / 2f) + 0.2f, 0f),
                               Color.red);
            }

            List<RaycastHit2D> leftSideHits = new List<RaycastHit2D>();
            leftSideHits.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)), new Vector2(-1, 0), (_playerWidth / 2f) + 0.2f));
            leftSideHits.AddRange(Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)), new Vector2(-1, 0), (_playerWidth / 2f) + 0.2f));
            foreach (var hit in leftSideHits)
            {
                if (hit.collider != null && !hit.collider.gameObject.CompareTag(GameManager.Instance.playerTag))
                {
                    _touchingSide = Side.left;
                    _wallDetected = true;
                    // Debug contact point
                    if (GameManager.Instance.debugMode)
                    {
                        Debugger.Instance.DrawCross(hit.point);
                    }
                }
            }
            // Debug raycast
            if (GameManager.Instance.debugMode)
            {
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)),
                               new Vector2(transform.position.x, transform.position.y + ((2f / 3f) * _playerHeight / 2f)) + new Vector2((-_playerWidth / 2f) - 0.2f, 0f),
                               Color.red);
                Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)),
                               new Vector2(transform.position.x, transform.position.y - ((2f / 3f) * _playerHeight / 2f)) + new Vector2((-_playerWidth / 2f) - 0.2f, 0f),
                               Color.red);
            }

            _grounded = false;
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
                        _grounded = true;

                        // Debug contact point
                        if (GameManager.Instance.debugMode)
                        {
                            Debugger.Instance.DrawCross(hit.point);
                        }
                    }
                }
            }

            print(_rb2d.velocity);
        }
    }

    private void Jump()
    {
        _canJump = false;

        _rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

        _lastJumpTime = Time.time;
    }

    private void Dash(Vector2 direction)
    {
        _canDash = false;

        _rb2d.velocity = Vector2.zero;

        _rb2d.AddForce(new Vector2(dashHorizontalForce * direction.x, dashVerticalForce * direction.y), ForceMode2D.Impulse);

        _lastDashTime = Time.time;
    }

    private void WallJump(Side side)
    {
        switch (side)
        {
            case Side.right:
                _rb2d.AddForce(new Vector2(-wallJumpHorizontalForce, wallJumpHeight), ForceMode2D.Impulse);
                _lastWallJumpTime = Time.time;
                break;

            case Side.left:
                _rb2d.AddForce(new Vector2(wallJumpHorizontalForce, wallJumpHeight), ForceMode2D.Impulse);
                _lastWallJumpTime = Time.time;
                break;

            case Side.none:
                break;
        }
    }
}
