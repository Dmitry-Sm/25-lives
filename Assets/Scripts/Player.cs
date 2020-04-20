using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnDead();
    public event OnDead deadEvent;

    public delegate void OnFinish();
    public event OnDead finishEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void hit()
    {
        deadEvent?.Invoke();
    }

    public void finish()
    {
        finishEvent?.Invoke();
    }
}
