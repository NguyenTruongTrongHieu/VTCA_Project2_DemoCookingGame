using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

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
        }
    }

    public void openLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    void SetStarForLevel(int index)
    {
        /*
         * Duyet vao 3 star trong button
        Image star1 = buttons[index].GetChild(1).GetComponent<Image>();
        Image star2 = buttons[index].GetChild(2).GetComponent<Image>();
        Image star3 = buttons[index].GetChild(3).GetComponent<Image>();

        if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.aim3)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = fullStar;
        }
        else if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.aim2)
        {
            star1.sprite = fullStar;
            star2.sprite = fullStar;
            star3.sprite = emptyStar;
        }
        else if (SaveAndLoad.saveLoadInstance.levelScores[index].score >= SaveAndLoad.saveLoadInstance.aim1)
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

        */
    }
}
