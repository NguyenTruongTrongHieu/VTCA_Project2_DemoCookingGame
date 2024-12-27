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
    [SerializeField] private float animSpeed = 1.0f;
    
    void Start()
    {
        menuPanel.SetActive(true);
        levelPanel.SetActive(false);
        gameTitle.SetActive(true);

        AudioManager.audioInstance.PlayMusic("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelPanel()
    {
        levelPanel.SetActive(!levelPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
        gameTitle.SetActive(false);

        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(!levelPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
        gameTitle.SetActive(false);

        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");
    }

    public void BackToMenu()
    {
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        //tutorialPanel.SetActive(false);
        menuPanel.SetActive(!menuPanel.activeSelf);
        //gameTitle.SetActive(!gameTitle.activeSelf);
        //StartCoroutine(TitleAnim());

        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");
    }

    public void CloseGameTitle()
    {
        StartCoroutine(TitleAnim());
    }

    public IEnumerator TitleAnim()
    {
        gameTitle.transform.localScale = Vector3.zero;
        gameTitle.gameObject.SetActive(true);

        while (gameTitle.transform.localScale.y < 0.98f)
        {
            gameTitle.transform.localScale += new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 10 * Time.deltaTime);
            yield return null;
        }
    }
}
