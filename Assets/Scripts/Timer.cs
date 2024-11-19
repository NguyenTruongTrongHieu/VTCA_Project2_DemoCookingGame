using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Slider timerslider;
    public Text timerstext;
    public float customertime;

    private bool stoptime;


    private void Start()
    {
        stoptime = false;
        timerslider.maxValue = customertime;
        timerslider.value = customertime;

    }
    private void Update()
    {
        if(stoptime) return;


        float time = customertime - Time.timeSinceLevelLoad;
        int minutes = Mathf.FloorToInt(time / 20);
        int seconds = Mathf.FloorToInt(time % 20);
        
            if (time <= 0)
            {
                stoptime = true;
            }
           if (stoptime == false)

            {
                
                timerslider.value = time;
            }

        string texttime = string.Format("{0:0}:{0:20}", minutes, seconds);
        timerstext.text = texttime;
        timerslider.value = time;
    }
}