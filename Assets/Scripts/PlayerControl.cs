using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] Vector2 sideJumpSpeed = new Vector2(30f, 15f);
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float defaultGravity = 5f;
    [SerializeField] float wallSlideGravity = 1f;
    [SerializeField] TriggerState bottom;
    [SerializeField] TriggerState left;
    [SerializeField] TriggerState right;
    [SerializeField] Rigidbody2D testPlat;

    private Rigidbody2D rigidbody;
    private Vector2 position;
    private Vector2 velocity;
    private SpriteRenderer sr;
    private float sideVelocity = 0f;

    


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, 0);
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal") * moveSpeed;
        hor *= 1f - Mathf.SmoothStep(0f, 1f, 1.4f * Mathf.Abs(sideVelocity) / sideJumpSpeed.x);
        velocity.x = hor + sideVelocity;

        sideVelocity *= 0.8f;

        velocity.y = rigidbody.velocity.y;
        // velocity += testPlat.velocity;
        

        if (Input.GetButtonDown("Jump") && bottom.trigger)
        {
            rigidbody.MovePosition(rigidbody.position + Vector2.up);
            velocity.y += jumpSpeed;
        }

        if ((left.trigger || right.trigger) && velocity.y < -0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                sideVelocity = left.trigger ? sideJumpSpeed.x : -sideJumpSpeed.x;
                velocity.y += sideJumpSpeed.y;
            }
            else
            {
                rigidbody.drag = 10f;
            }
        }
        else
        {
            rigidbody.drag = 0;
        }
    
        rigidbody.velocity = velocity;
    }
}
