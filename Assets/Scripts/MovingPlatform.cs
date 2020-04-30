using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector2 position = new Vector2(0f, 0f);
    private Rigidbody2D rigidbody;
    private Rigidbody2D mrb;
    private List<Rigidbody2D> connectedBodies;


    void Start()
    {
        position = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        connectedBodies = new List<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector2 offset = new Vector2(Mathf.Sin(Time.time * 4f) * 0.15f, 0f); 
        position += offset;

        foreach (var rb in connectedBodies)
        {
            rb.position += offset;
        }

        rigidbody.MovePosition(position);
    }


    private void OnCollisionEnter2D(Collision2D other) {
        float ny = other.GetContact(0).normal.y;
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb && ny < 0f)
        {
            connectedBodies.Remove(rb);
            connectedBodies.Add(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb)
        {
            connectedBodies.Remove(rb);
        }
    }
}
