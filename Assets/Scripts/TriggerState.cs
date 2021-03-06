﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerState : MonoBehaviour
{
    [HideInInspector] 
    public bool trigger { get => _trigger; }
    public List<Collider2D> triggeredObjects = new List<Collider2D>();
    private bool _trigger;
    // public bool t {get};
    private int num = 0;

    private void Start() {
        _trigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        num++;
        triggeredObjects.Add(other);
        _trigger = num > 0;
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        num--;
        triggeredObjects.Remove(other);
        _trigger = num > 0;
    }

}
