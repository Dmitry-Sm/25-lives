using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField] float speed;
    [SerializeField] float waitngTime = 1f;
    
    Vector3 position;
    Rigidbody2D rigidbody;
    SliderJoint2D joint;
    List<Rigidbody2D> connectedBodies;
    Transform target;
    int targetIndex = 0;

    int timer = 1;
    float arrivalTime = 0f;

    void Start()
    {
        position = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        joint = GetComponent<SliderJoint2D>();
        connectedBodies = new List<Rigidbody2D>();
        target = targets[targetIndex];
    }


    void FixedUpdate()
    {
        Vector3 difference = target.position - position;
        Vector3 offset = Vector3.ClampMagnitude(difference, speed);

        if (Vector3.Distance(position, target.position) < speed)
        {
            targetIndex = (targetIndex + 1) % targets.Count;
            target = targets[targetIndex];
            arrivalTime = Time.time;
        }
        
        if (Time.time > arrivalTime + waitngTime)
        {
            position += offset;
            if (rigidbody)
            {
                rigidbody.MovePosition(position);
            }
            else 
            {
                transform.position = position;
            }
            
            foreach (var rb in connectedBodies)
            {
                rb.position += new Vector2(offset.x, offset.y);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other) {
        float normal = other.GetContact(0).normal.y;
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        
        if (rb && !connectedBodies.Contains(rb) && normal < 0f)
        {   

            if (other.gameObject.tag == "Player")
            {
                connectedBodies.Add(rb);
                joint.connectedBody = rb;
                PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
                pm.jumpEvent += RemoveConnection;
                pm.fallEvent += RemoveConnection;
                rb.interpolation = RigidbodyInterpolation2D.None;
            }
        }
    }

    void RemoveConnection(PlayerMovement pm)
    {
        connectedBodies.Remove(pm.rigidBody);
        joint.connectedBody = null;
        pm.jumpEvent -= RemoveConnection;
        pm.fallEvent -= RemoveConnection;
        pm.rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
