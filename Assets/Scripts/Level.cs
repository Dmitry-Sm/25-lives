using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject finish;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject deadPlayer;
    
    [HideInInspector]
    public int number;

    private int lifes = 25;
    private float timer = 0f;


    void Start()
    {
        timer = Time.time;
        player.finishEvent += Complete;
        player.deadEvent += Restart;
        player.transform.position = start.transform.position;
    }


    void Restart()
    {
        if (--lifes <= 0)
        {
            Failure();
        }
        Transform dp = Instantiate(deadPlayer).transform;
        dp.position = player.transform.position;
        dp.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
        player.transform.position = start.transform.position;
            
    }

    void Complete()
    {
        Debug.Log("Level complete!");
    }

    void Failure()
    {

    }
}
