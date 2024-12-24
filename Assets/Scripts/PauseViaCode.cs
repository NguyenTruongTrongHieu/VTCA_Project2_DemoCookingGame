using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseViaCode : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Break();
        }
    }
}
