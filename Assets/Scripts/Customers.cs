using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customers : MonoBehaviour
{
    public string orderedFood;
    public bool havingFood;//Xac dinh xem mon an co duoc keo den cho khach chua
    public bool isOnEndDrag = false;//Xac dinh xem player da tha chuot chua
    public bool isAlreadyHaveFood = false;//Bien de xac dinh xem player da keo tha food vao cus chua
    public int slotInQueue;
    public bool isWaiting;//Bien de xac dinh khach dang di chuyen hay da den ban
    private bool isContinueMoving;//Bien de xac dinh xem cus co duoc tiep tuc di chuyen chua
    public bool isOutOfTime;

    //Thoi gian cho va order customer
    public GameObject orderPanel;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image imageSlider;
    [SerializeField] private Image imageOrderFood;
    [SerializeField] private Sprite imageTick;
    public float customerTime;

    [SerializeField] private float speed;


    //Vi tri cua customer tren hang cho
    [SerializeField] private float customerPosition;
    // Start is called before the first frame update
    void Start()
    {
        isWaiting = false;
        isAlreadyHaveFood = false;
        isContinueMoving = false;
        isOutOfTime = false;

        //Random 1 vi tri trong list orderedFood o class Gameplay
        int randomFood = Random.Range(0, GameplayFoods.instance.orderFoods.Length);
        //Gan ten mon an o vi tri do cho Patron
        orderedFood = GameplayFoods.instance.orderFoods[randomFood].name;
        //Gan sprite mon an do cho customer
        imageOrderFood.sprite = GameplayFoods.instance.orderFoods[randomFood].sprite;

        havingFood = false;
        isOnEndDrag = false;

        //Set up vi tri cho customer
        if (slotInQueue == 1)
        {
            customerPosition = -4.31f;
        }
        if (slotInQueue == 2)
        {
            customerPosition = 0.8f;
        }
        if (slotInQueue == 3)
        {
            customerPosition = 6.32f;
        }

        //Setup slider
        timerSlider.maxValue = customerTime;
        timerSlider.minValue = 0;
        //Tat panel order
        orderPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Khi nguoi choi dua mon an cho cus
        if (isOnEndDrag == true || isOutOfTime)
        {
            if (isContinueMoving)
            {
                this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                Destroy(this.gameObject, 2);
            }

            if (isAlreadyHaveFood)
            {
                return;
            }

            isAlreadyHaveFood = true;
            imageOrderFood.sprite = imageTick;
            StartCoroutine(SetAnimForCus());
            Debug.Log("Destroy cus");
        }

        //Cus dang doi
        if (isWaiting)
        {
            customerTime -= Time.deltaTime;
            if (customerTime > 10)
            {
                imageSlider.color = Color.green;
            }
            else if (customerTime > 5)
            {
                imageSlider.color = Color.yellow;
            }
            else if (customerTime > 0)
            {
                imageSlider.color = Color.red;
            }
            //Khi customerTime = 0 thi het thoi gian 
            else if (customerTime <= 0)
            {
                isOutOfTime = true;
            }
            timerSlider.value = customerTime;
        }
    }

    IEnumerator SetAnimForCus()
    {
        Debug.Log("Bat anim");
        yield return new WaitForSeconds(2);
        orderPanel.SetActive(false);
        Debug.Log("Tat anim");
        isContinueMoving = true;//Sau khi thuc hien anim vui ve hoac hien emote gi do thi cus moi duoc tiep tuc di chuyen
    }

    private void FixedUpdate()
    {
        //Neu dang doi thi khong can di chuyen nua
        if (isWaiting)
        {
            return;   
        }

        //Neu customer di chuyen den vi tri tren hang cho thi dung
        if (slotInQueue == 1)
        {
            if (this.transform.position.x <= customerPosition)
            {
                isWaiting = true;
                orderPanel.SetActive(true);
                return;
            }
        }
        else if (slotInQueue == 2)
        {
            if (this.transform.position.x <= customerPosition)
            {
                isWaiting = true;
                orderPanel.SetActive(true);
                return;
            }
        }
        else if (slotInQueue == 3)
        {
            if (this.transform.position.x <= customerPosition)
            {
                isWaiting = true;
                orderPanel.SetActive(true);
                return;
            }
        }
        this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }

    private void OnDestroy()
    {
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
    }
}
