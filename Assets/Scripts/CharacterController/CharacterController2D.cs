using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    // available movements


    // states: grounded, inAir

    // long range attack
    // short range attack
    // dash
    // jump
    // attack + dash = dash attack
    // attack + up = uppercut
    // attack + down = downcut
    //

    [Header("Movement Properties")]
    public float speed = 1f;
    public float jumpVelocity = 10f;

    [Header("Physics Check")]
    public float groundDistance = 1f;
    public float feetDistance = 1f;

    Rigidbody2D rb;

    bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        PhysicsChecks();

        float horizontal = Input.GetAxis("Horizontal");
        float horizontalVelocity = horizontal * speed;

        float vertical = Input.GetAxis("Vertical");

        //canJump = isGrounded
        bool canJump = true;
        if (jumpPressed && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
        jumpPressed = false;
        

        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
    }

    void PhysicsChecks()
    {
        
        // is grounded
        // touching wall front
        // touching wall back
        // touching ceiling
    }

    void CheckGrounded()
    {
        // float floorOrigin = transform.position
        // Physics2D.BoxCastAll()
    }
}
