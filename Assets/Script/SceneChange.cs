using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void LevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }
    
    public void HomeScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("HomeScene");
    }

    public void PlayLevel(int level)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Level" + level);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
