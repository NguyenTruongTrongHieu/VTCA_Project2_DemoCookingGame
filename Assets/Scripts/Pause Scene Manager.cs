using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseSceneManager : MonoBehaviour
{
    [Header( "Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private GameObject pausePanel;


    public void Start()
    {
        pausePanel.SetActive(false);
        
    }

    public void PauseTheGame()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 0.0f;
    }

     public void ContinueTheGame()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 1.0f;
    }

     public void BackToHome()
    {

        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;

        
    }
}

