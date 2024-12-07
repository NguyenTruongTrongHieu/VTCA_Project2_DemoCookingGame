using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;

    private void Awake()
    {

    }

    private void Start()
    {
        int unlockedLevel = SaveAndLoad.saveLoadInstance.levelScores.Count;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
            SetStarForLevel(i);
        }
    }

    public void openLevel(int levelId)
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    void SetStarForLevel(int index)
    {
        //Duyet vao 3 star trong button
        Image star1 = buttons[index].transform.GetChild(1).GetChild(0).GetComponent<Image>();
        Image star2 = buttons[index].transform.GetChild(1).GetChild(1).GetComponent<Image>();
        Image star3 = buttons[index].transform.GetChild(1).GetChild(2).GetComponent<Image>();

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
