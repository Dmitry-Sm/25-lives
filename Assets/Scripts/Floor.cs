using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    Material material2;
    [SerializeField]
    Player player;
    Vector3 prevPos = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.gameObject.transform.position;
        prevPos += (pos - prevPos) * 0.02f;


        material.SetVector("_PlayerPosition", pos);
        material2.SetVector("_PlayerPosition", pos);
        material.SetVector("_PrevPlayerPosition", prevPos);
        material2.SetVector("_PrevPlayerPosition", prevPos);
    }
}
