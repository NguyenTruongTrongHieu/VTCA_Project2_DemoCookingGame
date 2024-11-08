using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Materials : MonoBehaviour
{
    public int slotInCuttingboard;
    public bool isOnTheCuttingBoard;


    // Start is called before the first frame update
    void Start()
    {
        //Dua doi tuong hien tai tro thanh con cua canvas dung de keo tha
        var dragCanvas = GameObject.FindGameObjectWithTag("UIDrag");
        this.transform.SetParent(dragCanvas.transform);
        //Chinh scale cua object lai
        this.transform.localScale = new Vector3(1, 1, 1);

        isOnTheCuttingBoard = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Co va cham");
        if (collision.gameObject.CompareTag("cuttingBoard"))
        {
            Debug.Log("On cutting board");
            isOnTheCuttingBoard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("cuttingBoard"))
        {
            isOnTheCuttingBoard = false;
        }
    }
}
