using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timetext;  
    [SerializeField] float remainingtime = 90; 

    void Update()
    {
        
        if (remainingtime > 0)
        {
            remainingtime -= Time.deltaTime;
        }
        else
        {
            remainingtime = 0;
            timetext.color = Color.red;
        }

        
        int minutes = Mathf.FloorToInt(remainingtime / 60);
        int seconds = Mathf.FloorToInt(remainingtime % 60);

        
        timetext.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
