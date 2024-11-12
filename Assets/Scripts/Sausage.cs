using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class Sausage : MonoBehaviour
{
    public bool isOntheRoll;
    public int slotInCuttingBoard;

    // Start is called before the first frame update
    void Start()
    {
        //Dua doi tuong hien tai tro thanh con cua canvas dung de keo tha
        var dragCanvas = GameObject.FindGameObjectWithTag("UIDrag");
        this.transform.SetParent(dragCanvas.transform);
        //Chinh scale cua object lai
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("roll"))
        {
            Debug.Log("Cham roll");
            isOntheRoll = true;

            //Xet vi tri cua roll hien tai
            var roll = collision.gameObject.GetComponent<Materials>();
            slotInCuttingBoard = roll.slotInCuttingboard;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("roll"))
        {
            Debug.Log("Roi khoi roll");
            isOntheRoll = false;
        }
    }
}
