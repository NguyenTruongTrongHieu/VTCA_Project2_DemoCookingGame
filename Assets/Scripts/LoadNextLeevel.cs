using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextLeevel : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI maxLevelText;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gameplay.isWinning)
        {
            title.text = "You Win";
            restartButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            title.text = "You Lose";
            restartButton.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
        }
    }

    public void ToNextLevel()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        //Kiem tra xem level hien tai da la level cuoi cung hay chua
        var scene = SceneManager.GetActiveScene().name;
        var currentLevel = scene[scene.Length - 1];
        int level = int.Parse(currentLevel.ToString());

        if (level >= SaveAndLoad.saveLoadInstance.levels)
        {
            Debug.Log("Da het man choi");
            StartCoroutine(DestroyText());
            return;
        }

        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +  1);
        TurnOffCanvasForLoadingScreen();
        LoadingManager.instance.SwitchToSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        //SceneManager.LoadSceneAsync("Menu");

        TurnOffCanvasForLoadingScreen();
        LoadingManager.instance.SwitchToSceneByName("Menu");
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void RestartScene()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");

        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        TurnOffCanvasForLoadingScreen();
        LoadingManager.instance.SwitchToSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    IEnumerator DestroyText()
    {
        var newText = Instantiate(maxLevelText, maxLevelText.transform.position, Quaternion.identity);
        newText.transform.SetParent(maxLevelText.transform.parent);
        newText.transform.localScale = Vector3.zero;
        newText.gameObject.SetActive(true);

        while (newText.transform.localScale.y < 0.98f)
        {
            newText.transform.localScale += new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 10 * Time.deltaTime);
            yield return null;
        }

        Destroy(newText.gameObject, 1f);
        yield return null;
    }

    public void TurnOffCanvasForLoadingScreen()
    {
        //Tat cac phan bi thua ra khi hien loading scene
        GameObject.Find("BGCanvas").SetActive(false);
        GameObject.Find("TableCanvas").SetActive(false);
        GameObject.Find("Info Canvas").SetActive(false);
        GameObject.Find("Pause Canvas").SetActive(false);
    }
}
