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
    [SerializeField] private GameObject infoPanel;

    [SerializeField] RectTransform menuPanelRect;
    [SerializeField] RectTransform menuCoverRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private GameObject menuTitle;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject pauseButton;
   

    MenuManager menuManager;

    public void Start()
    {
        menuPanel.SetActive(false);
        menuBG.SetActive(false);
    }

    public void Update()
    {
        //Debug.Log("Update called"); // Thêm dòng này để kiểm tra xem Update có đang chạy không
        //playButton == null || settingsButton == null || menuTitle == null ||

        if (pauseButton == null || infoPanel == null)
        {
            if (isMenuOn)
            {
                MenuButton.enabled = false;
                playButton.enabled = false;
                settingsButton.enabled = false;
                menuTitle.SetActive(false);
            }
            else
            {
                MenuButton.enabled = true;
                playButton.enabled = true;
                settingsButton.enabled = true;
                menuTitle.SetActive(true);
            }
        }
        else
        {
            if (isMenuOn)
            {
                MenuButton.enabled = false;
                pauseButton.SetActive(false);
                infoPanel.SetActive(false);
            }
            else
            {
                MenuButton.enabled = true;
                pauseButton.SetActive(true);
                infoPanel.SetActive(true);
            }
        }


        //if (isMenuOn)
        //{
        //    Debug.Log(isMenuOn);
        //    MenuButton.enabled = false;
        //    playButton.enabled = false;
        //    settingsButton.enabled = false;
        //    pauseButton.SetActive(false);
        //    menuTitle.SetActive(false);
        //    infoPanel.SetActive(false);
        //}
        //else
        //{
            
        //    MenuButton.enabled = true;
        //    playButton.enabled = true;
        //    settingsButton.enabled = true;
        //    pauseButton.SetActive(true);
        //    menuTitle.SetActive(true);
        //    infoPanel.SetActive(true);
        //}
    }

    public void OpenMenu()
    {
        if (!isMenuOn) // Kiểm tra xem menu đã mở chưa
        { 
        //Debug.Log(isMenuOn);
        isMenuOn = true;
        MenuIntro();
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        menuPanel.SetActive(!menuPanel.activeSelf);
        //menuTitle.SetActive(!menuTitle.activeSelf);
        //infoPanel.SetActive(!infoPanel.activeSelf);
            menuBG.SetActive(!menuBG.activeSelf);
        Time.timeScale = 0.0f;


        //Them am thanh
        AudioManager.audioInstance.PlaySFX("ButtonPress");
        }
    }

    public async void CloseMenu()
    {
        if (isMenuOn) // Kiểm tra xem menu đã mở chưa
        {
            //Them am thanh
            AudioManager.audioInstance.PlaySFX("ButtonPress");

            //Debug.Log(isMenuOn);
            isMenuOn = false;
            await MenuOutro();
            canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
            menuPanel.SetActive(!menuPanel.activeSelf);
            //menuTitle.SetActive(!menuTitle.activeSelf);
            //infoPanel.SetActive(!infoPanel.activeSelf);
            menuBG.SetActive(!menuBG.activeSelf);
            Time.timeScale = 1.0f;
            

        }
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
    

