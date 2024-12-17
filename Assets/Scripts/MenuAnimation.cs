using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;

public class MenuAnimation : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button MenuButton;
     private bool isMenuOn = false;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject menuBG;

    [SerializeField] RectTransform menuPanelRect;
    [SerializeField] RectTransform menuCoverRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private GameObject menuTitle;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;

    public void Start()
    {
        menuPanel.SetActive(false);
        menuBG.SetActive(false);
    }

    public void Update()
    {
        if (isMenuOn)
        {
            MenuButton.enabled = false;
            playButton.enabled = false;
            settingsButton.enabled = false;
        }
        else
        {
            MenuButton.enabled=true;
            playButton.enabled=true;
            settingsButton.enabled=true;
        }
    }

    public  void OpenMenu()
    {
        isMenuOn = true;
         MenuIntro();
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        menuPanel.SetActive(!menuPanel.activeSelf);
        menuBG.SetActive(!menuBG.activeSelf);
        Time.timeScale = 0.0f;
        menuTitle.SetActive(false);
    }

    public async void CloseMenu()
    {
        isMenuOn = false;
        await MenuOutro();
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        menuPanel.SetActive(!menuPanel.activeSelf);
        menuBG.SetActive(!menuBG.activeSelf);
        Time.timeScale = 1.0f;
        menuTitle.SetActive(true);
    }

    void  MenuIntro()
    {
         menuCoverRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }
   async Task MenuOutro()
    {
      await  menuCoverRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}
    

