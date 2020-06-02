using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float prevUpdateSpeed = 0.04f;
    [SerializeField]
    float prevPrevUpdateSpeed = 0.01f;

    [HideInInspector]
    public Rigidbody2D rigidbody;
    
    public delegate void OnDead();
    public event OnDead deadEvent;

    public delegate void OnFinish();
    public event OnFinish finishEvent;
    
    public Vector3 prevPosition {get; private set;}
    public Vector3 prevPrevPosition {get; private set;}


    float deadTime = 0f;
    float deadTimeDuration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        prevPosition = transform.position;
        prevPrevPosition = transform.position;
    }

    public void hit()
    {
        if (Time.time > deadTime + deadTimeDuration)
        {
            deadTime = Time.time;
            deadEvent?.Invoke();
        }
    }

    private void Update() {
        
        Vector3 pos = transform.position;
        prevPosition += (pos - prevPosition) * prevUpdateSpeed;
        prevPrevPosition += (pos - prevPrevPosition) * prevPrevUpdateSpeed;
    }

    public void finish()
    {
        finishEvent?.Invoke();
    }
}
