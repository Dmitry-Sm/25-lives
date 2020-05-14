using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnDead();
    public event OnDead deadEvent;

    public delegate void OnFinish();
    public event OnFinish finishEvent;
    public Rigidbody2D rigidbody;

    float deadTime = 0f;
    float deadTimeDuration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void hit()
    {
        if (Time.time > deadTime + deadTimeDuration)
        {
            deadTime = Time.time;
            deadEvent?.Invoke();
        }
    }

    public void finish()
    {
        finishEvent?.Invoke();
    }
}
