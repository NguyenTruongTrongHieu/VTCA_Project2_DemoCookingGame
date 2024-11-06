using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnCustomer : MonoBehaviour
{
    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    private GameObject customer;

    //Bien de tinh thoi gian random sinh customer
    [SerializeField] private int beginRandomTime;
    [SerializeField] private int endRandomTime;
    private float randomTime;

    // Start is called before the first frame update
    void Start()
    {
        RandomTimer();
        RandomCustomer();
    }

    // Update is called once per frame
    void Update()
    {
        //Neu hang doi cua customer da full thi khong sinh them
        if (Gameplay.queueS1 == "full" && Gameplay.queueS2 == "full" && Gameplay.queueS3 == "full")
        {
            return;
        }
        //Dem nguoc thoi gian sinh ra customer
        if (randomTime > 0)
        {
            randomTime -= Time.deltaTime;
            return;
        }

        //Den luc sinh ra customer
        var cus = customer.GetComponent<Customers>();
        //Kiem tra vi tri trong hang doi cua customer
        if (Gameplay.queueS1 == "empty")
        {
            cus.slotInQueue = 1;
            Gameplay.queueS1 = "full";
        }
        else if (Gameplay.queueS2 == "empty")
        {
            cus.slotInQueue = 2;
            Gameplay.queueS2 = "full";
        }
        else if (Gameplay.queueS3 == "empty")
        {
            cus.slotInQueue = 3;
            Gameplay.queueS3 = "full";
        }
        Instantiate(customer, this.transform.position, Quaternion.identity);
        RandomTimer();
        RandomCustomer();

        //Khi spawn ra duoc thi se xet den vi tri cua customer
    }

    void RandomTimer()
    {
        //random thoi gian de sinh ra customer
        randomTime = Random.RandomRange(beginRandomTime, endRandomTime);
    }

    void RandomCustomer()
    {
        //Random customer
        int random = Random.RandomRange(0, customers.Count);
        customer = customers[random];
    }
}
