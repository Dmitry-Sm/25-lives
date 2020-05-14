using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField] float speed;
    
    Vector3 position = new Vector3(0f, 0f, 0f);
    Rigidbody2D rigidbody;
    Rigidbody2D mrb;
    SliderJoint2D joint;
    List<Rigidbody2D> connectedBodies;
    Transform target;
    int timer = 1;
    int targetIndex = 0;

    float waitngTime = 1f;
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
        Vector3 offset = target.position - position;
        Vector3 dir = Vector3.ClampMagnitude(offset, speed * Time.deltaTime);

        if (Vector3.Distance(position, target.position) < 0.1f)
        {
            target = targets[targetIndex];
            targetIndex = (targetIndex + 1) % targets.Count;
            arrivalTime = Time.time;
        }
        
        if (Time.time > arrivalTime + waitngTime)
        {
            position += dir;
            rigidbody.MovePosition(position);
            
            foreach (var rb in connectedBodies)
            {
                rb.position += new Vector2(dir.x, dir.y);
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D other) {
        float ny = other.GetContact(0).normal.y;
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        
        if (rb && !connectedBodies.Contains(rb) && ny < 0f)
        {   
            if (other.gameObject.tag == "Player")
            {
                connectedBodies.Add(rb);
                Debug.Log("Connect");
                joint.connectedBody = rb;
                PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
                pm.jumpEvent += RemoveConnection;
                pm.fallEvent += RemoveConnection;
                rb.interpolation = RigidbodyInterpolation2D.None;
            }
            
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb)
        {
            if (other.gameObject.tag != "Player")
            {
                connectedBodies.Remove(rb);
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
