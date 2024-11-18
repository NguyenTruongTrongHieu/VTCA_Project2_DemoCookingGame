using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timerslider;
    public Text timerstext;
    public float gametime;

    private bool stoptime;


    private void Start()
    {
        stoptime = false;
        timerslider.maxValue = gametime;
        timerslider.value = gametime; 

    }
    private void Update()
    {
        float time = gametime - Time.time;
        int minutes = Mathf.FloorToInt(time / 20);
        int seconds = Mathf.FloorToInt(time - minutes * 20f);
        string texttime = string.Format("{0:0}:{0:20}", minutes, seconds);
        if (time <= 0)
        {
            stoptime = true;
        }
        if (stoptime == false) 

        {
                timerstext.text = texttime;
                timerslider.value = time;
        }
    }
}