using System.Collections;
using System.Collections.Generic;
using Math2D;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController2D : MonoBehaviour 
{

    [Header("Physics Check")]
    public float gravityModifier = 1f;
    public float minGroundNormalY = .65f;

    protected Vector2 _targetVelocity;
    protected bool _grounded;
    protected Vector2 _groundNormal;
    protected Rigidbody2D _rb;
    protected ContactFilter2D _contactFilter;
    protected Vector2 _velocity;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D>hitBufferList = new List<RaycastHit2D>(16);
    protected Animator _anim;


    protected const float MinMoveDistance = 0.01f;
    protected const float shellRadius = 0.01f;

    void OnEnable() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Start() 
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask (gameObject.layer));
    }

    void Update() 
    {
        _targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate() 
    {
        _velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        _grounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement (move, false);

        move = Vector2.up * deltaPosition.y;
        Movement (move, true);
    }

    public void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance) 
        {
            int count = _rb.Cast(move, _contactFilter, hitBuffer, distance + shellRadius);

            hitBufferList.Clear();
            for (int i = 0; i < count; i++) 
            {
                hitBufferList.Add (hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++) 
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                
                if(currentNormal.y > minGroundNormalY) 
                {
                    _grounded = true;

                    if (yMovement) 
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if(projection < 0) 
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rb.position = _rb.position + move.normalized * distance;
    }
    
}

