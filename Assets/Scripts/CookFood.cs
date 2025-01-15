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
    [Header("CookFood position")]
    //Bien de xac dinh xem mieng thit co nam tren thot khong va nam o dau
    public int slotInGrill;
    public bool isOnTheGrill;

    private bool isReturnStartPosition;//Bien de xac dinh co can quay ve vi tri bat dau kkhong
    private int slotBunInCuttingBoard;//Bien de xac dinh vi tri bun duoc cham vao tren ban

    [Header("CookFood name")]
    //Bien de xac dinh cac cookFood khac nhau
    private string nameCookFood;
    [SerializeField] private GameObject food;

    [Header("timer")]
    [SerializeField]
    private float cookingTime = 0;
    private string ripeness;
    public bool isChoose;//Bien de xac dinh khi player chon mon an

    [Header("Image and animation meat")]
    [SerializeField] private Image imageMeat;
    [SerializeField] private List<Sprite> spriteMeat = new List<Sprite>();

    public GameObject cookFoodPanel;
    [SerializeField] private Slider sliderCookingTime;
    [SerializeField] private Image sliderImage;
    [SerializeField] private Color sliderColor;
    [SerializeField] private GameObject fireBurnCookFood;//Gameobject chua anim chay cua cook food
    [SerializeField] private GameObject getToGarbage;//Gameobject chua anim goi y thung rac
    [SerializeField] private Image emojiImage;
    [SerializeField] private Sprite[] emojis;
    private float emojiChangeScaleTime = 0.25f;
    private bool isSetScaleForRipeness = false;

    [SerializeField] private GameObject positionOnCuttingBoard;
    [SerializeField] private GameObject positionOnGrill;

    //Keo tha
    [Header("Drag and drop")]
    [SerializeField] private RectTransform rectTransform; 
    private Vector2 screenPosition;
    private Vector3 worldPosition;

    private Vector3 startPosition;

    [Header("Minus score")]
    [SerializeField] private int minusScore = 3;


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
        fireBurnCookFood.SetActive(false);
        getToGarbage.SetActive(false);

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
        //Neu player da hoan thanh game roi thi ko can lam gi het
        if (Gameplay.isGameOver)
        {
            return;
        }

        if (isChoose || cookingTime < 0)
        {
            return;
        }

        cookingTime -= Time.deltaTime;
        sliderCookingTime.value = cookingTime;

        if (cookingTime >= 4 && ripeness != "raw")
        {
            imageMeat.sprite = spriteMeat[0];
            sliderImage.color = Color.yellow;
            emojiImage.sprite = null;
            ripeness = "raw";

            //Them am thanh
            AudioManager.audioInstance.PlaySFX("CookFoodGrilled");
        }
        else if ((cookingTime < 4 && cookingTime > 2) && ripeness != "ripe")
        {
            imageMeat.sprite = spriteMeat[1];
            ripeness = "ripe";
            sliderImage.color = Color.green;
            emojiImage.sprite = emojis[0];

            if (!isSetScaleForRipeness)//Dieu kien de chi set scale cho emoji 1 lan duy nhat o moi trang thai
            {
                StartCoroutine(SetScaleForEmojiImage());
                isSetScaleForRipeness = true;
            }

            //Them am thanh
            AudioManager.audioInstance.PlaySFX("CookFoodRipe");
        }
        else if (cookingTime <= 2 && cookingTime > 0)
        {
            sliderImage.color = Color.red; 
            emojiImage.sprite = emojis[1];

            if (isSetScaleForRipeness)
            {
                StartCoroutine(SetScaleForEmojiImage());
                isSetScaleForRipeness = false;

                //Them am thanh
                AudioManager.audioInstance.PlaySFX("CookFoodWarning");
            }
        }
        else if ((cookingTime <= 0) && ripeness != "burn")
        {
            imageMeat.sprite = spriteMeat[2];
            ripeness = "burn";
            sliderImage.color = Color.white;
            emojiImage.sprite = emojis[2];
            fireBurnCookFood.SetActive(true);
            getToGarbage.SetActive(true);

            if (!isSetScaleForRipeness)//Dieu kien de chi set scale cho emoji 1 lan duy nhat o moi trang thai
            {
                StartCoroutine(SetScaleForEmojiImage());
                isSetScaleForRipeness = true;
            }
            //Them am thanh
            AudioManager.audioInstance.PlaySFX("CookFoodBurn");
        }
    }

    IEnumerator SetScaleForEmojiImage()//Ham de phong to emoji len nham cho nguoi choi de nhan biet
    {
        emojiImage.rectTransform.localScale = new Vector3( 5, 5, 1);
        var scale = emojiImage.rectTransform.localScale;
        while (scale.x > 1.5f)
        {
            scale.x -= (scale.x - 1) / emojiChangeScaleTime * Time.deltaTime;
            scale.y -= (scale.y - 1) / emojiChangeScaleTime * Time.deltaTime;
            emojiImage.rectTransform.localScale = scale;

            yield return null;
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
            isReturnStartPosition = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bun"))
        {
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
            slotBunInCuttingBoard = 0;
            isReturnStartPosition = true;
        }

        if (collision.gameObject.CompareTag("TrashBin"))
        {
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

        //Zoom thung rac to len
        var trashBin = GameObject.FindGameObjectWithTag("TrashBin").GetComponent<RectTransform>();
        trashBin.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        trashBin.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Zoom thung rac nho xuong
        var trashBin = GameObject.FindGameObjectWithTag("TrashBin").GetComponent<RectTransform>();
        trashBin.transform.localScale = new Vector3(1, 1, 1);
        trashBin.transform.position -= new Vector3(0, 0.2f, 0);

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
                //Them am thanh
                AudioManager.audioInstance.PlaySFX("FoodAppear");

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
            else //Neu khong cham vao bun thi la thung rac
            {
                //Tru diem khi vut vao thung rac
                Gameplay gameplay = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gameplay>();
                Gameplay.score = Mathf.Max(Gameplay.score - minusScore, 0);
                gameplay.UpdateTextScore();

                //Them am thanh
                AudioManager.audioInstance.PlaySFX("TrashBin");
            }
            
            //Xoa cook food va reset lai slot
            SetSlotInGrill();
            Destroy(this.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Lam object di theo con chuot hoac ngon tay
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        rectTransform.position = new Vector2(worldPosition.x, worldPosition.y);
    }
}
