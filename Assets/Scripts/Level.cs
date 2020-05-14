 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject finish;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject deadPlayer;
    [SerializeField]
    private Text lifeCounter;
    [SerializeField]
    private GameObject levelCompletePanel;
    [SerializeField]
    private GameObject levelFailedPanel;
    [SerializeField]
    float restartTimeOffset = 2f;
    
    [HideInInspector]
    public int number;
    private int lives = 25;

    CinemachineVirtualCamera virtualCamera;
    float deadTime = 0f;
    // private float timer = 0f;


    void Start()
    {
        Time.timeScale = 1;
        lifeCounter.text = "Lives: " + lives;
        // timer = Time.time;
        player.finishEvent += Complete;
        player.deadEvent += Restart;
        player.transform.position = start.transform.position;
        virtualCamera = camera.GetComponentInChildren<CinemachineVirtualCamera>();
    }


    void Restart()
    {
        lifeCounter.text = "Lives: " + --lives;
        player.gameObject.SetActive(false);

        if (lives <= 0)
        {
            Failed();
            return;
        }
        Transform dp = Instantiate(deadPlayer).transform;
        dp.position = player.transform.position;
        dp.GetComponent<Rigidbody2D>().velocity = player.rigidbody.velocity;

        player.rigidbody.velocity = Vector2.zero;
        player.transform.position = start.transform.position;

        virtualCamera.Follow = dp;
        
        StartCoroutine(continueGame());
    }

    IEnumerator continueGame()
    {
        yield return new WaitForSeconds(restartTimeOffset);
        player.gameObject.SetActive(true);
        virtualCamera.Follow = player.gameObject.transform;
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
