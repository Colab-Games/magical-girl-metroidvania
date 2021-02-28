using Math2D;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed = 2f;
    public float jumpVelocity = 10f;
    public float airHorizontalInertia = 5f;
    public float wallJumpVelocity = 5f;
    

    [Header("Physics Check")]
    public float groundDistance = 0.1f;
    public float feetDistance = 0.9f;
    public float wallDistance = 0.1f;
    public float wallGrabHeight = 1f;

    Rigidbody2D _rb;
    BoxCollider2D _bc;
    Animator _anim;
    int _collisionLayerMask;

    bool _jumpPressed = false;

    // Physics checks
    bool _isGrounded = false;
    bool _isTouchingWallFront = false;
    bool _isTouchingWallBack = false;

    float _originalGravity;
    float _originalXScale;
    int _direction = 1;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();

        _collisionLayerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);

        _originalGravity = _rb.gravityScale;
        _originalXScale = transform.localScale.x;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) {
            _jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        PhysicsChecks();

        float horizontal = Input.GetAxis("Horizontal");
        float xVelocity = horizontal * speed;

        float yVelocity = _rb.velocity.y;

        // Animation
        bool isMoving = xVelocity != 0;
        if (!isMoving) {
            _anim.SetBool("isRunning", false);
        } else {
            _anim.SetBool("isRunning", true);
        }

        bool isFalling = yVelocity < -0.2;
        if (isFalling) {
            _anim.SetBool("isFalling", true);
        } else {
            _anim.SetBool("isFalling", false);
        }

        bool isJumping = yVelocity > 0.7 && !_isGrounded;
        if (isJumping) {
            _anim.SetBool("isJumping", true);
        } else {
            _anim.SetBool("isJumping", false);
        }

        // Wall grab
        bool canGrab = yVelocity <= 0f && !_isGrounded && _isTouchingWallFront;
        if (Mathf.Abs(xVelocity) > 0f && canGrab) {
            yVelocity = 0;
            _rb.gravityScale = 0f;
        } else
            _rb.gravityScale = _originalGravity;

        // Air horizontal movement
        if (!_isGrounded) {
            float keptVelocity = _rb.velocity.x;
            
            if (Mathf.Abs(keptVelocity) > 0 || Mathf.Abs(xVelocity) > 0) {
                xVelocity = Mathf.Clamp(keptVelocity + ((xVelocity - _direction) / airHorizontalInertia), -speed, speed);
            }
        } else {
            _anim.SetBool("isFalling", false);
        }

        // Jump
        bool canJump = _isGrounded;
        if (_jumpPressed && canJump) {
            yVelocity += jumpVelocity;
        } else if (!_jumpPressed && _isGrounded){
            _anim.SetBool("isJumping", false);
        }


        // Wall Jump
        bool canWallJump = !_isGrounded && (_isTouchingWallFront || _isTouchingWallBack);
        if (_jumpPressed && canWallJump) {
            xVelocity = speed * (_isTouchingWallBack ? _direction : -_direction);
            yVelocity = wallJumpVelocity;
        }
        _jumpPressed = false;



        if (xVelocity * _direction < 0f)
            FlipCharacterDirection();
        _rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    void PhysicsChecks()
    {
        CheckGrounded();
        CheckTouchingWalls();
        // touching ceiling
    }

    void CheckGrounded()
    {
        // Check collision creating a box from the bottom of the character to groundDistance with width feetDistance
        Vector2 bottom = transform.position.ToVector2OnXY() + _bc.offset + (Vector2.down * _bc.size.y / 2);
        Vector2 floorOrigin = bottom + Vector2.down * (groundDistance / 2);
        Vector2 boxSize = new Vector2(feetDistance, groundDistance);
        Collider2D collision = Physics2D.OverlapBox(floorOrigin, boxSize, 0, _collisionLayerMask);

        // If collided with a valid collider then is grounded
        _isGrounded = collision != null;
        DrawDebug.DrawBox2D(floorOrigin.ToVector3OnXY(transform.position.z), boxSize, _isGrounded ? Color.red : Color.green);
    }

    void CheckTouchingWalls()
    {
        Vector2 bottom = transform.position.ToVector2OnXY() + _bc.offset + (Vector2.down * _bc.size.y / 2);
        Vector2 boxSize = new Vector2(wallDistance, wallGrabHeight);

        bool CheckWallCollision(Vector2 checkDirection)
        {
            Vector2 bottomDirection = bottom + (checkDirection * _bc.size.x / 2);
            Vector2 directedOrigin = bottomDirection + (Vector2.up * wallGrabHeight / 2) + (checkDirection * wallDistance / 2);
            Collider2D collision = Physics2D.OverlapBox(directedOrigin, boxSize, 0, _collisionLayerMask);

            bool collided = collision != null;
            DrawDebug.DrawBox2D(directedOrigin.ToVector3OnXY(transform.position.z), boxSize, collided ? Color.red : Color.green);
            return collided;
        }

        _isTouchingWallFront = CheckWallCollision(Vector2.right * _direction);
        _isTouchingWallBack = CheckWallCollision(Vector2.left * _direction);
    }

    void FlipCharacterDirection()
    {
        _direction *= -1;

        // Flip the characther by flipping the x scale
        Vector3 scale = transform.localScale;
        scale.x = _originalXScale * _direction;

        // Apply the new scale
        transform.localScale = scale;
    }
}
