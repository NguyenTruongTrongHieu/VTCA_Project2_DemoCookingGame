using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelPanel;
    public Button[] buttons;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

    //Chinh value scroll
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {

    }

    private void Start()
    {
        int unlockedLevel = SaveAndLoad.saveLoadInstance.levelScores.Count;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(ClickSound);
            buttons[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].transform.GetChild(0).gameObject.SetActive(false);

            string result1 = buttons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;


            if (!int.TryParse(result1, out int result))
            {
                Debug.Log("Error");
                return;
            }
       
            buttons[i].onClick.AddListener(() => openLevel(result));
            SetStarForLevel(i);
        }

        //Gan value cho scroll de di chuyen cac button den button da duoc mo khoa hien tai
        float valueScrollRect = (unlockedLevel - 1) / (buttons.Length/1.0f);
        Debug.Log(valueScrollRect);
        scrollRect.horizontalScrollbar.value = valueScrollRect;
    }

    public void openLevel(int levelId)
    {
        Debug.Log(levelId);
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        string levelName = "Level " + levelId;
        levelPanel.SetActive(false);
        //SceneManager.LoadScene(levelName);
        LoadingManager.instance.SwitchToSceneByName(levelName);
    }

    public void ClickSound()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");
    }

    void SetStarForLevel(int index)
    {
        //Duyet vao 3 star trong button
        Image star1 = buttons[index].transform.GetChild(2).GetChild(0).GetComponent<Image>();
        Image star2 = buttons[index].transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Image star3 = buttons[index].transform.GetChild(2).GetChild(2).GetComponent<Image>();

        star1.gameObject.SetActive(true);
        star2.gameObject.SetActive(true);
        star3.gameObject.SetActive(true);

        if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.threeStar)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = fullStar;
        }
        else if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.twoStar)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = emptyStar;
        }
        else if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.oneStar)
        {
            star1.sprite = fullStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar; 
        }
        else
        {
            star1.sprite = emptyStar;
            star2.sprite = emptyStar;
            star3.sprite = emptyStar;
        }
    }
}
