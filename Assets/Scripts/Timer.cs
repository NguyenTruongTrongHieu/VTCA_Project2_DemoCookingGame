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
    [SerializeField] float endUpTime = 10;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Sprite fullStar;
    [SerializeField] private Sprite emptyStar;
    [SerializeField] private Image[] stars;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (Gameplay.isGameOver)
        {
            return;
        }

        
        if (remainingtime > 0)
        {
            remainingtime -= Time.deltaTime;

            //Neu gan het thoi gian thi chuyen sang do va chay anim
            if (remainingtime <= endUpTime && !animator.GetBool("endTimeComing"))
            { 
                timetext.color = Color.red;
                AudioManager.audioInstance.PlaySFX("ClockTicking");
                animator.SetBool("endTimeComing", true);
            }
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
            StartCoroutine(SetStarForGameOverPanel());
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

            //Them am thanh
            AudioManager.audioInstance.PlayMusic("Win");
            AudioManager.audioInstance.musicSource.loop = false;

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

            //Them am thanh
            AudioManager.audioInstance.PlayMusic("Lose");
            AudioManager.audioInstance.musicSource.loop = false;
        }

        //Cho panel game over chay
        gameOverPanel.gameObject.SetActive(true);
        Gameplay.isGameOver = true;

        //Neu diem cua level hien tai ma nguoi choi vua hoan thanh lon hon diem duoc luu thi luu lai
        if (Gameplay.score > levelScore.score)
        {
            levelScore.score = Gameplay.score;
        }
        //Luu list lai
        SaveAndLoad.saveLoadInstance.SaveDataWithPlayerPrefs();
    }

    IEnumerator SetStarForGameOverPanel()
    {
        if (Gameplay.score >= SaveAndLoad.saveLoadInstance.threeStar)
        {
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(0));

            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(1));

            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(2));
        }
        else if (Gameplay.score >= SaveAndLoad.saveLoadInstance.twoStar)
        {
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(0));

            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(1));

            stars[2].sprite = emptyStar;
        }
        else if (Gameplay.score >= SaveAndLoad.saveLoadInstance.oneStar)
        {
            yield return new WaitForSeconds(0.8f);
            StartCoroutine(SetAnimForStar(0));

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

    IEnumerator SetAnimForStar(int index)
    {
        stars[index].sprite = fullStar;
        stars[index].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        while (stars[index].transform.localScale.y > 1)
        {
            stars[index].transform.localScale -= new Vector3(3.5f * Time.deltaTime, 3.5f * Time.deltaTime, 3.5f * Time.deltaTime);
            yield return null;
        }

        stars[index].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }
}
