using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad saveLoadInstance;

    public List<LevelScore> levelScores;
    public int levels;
    public int oneStar;
    public int twoStar;
    public int threeStar;

    public bool isFristTimePlayGame = true;

    private SaveAndLoad()
    { 
        
    }


    private void Awake()
    {
        if (saveLoadInstance == null)
        {
            saveLoadInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
        
        LoadDataWithPlayerPrefs(out levelScores);

        //Set cac thong so co ban cho game
        levels = 1;
        oneStar = 30;
        twoStar = 60;
        threeStar = 100;

        if (levelScores == null)
        {
            Debug.Log("list null");

            //Khai bao list
            levelScores = new List<LevelScore>();

            //Tao ra 1 level dau tien va add vao list
            LevelScore level1 = new LevelScore("Level 1", 0);
            levelScores.Add(level1);
        }

        //Kiem tra moi khi co update level moi thi se tang cho nguoi choi len level moi neu nguoi choi da choi het tat ca level truoc do
        if (levelScores[levelScores.Count - 1].score >= oneStar)
        {
            if (levels > levelScores.Count)
            {
                LevelScore newLevel = new LevelScore($"Level {levelScores.Count + 1}", 0);
                levelScores.Add(newLevel);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveDataWithPlayerPrefs()
    {
        //Luu list diem
        string jsonListLevelScores = JsonConvert.SerializeObject(levelScores);
        Debug.Log(jsonListLevelScores);
        PlayerPrefs.SetString("ListLevelScores", jsonListLevelScores);
        PlayerPrefs.SetString("IsFristTimePlayGame", isFristTimePlayGame.ToString());

        PlayerPrefs.Save();
    }

    public void LoadDataWithPlayerPrefs(out List<LevelScore> levelScores) 
    {
        string jsonListLevelScores = PlayerPrefs.GetString("ListLevelScores");
        Debug.Log(jsonListLevelScores);
        levelScores = JsonConvert.DeserializeObject<List<LevelScore>>(jsonListLevelScores);

        isFristTimePlayGame = bool.Parse(PlayerPrefs.GetString("IsFristTimePlayGame"));
    }
}
