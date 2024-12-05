using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timetext;  
    [SerializeField] float remainingtime = 90;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;
    [SerializeField] private Image[] stars;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        
        if (remainingtime > 0)
        {
            remainingtime -= Time.deltaTime;
        }
        else//Het thoi gian 1 man choi
        {
            if (gameOverPanel.gameObject.activeSelf)
            {
                return;
            }
            remainingtime = 0;
            //Bat panel thang thua len
            TurnOnGameOverPanel();
            SetStarForGameOverPanel();
        }

        
        int minutes = Mathf.FloorToInt(remainingtime / 60);
        int seconds = Mathf.FloorToInt(remainingtime % 60);

        
        timetext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TurnOnGameOverPanel()
    {
        //Test save and load
        var levelScore = SaveAndLoad.saveLoadInstance.levelScores.Find(x => x.level == SceneManager.GetActiveScene().name);//Tim level hien tai o trong list
        if (levelScore == null)
        {
            Debug.Log("khong tim thay level hien tai trong list");
            return;
        }

        //Kiem tra xem diem cua level hien tai ma nguoi choi vua hoan thanh co qua duoc moc de win khong
        if (Gameplay.score >= SaveAndLoad.saveLoadInstance.oneStar)
        {
            //Hien panel win
            Debug.Log("Hien panel win");
            Gameplay.isWinning = true;

            //Win
            if (levelScore.score < SaveAndLoad.saveLoadInstance.oneStar)//Neu nguoi choi chua vuot qua man nay truoc do thi se can tao ra 1 man moi luu vao list
            {
                //Kiem tra so luong man choi trong game voi so luong man choi ma nguoi choi da mo khoa duoc
                //Neu nguoi choi da mo khoa het cac man choi thi khong can them man nua
                if (SaveAndLoad.saveLoadInstance.levels <= SaveAndLoad.saveLoadInstance.levelScores.Count)
                {
                    Debug.Log("Da het man choi");
                }
                else
                {
                    string newLevel = "Level " + (SaveAndLoad.saveLoadInstance.levelScores.Count + 1);//Lay ten cua level moi
                    LevelScore newLevelScore = new LevelScore(newLevel, 0);
                    SaveAndLoad.saveLoadInstance.levelScores.Add(newLevelScore);//Luu man choi moi duoc mo khoa vao list
                }
            }
            else//Neu nguoi choi da vuot qua man nay truoc do thi se khong can tao ra 1 man moi
            {
                Debug.Log("Da vuot qua man choi truoc do");
            }
        }
        else
        {
            //Lose
            //Hien panel lose
            Debug.Log("Hien panel lose");
            Gameplay.isWinning = false;
        }

        //Cho panel game over chay
        gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;

        //Neu diem cua level hien tai ma nguoi choi vua hoan thanh lon hon diem duoc luu thi luu lai
        if (Gameplay.score > levelScore.score)
        {
            levelScore.score = Gameplay.score;
        }
        //Luu list lai
        SaveAndLoad.saveLoadInstance.SaveDataWithPlayerPrefs();
    }

    void SetStarForGameOverPanel()
    {
        if (Gameplay.score >= SaveAndLoad.saveLoadInstance.threeStar)
        {
            stars[0].sprite = fullStar;

            stars[1].sprite = fullStar;

            stars[2].sprite = fullStar;
        }
        else if (Gameplay.score >= SaveAndLoad.saveLoadInstance.twoStar)
        {
            stars[0].sprite = fullStar;

            stars[1].sprite = fullStar;

            stars[2].sprite = emptyStar;
        }
        else if (Gameplay.score >= SaveAndLoad.saveLoadInstance.oneStar)
        {
            stars[0].sprite = fullStar;

            stars[1].sprite = emptyStar;

            stars[2].sprite = emptyStar;
        }
        else
        {
            stars[0].sprite = emptyStar;

            stars[1].sprite = emptyStar;

            stars[2].sprite = emptyStar;
        }
    }
}
