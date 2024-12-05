using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
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

        LoadDataWithPlayerPrefs();

        if (levelScores == null)
        {
            Debug.Log("list null");

            //Khai bao list
            levelScores = new List<LevelScore>();

            //Tao ra 1 level dau tien va add vao list
            LevelScore level1 = new LevelScore("Level 1", 0);
            levelScores.Add(level1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Dem so luong man choi co trong game
        levels = 1;
        oneStar = 30;
        twoStar = 60;
        threeStar = 100;
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

        PlayerPrefs.Save();
    }

    public void LoadDataWithPlayerPrefs() 
    {
        string jsonListLevelScores = PlayerPrefs.GetString("ListLevelScores");
        Debug.Log(jsonListLevelScores);
        levelScores = JsonConvert.DeserializeObject<List<LevelScore>>(jsonListLevelScores);
    }
}
