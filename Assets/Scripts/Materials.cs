using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    [SerializeField]
    Material floorMaterial;
    [SerializeField]
    Material wallMaterial;
    [SerializeField]
    Player player;


    void Update()
    {
        Vector3 pos = player.gameObject.transform.position;

        floorMaterial.SetVector("_PlayerPosition", pos);
        floorMaterial.SetVector("_PrevPlayerPosition", player.prevPosition);
        
        wallMaterial.SetVector("_PlayerPosition", player.prevPosition);
        wallMaterial.SetVector("_PrevPlayerPosition", player.prevPrevPosition);
    }
}
