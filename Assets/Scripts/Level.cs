 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Text lifeCounter;
    [SerializeField]
    private GameObject levelCompletePanel;
    [SerializeField]
    private GameObject levelFailedPanel;
    
    [HideInInspector]
    public int number;

    private int lives = 5;
    // private float timer = 0f;


    void Start()
    {
        Time.timeScale = 1;
        lifeCounter.text = "Lives: " + lives;
        // timer = Time.time;
        player.finishEvent += Complete;
        player.deadEvent += Restart;
        player.transform.position = start.transform.position;
    }


    void Restart()
    {
        lifeCounter.text = "Lives: " + --lives;
        if (lives <= 0)
        {
            Failed();
        }
        Transform dp = Instantiate(deadPlayer).transform;
        dp.position = player.transform.position;
        dp.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity;
        player.transform.position = start.transform.position;
            
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    void Complete()
    {
        levelCompletePanel.SetActive(true);
        Pause();
    }

    void Failed()
    {
        Pause();
        levelFailedPanel.SetActive(true);
    }
}
