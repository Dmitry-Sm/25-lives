using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] TriggerState bottom;

    private Rigidbody2D rigidbody;
    private Vector2 position;
    private Vector2 velocity;
    private SpriteRenderer sr;

    


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, 0);
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        velocity.x = hor * moveSpeed;

        velocity.y = rigidbody.velocity.y;
        if (Input.GetButtonDown("Jump") && bottom.trigger)
        {
            rigidbody.MovePosition(rigidbody.position + Vector2.up);
            velocity.y += jumpSpeed;
        }
    
        rigidbody.velocity = velocity;
    }
}
