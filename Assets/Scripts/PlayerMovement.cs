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
    public Vector2 wallJumpForce = new Vector2(25f, 15f);
    public Vector2 jumpVelocityPriorety = new Vector2(0.1f, 0.01f);

    [Header("Envirement Check Properties")]
    // public float footOfset = 0.4f;
    public float groundDistance = 0.1f;
    public LayerMask groundLayer;

    [Header("Status Flags")]
    public bool isOnGround;
    public bool isJumping;
    public bool isClimb;
    public bool isTouchWall;

    PlayerInput input;
    BoxCollider2D bodyCollider;
    Rigidbody2D rigidBody;

    Vector2 velocity;
    Vector2 wallJumpVelocity;
    Vector2 inputVelocity;
    float jumpTime;
    float wallJumpTime;
    float playerWidth;
    float playerHeight;
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
        WallJump();
        if (isOnGround && !isJumping && input.jumpPressed)
        {
            isOnGround = false;
            isJumping = true;

            jumpTime = Time.time + jumpHoldDuration;
            velocity.y += jumpForce;
        }

        if (isJumping && jumpTime <= Time.time)
        {
            isJumping = false;
        }

        velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
    }

    void WallJump()
    {
        RaycastHit2D leftWallCheck = Raycast(new Vector2(-playerWidth/2f, playerHeight/2f), Vector2.left, groundDistance);
        RaycastHit2D rightWallCheck = Raycast(new Vector2(playerWidth/2f, playerHeight/2f), Vector2.right, groundDistance);

        isTouchWall = leftWallCheck || rightWallCheck;

        if (!isOnGround && isTouchWall && input.jumpPressed && Time.time > wallJumpTime + wallJumpHoldDuration)
        {
            wallJumpTime = Time.time;
            wallJumpVelocity = wallJumpForce;
            wallJumpVelocity.x *= leftWallCheck ? 1f : -1f;
        }
        
        if (!isOnGround)
        {   
            float progress = 1f - (Time.time - wallJumpTime) / wallJumpDuration;
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
