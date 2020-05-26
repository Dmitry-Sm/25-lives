using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToPlayer : MonoBehaviour
{
    [SerializeField]
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 d = transform.position - player.prevPosition;
        transform.rotation = Quaternion.AngleAxis(180 + Mathf.Rad2Deg * Mathf.Atan2(d.x, d.y), Vector3.back);        
    }
}
