using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{

    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool jumpHeld;

    bool readyToClear;

    
    void Update()
    {
        if (readyToClear)
        {
            ClearInput(); 
        }

        ProcessInputs();
    }


    void FixedUpdate() 
    {
        readyToClear = true;        
    }


    void ClearInput()
    {
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;

        readyToClear = false;
    }


    void ProcessInputs()
    {
        horizontal += Input.GetAxis("Horizontal");
        horizontal = Mathf.Sign(horizontal) * Mathf.Min(1f, Mathf.Abs(horizontal));

        jumpPressed |= Input.GetButtonDown("Jump");
        jumpHeld |= Input.GetButton("Jump");
    }
}
