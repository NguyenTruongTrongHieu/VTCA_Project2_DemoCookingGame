using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Bien de xac dinh xem mieng thit co nam tren thot khong va nam o dau
    public int slotInGrill;
    public bool isOnTheGrill;

    private bool isReturnStartPosition;//Bien de xac dinh co can quay ve vi tri bat dau kkhong
    private int slotBunInCuttingBoard;//Bien de xac dinh vi tri bun duoc cham vao tren ban

    //Bien de xac dinh cac cookFood khac nhau
    private string nameCookFood;
    [SerializeField] private GameObject food;

    [SerializeField]
    private float cookingTime = 0;
    private string ripeness;
    public bool isChoose;//Bien de xac dinh khi player chon mon an

    [SerializeField] private Image imageMeat;
    [SerializeField] private List<Sprite> spriteMeat = new List<Sprite>();

    public GameObject cookFoodPanel;
    [SerializeField] private Slider sliderCookingTime;
    [SerializeField] private Image sliderImage;
    [SerializeField] private Color sliderColor;

    [SerializeField] private GameObject positionOnCuttingBoard;
    [SerializeField] private GameObject positionOnGrill;
    [SerializeField] private RectTransform rectTransform;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        nameCookFood = this.gameObject.tag;
        isChoose = true;
        isOnTheGrill = false;
        slotBunInCuttingBoard = 0;//Xet Bien de xac dinh vi tri bun duoc cham vao tren ban = 0, tuc la chua cham vao cai bun nao het
        isReturnStartPosition = true;//Moi dau thi luon quay ve vi tri cu

        //Set slider va panel cho cookFood
        sliderCookingTime.maxValue = cookingTime;
        sliderCookingTime.minValue = 0;
        cookFoodPanel.SetActive(false);

        //Dua doi tuong hien tai tro thanh con cua canvas dung de keo tha
        var dragCanvas = GameObject.FindGameObjectWithTag("UIDrag");
        this.transform.SetParent(dragCanvas.transform);
        //Chinh scale cua object lai
        this.transform.localScale = new Vector3(1, 1, 1);

        //Tim 2 position
        positionOnCuttingBoard = GameObject.FindGameObjectWithTag("MaterialPosition");
        positionOnGrill = GameObject.FindGameObjectWithTag("MeatPosition");
        //Gan component
        rectTransform = this.GetComponent<RectTransform>();
        //Gan  start position
        if (slotInGrill == 1)
        {
            startPosition = positionOnGrill.transform.position + new Vector3(-1, 0, 0);
        }
        else if (slotInGrill == 2)
        {
            startPosition = positionOnGrill.transform.position + new Vector3(1, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoose || cookingTime < 0)
        {
            return;
        }

        cookingTime -= Time.deltaTime;
        sliderCookingTime.value = cookingTime;
        
        if (cookingTime >= 3)
        {
            imageMeat.sprite = spriteMeat[0];
            sliderImage.color = Color.yellow;
            ripeness = "raw";
        }
        else if ((cookingTime < 3 && cookingTime > 0))
        {
            imageMeat.sprite = spriteMeat[1];
            ripeness = "ripe";
            sliderImage.color = Color.green;
            Debug.Log("ripe");
        }
        else if ((cookingTime <= 0))
        {
            imageMeat.sprite = spriteMeat[2];
            ripeness = "burn";
            sliderImage.color = Color.white;
            Debug.Log("burn");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("grill"))
        {
            isOnTheGrill = true;
        }

        if (collision.gameObject.CompareTag("TrashBin"))
        {
            Debug.Log("Cham thung rac");
            isReturnStartPosition = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bun"))
        {
            Debug.Log("Cham bun");
            slotBunInCuttingBoard = collision.gameObject.GetComponent<Materials>().slotInCuttingboard;
            isReturnStartPosition = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("grill"))
        {
            isOnTheGrill = false;
        }

        if (collision.gameObject.CompareTag("bun"))
        {
            Debug.Log("Roi khoi bun");
            slotBunInCuttingBoard = 0;
            isReturnStartPosition = true;
        }

        if (collision.gameObject.CompareTag("TrashBin"))
        {
            Debug.Log("Roi thung rac");
            isReturnStartPosition = true;
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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isChoose = true;
        cookFoodPanel.SetActive(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isReturnStartPosition)
        {
            isChoose = false;
            cookFoodPanel.SetActive(true);
            //Tra ve vi tri ban dau
            this.transform.position = startPosition;
        }
        else
        {
            if (slotBunInCuttingBoard != 0)//Kiem tra xem co cham vao bun khong
            {
                Debug.Log("Co cham vao bun");
                //Kiem tra xem mieng thit co dang chin khong, neu chin thi moi duoc lam
                if (ripeness == "ripe")
                {
                    //Kiem tra xem co cham vao bun khong, neu co thi sinh ra hamburger
                    if (slotBunInCuttingBoard == 1)
                    {
                        food.GetComponent<Foods>().slotInCuttingboard = 1;
                        Instantiate(food, positionOnCuttingBoard.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
                    }
                    else if (slotBunInCuttingBoard == 2)
                    {
                        food.GetComponent<Foods>().slotInCuttingboard = 2;
                        Instantiate(food, positionOnCuttingBoard.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                    }
                }
                else//Neu o trang thai khac ngoai chin thi se quay ve vi tri cu
                {
                    isChoose = false;
                    cookFoodPanel.SetActive(true);
                    //Tra ve vi tri ban dau
                    this.transform.position = startPosition;
                    return;
                }
            }
            
            //Xoa cook food va reset lai slot
            SetSlotInGrill();
            Destroy(this.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Lam object di theo con chuot hoac ngon tay
        rectTransform.anchoredPosition += eventData.delta;
    }
}
