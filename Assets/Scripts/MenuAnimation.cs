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
    [SerializeField] private bool isMenuOn = true;

    [SerializeField] private GameObject menuPanel;

    [SerializeField] RectTransform menuPanelRect;
    [SerializeField] RectTransform menuCoverRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;

    public void Start()
    {
        menuPanel.SetActive(false);

    }

    public void Update()
    {
        if (isMenuOn)
        {
            MenuButton.enabled = false;
        }
        else
        {
            MenuButton.enabled=true;
        }
    }

    public  void OpenMenu()
    {
        isMenuOn = true;
         MenuIntro();
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        menuPanel.SetActive(!menuPanel.activeSelf);
        Time.timeScale = 0.0f;
    }

    public async void CloseMenu()
    {
        isMenuOn = true;
        await MenuOutro();
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        menuPanel.SetActive(!menuPanel.activeSelf);
        Time.timeScale = 1.0f;
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
    

