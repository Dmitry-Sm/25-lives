using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    Material material2;
    [SerializeField]
    Player player;


    void Update()
    {
        Vector3 pos = player.gameObject.transform.position;

        material.SetVector("_PlayerPosition", pos);
        material.SetVector("_PrevPlayerPosition", player.prevPosition);
        
        material2.SetVector("_PlayerPosition", player.prevPosition);
        material2.SetVector("_PrevPlayerPosition", player.prevPrevPosition);
    }
}
