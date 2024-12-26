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
    [SerializeField] private GameObject settingsPanel;


    public void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        
    }

    public void PauseTheGame()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 0.0f;
    }

     public void ContinueTheGame()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = 1.0f;
    }

     public void BackToHome()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        TurnOffCanvasForLoadingScreen();
        LoadingManager.instance.SwitchToSceneByName("Menu");
        Time.timeScale = 1.0f;
    }

    public void SettingsMenu()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        settingsPanel.SetActive(!settingsPanel.activeSelf); 
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void BackToPauseMenu()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        settingsPanel.SetActive(!settingsPanel.activeSelf);
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void TurnOffCanvasForLoadingScreen()
    {
        //Tat cac phan bi thua ra khi hien loading scene
        GameObject.Find("BGCanvas").SetActive(false);
        GameObject.Find("TableCanvas").SetActive(false);
        GameObject.Find("Info Canvas").SetActive(false);
        GameObject.Find("Pause Canvas").SetActive(false);
    }
}

