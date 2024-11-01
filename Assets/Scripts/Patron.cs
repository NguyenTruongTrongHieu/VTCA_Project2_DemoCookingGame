using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    public string orderedFood;
    public bool havingFood;
    public bool isOnEndDrag = false;
    [SerializeField]private GameObject customer;

    // Start is called before the first frame update
    void Start()
    {
        //Random 1 vi tri trong list orderedFood o class Gameplay
        int randomFood = Random.Range(0, Gameplay.orderedFood.Count);
        //Gan ten mon an o vi tri do cho Patron
        orderedFood = Gameplay.orderedFood[randomFood];
        havingFood = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnEndDrag == true)
        {
            Debug.Log("Destroy cus");
            Destroy(customer);
        }
    }
}
