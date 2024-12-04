using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    public bool isOnTheHamburger;
    public int slotInCuttingBoard;//Vi tri cua hamburger ma vegetable cham vao

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
        if (collision.gameObject.CompareTag("hamburger"))
        {
            isOnTheHamburger = true;

            //Xet vi tri cua roll hien tai
            var hamburger = collision.gameObject.GetComponent<Foods>();
            slotInCuttingBoard = hamburger.slotInCuttingboard;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hamburger"))
        {
            isOnTheHamburger = false;
        }
    }
}
