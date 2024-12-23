using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class Book : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages;
    int index = -1;
    bool rotate = false;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject forwardButton;

    private void Start()
    {
        InitialState();
    }

    public void InitialState()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.rotation = Quaternion.identity;
        }
        pages[0].SetAsLastSibling();
        backButton.SetActive(false);
    }

    public void OnNextButtonClick() // Called when the Next button is clicked
    {
        Debug.Log("Next Button Clicked");
        RotateForward();
    }

    public void OnBackButtonClick() // Called when the Back button is clicked
    {
        RotateBack();
    }

    public void RotateForward()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("PageTurn");

        if (rotate || index >= pages.Count - 1) { return; }

        index++;
        float angle = 180; // Rotate forward
        ForwardButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, true));
    }

    public void ForwardButtonActions()
    {
        if (!backButton.activeInHierarchy)
        {
            backButton.SetActive(true);
        }
        if (index == pages.Count - 1)
        {
            forwardButton.SetActive(false);
        }
    }

    public void RotateBack()
    {
        //Them am thanh
        AudioManager.audioInstance.PlaySFX("PageTurn");

        if (rotate == true) { return; }
        float angle = 0; //in order to rotate the page back, you need to set the rotation to 0 degrees around the y axis
        pages[index].SetAsLastSibling();
        BackButtonActions();
        StartCoroutine(Rotate(angle, false));
    }

    public void BackButtonActions()
    {
        if (!forwardButton.activeInHierarchy)
        {
            forwardButton.SetActive(true);
        }
        if (index - 1 < 0)
        {
            backButton.SetActive(false);
        }
    }

    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        rotate = true;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        while (true)
        {
            value += Time.unscaledDeltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);

            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (!forward)
                {
                    index--;
                }
                rotate = false;
                break;
            }
            yield return null;
        }
    }
}