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

        //Test save and load
        var levelScore = SaveAndLoad.saveLoadInstance.levelScores.Find(x => x.level == SceneManager.GetActiveScene().name);//Tim level hien tai o trong list
        if (levelScore == null)
        {
            Debug.Log("khong tim thay level hien tai trong list");
            return;
        }

        //Kiem tra xem diem cua level hien tai ma nguoi choi vua hoan thanh co qua duoc moc de win khong
        if (Gameplay.score >= 30)
        {
            //Hien panel win
            Debug.Log("Hien panel win");

            //Win
            if (levelScore.score < 30)//Neu nguoi choi chua vuot qua man nay truoc do thi se can tao ra 1 man moi luu vao list
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
        }

        //Neu diem cua level hien tai ma nguoi choi vua hoan thanh lon hon diem duoc luu thi luu lai
        if (Gameplay.score > levelScore.score)
        {
            levelScore.score = Gameplay.score;
        }
        //Luu list lai
        SaveAndLoad.saveLoadInstance.SaveDataWithPlayerPrefs();
    }
}

