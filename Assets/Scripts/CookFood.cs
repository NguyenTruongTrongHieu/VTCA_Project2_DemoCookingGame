using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookFood : MonoBehaviour, IPointerDownHandler
{
    public int slotInGrill;
    public bool isOnTheGrill;
    private string name;
    [SerializeField] private GameObject food;

    [SerializeField]
    private float cookingTime = 0;
    private string ripeness;
    private int moveCookFood;
    [SerializeField] private float speed = 5f;
    public bool isChoose;//Bien de xac dinh khi player chon mon an
    [SerializeField] private Image imageMeat;
    [SerializeField] private List<Sprite> spriteMeat = new List<Sprite>();

    [SerializeField] private GameObject positionOnCuttingBoard;
    [SerializeField] private GameObject positionOnGrill;

    private Vector2 firstPositionOnCuttingBoard = new Vector2(-1, -2.69f);
    private Vector2 secondPositionOnCuttingBoard = new Vector2(0, -2.69f);
    private Vector2 thirdPositionOnCuttingBoard = new Vector2(1, -2.69f);

    private Vector2 firstPositionOnGrill = new Vector2(-6f, -2.69f);
    private Vector2 secondPositionOnGrill = new Vector2(-5f, -2.69f);
    private Vector2 thirdPositionOnGrill = new Vector2(-4f, -2.69f);

    // Start is called before the first frame update
    void Start()
    {
        name = this.gameObject.tag;
        moveCookFood = 0;
        isChoose = true;
        isOnTheGrill = false;

        //Dua doi tuong hien tai tro thanh con cua canvas dung de keo tha
        var dragCanvas = GameObject.FindGameObjectWithTag("UIDrag");
        this.transform.SetParent(dragCanvas.transform);
        //Chinh scale cua object lai
        this.transform.localScale = new Vector3(1, 1, 1);

        //Tim 2 position
        positionOnCuttingBoard = GameObject.FindGameObjectWithTag("MaterialPosition");
        positionOnGrill = GameObject.FindGameObjectWithTag("MeatPosition");
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoose)
        {
            return;
        }

        cookingTime -= Time.deltaTime;
        if (cookingTime >= 3)
        {
            imageMeat.sprite = spriteMeat[0];
            ripeness = "raw";
        }
        else if ((cookingTime < 3 && cookingTime >= 0))
        {
            imageMeat.sprite = spriteMeat[1];
            ripeness = "ripe";
            Debug.Log("ripe");
        }
        else if ((cookingTime < 0))
        {
            imageMeat.sprite = spriteMeat[2];
            ripeness = "burn";
            Debug.Log("burn");
        }
    }

    private void FixedUpdate()
    {
        //Neu bien move bat dau thay doi thi cho cook food chay
        if (moveCookFood != 0)
        {
            this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }

        //Neu chay toi diem dich roi thi dung
        //bat dieu kien de xet tung truong hop cook food se di toi
        switch (moveCookFood) 
        {
            case 1:
                {
                    if (this.transform.position.x >= firstPositionOnCuttingBoard.x)
                    {
                        //Gan vi tri tren thot cho mon an
                        food.gameObject.GetComponent<Foods>().slotInCuttingboard = 1;
                        Instantiate(food, firstPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
            case 2:
                {
                    if (this.transform.position.x >= secondPositionOnCuttingBoard.x)
                    {
                        food.gameObject.GetComponent<Foods>().slotInCuttingboard = 2;
                        Instantiate(food, secondPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
            case 3:
                {
                    if (this.transform.position.x >= thirdPositionOnCuttingBoard.x)
                    {
                        food.gameObject.GetComponent<Foods>().slotInCuttingboard = 3;
                        Instantiate(food, thirdPositionOnCuttingBoard, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("grill"))
        {
            isOnTheGrill = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("grill"))
        {
            isOnTheGrill = false;
        }
    }

    //private void OnMouseDown()
    //{
    //    //Neu chua chin thi chua duoc dem di
    //    if (ripeness == "notYet")
    //    {
    //        return;
    //    }
    //    //Neu bi khet thi huy
    //    if (ripeness == "burn")
    //    {
    //        SetSlotInGrill();
    //        Debug.Log("khet");
    //        Destroy(gameObject);
    //        return;
    //    }

    //    //Neu tren thot/ban/dia chua co mieng banh hay nguyen lieu nao thi return ve
    //    if (name == "meat" && Gameplay.cuttingboardS1 != "JustBun" && Gameplay.cuttingboardS2 != "JustBun" && Gameplay.cuttingboardS3  != "JustBun")
    //    {
    //        return;
    //    }
    //    if (name == "sausage" && Gameplay.cuttingboardS1 != "JustRoll" && Gameplay.cuttingboardS2 != "JustRoll" && Gameplay.cuttingboardS3 != "JustRoll")
    //    {
    //        return;
    //    }

    //    isChoose = true;
    //    SetSlotInGrill();

    //    //Setup cho slot cua thot va gan bien moveCookFood de cookfood co the di chuyen
    //    if ((Gameplay.cuttingboardS1 == "JustBun") && name == "meat")
    //    {
    //        moveCookFood = 1;
    //        Gameplay.cuttingboardS1 = "FullBun";
    //    }
    //    else if ((Gameplay.cuttingboardS2 == "JustBun") && name == "meat")
    //    {
    //        moveCookFood = 2;
    //        Gameplay.cuttingboardS2 = "FullBun";
    //    }
    //    else if ((Gameplay.cuttingboardS3 == "JustBun") && name == "meat")
    //    {
    //        moveCookFood = 3;
    //        Gameplay.cuttingboardS3 = "FullBun";
    //    }
    //    else if ((Gameplay.cuttingboardS1 == "JustRoll") && name == "sausage")
    //    {
    //        moveCookFood = 1;
    //        Gameplay.cuttingboardS1 = "FullRoll";
    //    }
    //    else if ((Gameplay.cuttingboardS2 == "JustRoll") && name == "sausage")
    //    {
    //        moveCookFood = 2;
    //        Gameplay.cuttingboardS2 = "FullRoll";
    //    }
    //    else if ((Gameplay.cuttingboardS3 == "JustRoll") && name == "sausage")
    //    {
    //        moveCookFood = 3;
    //        Gameplay.cuttingboardS3 = "FullRoll";
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
        //Neu chua chin thi chua duoc dem di
        if (ripeness == "notYet")
        {
            return;
        }
        //Neu bi khet thi huy
        if (ripeness == "burn")
        {
            SetSlotInGrill();
            Debug.Log("khet");
            Destroy(gameObject);
            return;
        }

        //Neu tren thot/ban/dia chua co mieng banh hay nguyen lieu nao thi return ve
        if (name == "meat" && Gameplay.cuttingboardS1 != "JustBun" && Gameplay.cuttingboardS2 != "JustBun" && Gameplay.cuttingboardS3 != "JustBun")
        {
            return;
        }
        if (name == "sausage" && Gameplay.cuttingboardS1 != "JustRoll" && Gameplay.cuttingboardS2 != "JustRoll" && Gameplay.cuttingboardS3 != "JustRoll")
        {
            return;
        }

        isChoose = true;
        SetSlotInGrill();

        //Setup cho slot cua thot va gan bien moveCookFood de cookfood co the di chuyen
        if ((Gameplay.cuttingboardS1 == "JustBun") && name == "meat")
        {
            moveCookFood = 1;
            Gameplay.cuttingboardS1 = "FullBun";
        }
        else if ((Gameplay.cuttingboardS2 == "JustBun") && name == "meat")
        {
            moveCookFood = 2;
            Gameplay.cuttingboardS2 = "FullBun";
        }
        else if ((Gameplay.cuttingboardS3 == "JustBun") && name == "meat")
        {
            moveCookFood = 3;
            Gameplay.cuttingboardS3 = "FullBun";
        }
        else if ((Gameplay.cuttingboardS1 == "JustRoll") && name == "sausage")
        {
            moveCookFood = 1;
            Gameplay.cuttingboardS1 = "FullRoll";
        }
        else if ((Gameplay.cuttingboardS2 == "JustRoll") && name == "sausage")
        {
            moveCookFood = 2;
            Gameplay.cuttingboardS2 = "FullRoll";
        }
        else if ((Gameplay.cuttingboardS3 == "JustRoll") && name == "sausage")
        {
            moveCookFood = 3;
            Gameplay.cuttingboardS3 = "FullRoll";
        }
    }

    void SetSlotInGrill() 
    {
        //Kiem tra thit o slot nao de lam trong slot do
        if (slotInGrill == 1)
        {
            Gameplay.grillS1 = "empty";
        }
        else if (slotInGrill == 2)
        {
            Gameplay.grillS2 = "empty";
        }
        else if (slotInGrill == 3)
        {
            Gameplay.grillS3 = "empty";
        }
    }
}
