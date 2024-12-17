using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Gameplay : MonoBehaviour
{
    //Level hien tai
    public string level;
    public TextMeshProUGUI titleLevelText;

    //Diem so trong man choi
    public static int score;
    public TextMeshProUGUI textScore;
    public GameObject star;
    public static bool isDestroyStar;

    //So mang trong man choi
    public static int heart;
    public Image[] heartImage;

    //slot tren thot
    public static string cuttingboardS1 = "empty";
    public static string cuttingboardS2 = "empty";

    //slot tren vi nuong
    public static string grillS1 = "empty";
    public static string grillS2 = "empty";

    //slot hang doi cua customer
    public static string queueS1 = "empty";
    public static string queueS2 = "empty";
    public static string queueS3 = "empty";

    //check player cham vao bin
    public static bool isChooseBin;

    //Check xem player thang hay thua
    public static bool isWinning;
    public static bool isGameOver;
    private Gameplay()
    { 
        
    }

    private void Awake()
    {
        level = SceneManager.GetActiveScene().name;
        titleLevelText.text = level;

        score = 0;
        UpdateTextScore();
        isDestroyStar = false;

        heart = 3;
        foreach (var heart in heartImage)
        { 
            heart.gameObject.SetActive(true);
        }

        isChooseBin = false;

        cuttingboardS1 = "empty";
        cuttingboardS2 = "empty";

        grillS1 = "empty";
        grillS2 = "empty";

        queueS1 = "empty";
        queueS2 = "empty";
        queueS3 = "empty";

        isWinning = false;
        isGameOver = false;

        SaveAndLoad.saveLoadInstance.isFristTimePlayGame = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //An title level di
        StartCoroutine(SetSCaleForTitleLevel());

        AudioManager.audioInstance.PlayMusic("InGame");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyStar)
        {
            StartCoroutine(SetSCaleForStar());
            isDestroyStar = false;
        }
    }

    public void UpdateTextScore()
    {
        if (score < 30) {
            textScore.text = $"{score}/30";
        }
        else if (score < 60)
        {
            textScore.text = $"{score}/60";
        }
        else 
        {
            textScore.text = $"{score}/100";
        }
    }

    IEnumerator SetSCaleForStar()
    {
        while (star.transform.localScale.x < 1.2f)
        {
            star.transform.localScale += new Vector3(6 * Time.deltaTime, 6 * Time.deltaTime, 6 * Time.deltaTime);
            yield return null;
        }
        while (star.transform.localScale.x > 0.6f)
        {
            star.transform.localScale -= new Vector3(6 * Time.deltaTime, 6 * Time.deltaTime, 6 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator SetSCaleForTitleLevel()
    {
        titleLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        while (titleLevelText.transform.localScale.x > 0f)
        {
            titleLevelText.transform.localScale -= new Vector3(6 * Time.deltaTime, 6 * Time.deltaTime, 6 * Time.deltaTime);
            yield return null;
        }

        Destroy(titleLevelText.gameObject);
    }
}
