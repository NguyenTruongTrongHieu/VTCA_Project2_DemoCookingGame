using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookFood : MonoBehaviour
{
    public int slotInGrill;
    [SerializeField] private GameObject hamburger;

    [SerializeField]
    private float cookingTime = 0;
    private string ripeness;
    private int moveCookFood;

    private Vector2 firstPositionOnCuttingBoard = new Vector2(-1, -1.25f);
    private Vector2 secondPositionOnCuttingBoard = new Vector2(0, -1.25f);
    private Vector2 thirdPositionOnCuttingBoard = new Vector2(1, -1.25f);

    private Vector2 firstPositionOnGrill = new Vector2(5.5f, -1.25f);
    private Vector2 secondPositionOnGrill = new Vector2(6.5f, -1.25f);
    private Vector2 thirdPositionOnGrill = new Vector2(7.5f, -1.25f);

    // Start is called before the first frame update
    void Start()
    {
        moveCookFood = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cookingTime += Time.deltaTime;
        if (cookingTime <= 3)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            ripeness = "notYet";
        }
        if ((cookingTime > 3 && cookingTime <= 6) && (transform.position.x > 5))
        { 
            GetComponent<SpriteRenderer>().color = new Color (1, 1, 0);
            ripeness = "ripe";
        }
        if ((cookingTime > 6) && (transform.position.x > 5))
        {
            ripeness = "burn";
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        //Neu bien move bat dau thay doi thi cho cook food chay
        if (moveCookFood != 0)
        {
            this.transform.position -= new Vector3(0.25f, 0, 0);
        }

        //Neu chay toi diem dich roi thi dung
        //bat dieu kien de xet tung truong hop cook food se di toi
        switch (moveCookFood) 
        {
            case 1:
                {
                    if (this.transform.position.x == firstPositionOnCuttingBoard.x)
                    {
                        Instantiate(hamburger, firstPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
            case 2:
                {
                    if (this.transform.position.x == secondPositionOnCuttingBoard.x)
                    {
                        Instantiate(hamburger, secondPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
            case 3:
                {
                    if (this.transform.position.x == thirdPositionOnCuttingBoard.x)
                    {
                        Instantiate(hamburger, thirdPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
        }
    }

    private void OnMouseDown()
    {
        //Neu chua chin thi chua duoc dem di
        if (ripeness == "notYet")
        {
            return;
        }
        //Kiem tra thit o slot nao de lam trong slot do
        if (slotInGrill == 1)
        {
            Gameplay.grillS1 = "empty";
        }
        if (slotInGrill == 2)
        {
            Gameplay.grillS2 = "empty";
        }
        if (slotInGrill == 3)
        {
            Gameplay.grillS3 = "empty";
        }
        //Neu bi khet thi huy
        if (ripeness == "burn")
        {
            Debug.Log("khet");
            Destroy(gameObject);
            return;
        }

        if ((Gameplay.cuttingboardS1 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 1;
            moveCookFood = 1;
            Gameplay.cuttingboardS1 = "FullBun";
        }
        else
            if ((Gameplay.cuttingboardS2 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 2;
            moveCookFood = 2;
            Gameplay.cuttingboardS2 = "FullBun";
        }
        else
            if ((Gameplay.cuttingboardS3 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 3;
            moveCookFood = 3;
            Gameplay.cuttingboardS3 = "FullBun";
        }
    }
}
