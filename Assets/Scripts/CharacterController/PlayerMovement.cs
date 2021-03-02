using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterController2D 
{

    [Header("Movement Properties")]
    public float maxSpeed = 7f;
    public float jumpTakeOffSpeed = 10f;
    public PlayerInput playerInput;
    
    protected float _horizontal;
    protected float _vertical = 0f;

    private SpriteRenderer spriteRenderer;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ComputeVelocity()
    {   
        Vector2 move = Vector2.zero;

        move.x = _horizontal;


        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if(flipSprite) 
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        _anim.SetBool("isGrounded", _grounded);
        _anim.SetFloat("xVelocity", Mathf.Abs(_velocity.x) / maxSpeed);
        _anim.SetFloat("yVelocity", _velocity.y / maxSpeed);

        _targetVelocity = move * maxSpeed;
    }
    public void Move(InputAction.CallbackContext context) 
    {
        if (context.started || context.performed) {
            _horizontal = context.ReadValue<Vector2>().x;
        }

        if (context.canceled) {
            _horizontal = 0;
        }
    }
    public void Jump(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            _velocity.y = jumpTakeOffSpeed;
        }

        if (context.canceled) 
        {
            if(_velocity.y > 0) 
            {
                _velocity.y = _velocity.y * .5f;
            }
        } 
    }
    
}

