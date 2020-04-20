﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().hit();
        }
    }
}