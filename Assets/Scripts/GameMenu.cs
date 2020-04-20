using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    [SerializeField] GameObject GameMenuPanel;

    public void ResumeButton()
    {
        GameMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitLevelButton()
    {
        SceneManager.LoadScene("Main menu");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update() {
        if (Input.GetButtonDown("Cancel"))
        {
            GameMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
