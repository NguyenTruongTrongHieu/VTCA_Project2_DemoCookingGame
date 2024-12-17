using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelPanel;
    //[SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gameTitle;
    
    void Start()
    {
        menuPanel.SetActive(true);
        levelPanel.SetActive(false);
        gameTitle.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelPanel()
    {
        levelPanel.SetActive(!levelPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
        gameTitle.SetActive(!gameTitle.activeSelf);
    }

    public void BackToMenu()
    {
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        //tutorialPanel.SetActive(false);
        menuPanel.SetActive(!menuPanel.activeSelf);
        gameTitle.SetActive(!gameTitle.activeSelf);
    }
}
