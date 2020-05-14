using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool drawDebugRaycast = true;

    [Header("Movement Properties")]
    public float speed = 8f;
    public float maxFallSpeed = 25f;

    [Header("Jump Properties")]
    public float jumpForce = 8f;
    public float jumpHoldForce = 0.1f;
    public float jumpHoldDuration = 0.1f;
    public float wallJumpHoldDuration = 0.1f;
    public float wallJumpDuration = 0.4f;
    public float wallMaxFallSpeed = 5f;
    public float cayotTime = 0.2f;
    public Vector2 wallJumpForce = new Vector2(25f, 15f);

    [Header("Envirement Check Properties")]
    // public float footOfset = 0.4f;
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    [Header("Status Flags")]
    public bool isOnGround;
    public bool isJumping;
    public bool isClimb;
    public bool isTouchWall;

    public delegate void EventType(PlayerMovement pm);
    public event EventType jumpEvent;
    public event EventType fallEvent;

    public Rigidbody2D rigidBody {get; private set;}

    PlayerInput input;
    BoxCollider2D bodyCollider;

    Vector2 velocity;
    Vector2 wallJumpVelocity;
    Vector2 inputVelocity;
    float jumpTime;
    float wallJumpTime;
    float playerWidth;
    float playerHeight;
    float onGroundTime;
    float onWallTime;
    bool lastWallIsLeft;
    float maxVelocity = 0f;
    int direction = 1;
    const float smallAmount = 0.05f;
        

    void Start()
    {
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        playerWidth = bodyCollider.size.x;
        playerHeight = bodyCollider.size.y;
        velocity = new Vector2(0f, 0f);
        wallJumpVelocity = new Vector2(0f, 0f);
        inputVelocity = new Vector2(0f, 0f);
        wallJumpTime = Time.time;
        onGroundTime = Time.time;
    }

    void FixedUpdate() {
        velocity = new Vector2(0f, rigidBody.velocity.y);

        PhysicsCheck();
        GroundMovement();
        MidAirMovement();

        rigidBody.velocity = velocity;
    }


    void PhysicsCheck()
    {
        RaycastHit2D leftGroundCheck = Raycast(new Vector2(-playerWidth/2f, 0f), Vector2.down, groundDistance);
        RaycastHit2D rightGroundCheck = Raycast(new Vector2(playerWidth/2f, 0f), Vector2.down, groundDistance);

        isOnGround = leftGroundCheck || rightGroundCheck;

        if (isOnGround)
        {
            isJumping = false;
        }
        else
        {
            Fall();
        }
    }

    void GroundMovement()
    {
        velocity.x = speed * input.horizontal;
        // Debug.Log(input.horizontal);

        if (inputVelocity.x * direction < 0f)
        {
            FlipDirection();
        }
    }

    void MidAirMovement()
    {
        float time = Time.time;
        WallJump();
        if (isOnGround)
        {
            onGroundTime = time;
        }

        if (!isJumping && input.jumpPressed && time < onGroundTime + cayotTime)
        {
            Jump();
        }

        if (isJumping && input.jumpHeld && velocity.y > 0f && time < jumpTime + jumpHoldDuration)
        {
            velocity.y += jumpHoldForce;
        }
        if (velocity.y > maxVelocity)
        {
            maxVelocity = velocity.y;
            Debug.Log(maxVelocity);
        }

        // if (isJumping && jumpTime <= time)
        // {
        //     // isJumping = false;
        // }

        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
    }

    void WallJump()
    {
        RaycastHit2D leftWallCheck = Raycast(new Vector2(-playerWidth/2f, playerHeight/2f), Vector2.left, groundDistance);
        RaycastHit2D rightWallCheck = Raycast(new Vector2(playerWidth/2f, playerHeight/2f), Vector2.right, groundDistance);
        float time = Time.time;

        isTouchWall = leftWallCheck || rightWallCheck;
        if (isTouchWall)
        {
            lastWallIsLeft = leftWallCheck;
            onWallTime = time;
        }

        if (!isOnGround && input.jumpPressed && time > wallJumpTime + wallJumpHoldDuration && time < onWallTime + cayotTime)
        {
            wallJumpTime = time;
            wallJumpVelocity = wallJumpForce;
            wallJumpVelocity.x *= lastWallIsLeft ? 1f : -1f;
        }
        
        if (!isOnGround)
        {   
            float progress = 1f - (time - wallJumpTime) / wallJumpDuration;
            velocity = Vector2.Lerp(velocity, wallJumpVelocity, progress);
            wallJumpVelocity *= 0.9f;
        }
        else 
        {
            wallJumpVelocity *= 0f;
        }
        
        if (!isOnGround && isTouchWall && rigidBody.velocity.y < -0.1f)
        {
            velocity.y = Mathf.Max(velocity.y, -wallMaxFallSpeed);
        }
    }


    void Jump()
    {
        isOnGround = false;
        isJumping = true;

        jumpTime = Time.time;
        velocity.y = jumpForce;

        jumpEvent?.Invoke(this);
    }

    void Fall()
    {
        fallEvent?.Invoke(this);
    }


    void FlipDirection()
    {
        direction *= -1;
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        return Raycast(offset, rayDirection, length, groundLayer);
    }


    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        if (drawDebugRaycast)
        {
            Color color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        return hit;
    }
}
