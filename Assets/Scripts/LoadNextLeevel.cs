using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLeevel : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToNextLevel()
    {
       SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +  1);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
        Time.timeScale = 1.0f;
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void TurnOnPanel()
    {
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        Time.timeScale = 0.0f;
    }

    public void BackToGame()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
