using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    public GameObject loadingScreenObject;
    public Slider progressBar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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

    public void SwitchToSceneByName(string levelName)
    {
        this.gameObject.SetActive(true);
        loadingScreenObject.SetActive(true);

        progressBar.value = 0;
        StartCoroutine(SwitchToSceneAsycByName(levelName));
    }

    public void SwitchToSceneByBuildIndex(int id)
    {
        this.gameObject.SetActive(true);
        loadingScreenObject.SetActive(true);

        progressBar.value = 0;
        StartCoroutine(SwitchToSceneAsycByBuildIndex(id));
    }

    IEnumerator SwitchToSceneAsycByName(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone)
        {
            progressBar.value = asyncLoad.progress;
            yield return null;
        }

        //yield return new WaitForSeconds(0.2f);
        loadingScreenObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator SwitchToSceneAsycByBuildIndex(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        while (!asyncLoad.isDone)
        {
            progressBar.value = asyncLoad.progress;
            yield return null;
        }

        //yield return new WaitForSeconds(0.2f);
        loadingScreenObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
