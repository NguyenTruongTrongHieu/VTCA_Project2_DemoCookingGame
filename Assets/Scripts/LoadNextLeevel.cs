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

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex +  1);
        Time.timeScale = 1.0f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
        Time.timeScale = 1.0f;
    }

    public void RestartScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void TurnOnPanel()
    {
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        Time.timeScale = 0.0f;
    }

    IEnumerator DestroyText()
    {
        var newText = Instantiate(maxLevelText, maxLevelText.transform.position, Quaternion.identity);
        newText.transform.SetParent(maxLevelText.transform.parent);
        newText.transform.localScale = Vector3.zero;
        newText.gameObject.SetActive(true);

        while (newText.transform.localScale.y < 1)
        {
            newText.transform.localScale += new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 10 * Time.deltaTime);
            yield return null;
        }

        Destroy(newText.gameObject, 1.5f);
        yield return null;
    }
}
