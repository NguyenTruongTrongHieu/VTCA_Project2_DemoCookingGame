using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    public string orderedFood;
    public bool havingFood;
    public bool isOnEndDrag = false;
    public int slotInQueue;

    [SerializeField] private float speed;

    //Vi tri cua customer tren hang cho
    [SerializeField] private float customerPosition;
    // Start is called before the first frame update
    void Start()
    {
        //Random 1 vi tri trong list orderedFood o class Gameplay
        int randomFood = Random.Range(0, Gameplay.orderedFood.Count);
        //Gan ten mon an o vi tri do cho Patron
        orderedFood = Gameplay.orderedFood[randomFood];
        havingFood = false;
        isOnEndDrag = false;

        //Set up vi tri cho customer
        if (slotInQueue == 1)
        {
            customerPosition = -6.32f;
        }
        if (slotInQueue == 2)
        {
            customerPosition = 0f;
        }
        if (slotInQueue == 3)
        {
            customerPosition = 6.32f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnEndDrag == true)
        {
            Debug.Log("Destroy cus");
            //Chinh lai slot o queue cua cus hien tai thanh empty
            if (slotInQueue == 1)
            {
                Gameplay.queueS1 = "empty";
            }
            else if (slotInQueue == 2)
            {
                Gameplay.queueS2 = "empty";
            }
            else if (slotInQueue == 3)
            {
                Gameplay.queueS3 = "empty";
            }
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        //Neu customer di chuyen den vi tri tren hang cho thi dung
        if (slotInQueue == 1)
        {
            if (this.transform.position.x <= customerPosition)
            {
                return;
            }
        }
        else if (slotInQueue == 2)
        {
            if (this.transform.position.x <= customerPosition)
            {
                return;
            }
        }
        else if (slotInQueue == 3)
        {
            if (this.transform.position.x <= customerPosition)
            {
                return;
            }
        }
        this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
