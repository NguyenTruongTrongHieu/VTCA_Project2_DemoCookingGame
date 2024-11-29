using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Customers : MonoBehaviour
{
    [SerializeField] bool isMale;

    public List<string> orderedFoods = new List<string>();
    private int amountOfOrderFoods;
    public int randomAmountOfOrders;
    public int orderFoodChoose;//Xac dinh xem mon an co duoc keo den cho khach chua va vi tri cua mon an do trong list orderFoods

    public bool havingFood;//Xac dinh xem mon an co duoc keo den cho khach chua
    public bool isOnEndDrag = false;//Xac dinh xem player da tha chuot chua
    public bool isAlreadyDone = false;//Bien de xac dinh xem player da keo tha food vao cus chua
    public int slotInQueue;
    public bool isWaiting;//Bien de xac dinh khach dang di chuyen hay da den ban
    private bool isContinueMoving;//Bien de xac dinh xem cus co duoc tiep tuc di chuyen chua
    public bool isOutOfTime;
    private bool isAnyOrder;//Kiem tra xem con order nao chua hoan thanh khong, false la het order
    private string customerEmotion;

    //Thoi gian cho va order customer
    public GameObject orderPanel;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image imageSlider;
    [SerializeField] private Image[] imageOrderFoods;
    [SerializeField] private Sprite imageTick;
    [SerializeField] private GameObject coin;
    public float customerTime;
    public float safeTime;
    public float warningTime;

    private Animator animator;

    [SerializeField] private float speed;
    //Di chuyen len xuong
    private float sinCenterY;
    [SerializeField] private float amplitude = 2;
    [SerializeField] private float frequency = 2;

    //Vi tri cua customer tren hang cho
    [SerializeField] private float customerPosition;

    //Diem so player nhan duoc khi hoan thanh order cua customer
    public int highScore;
    public int mediumScore;
    public int lowScore;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        sinCenterY = this.transform.position.y;

        isWaiting = false;
        havingFood = false;
        isAlreadyDone = false;
        isContinueMoving = false;
        isOutOfTime = false;
        orderFoodChoose = 0; 
        isAnyOrder = true;
        customerEmotion = "normal";

        //random so luong order cua cus
        amountOfOrderFoods = Random.Range(1, randomAmountOfOrders + 1);

        for (int i = 0; i < amountOfOrderFoods; i++)
        {
            //Random 1 vi tri trong list orderedFood o class Gameplay
            int randomFood = Random.Range(0, GameplayFoods.instance.orderFoods.Length);
            //Gan ten mon an o vi tri do cho cus
            orderedFoods.Add(GameplayFoods.instance.orderFoods[randomFood].name);
            //Gan sprite mon an do cho customer
            imageOrderFoods[i].sprite = GameplayFoods.instance.orderFoods[randomFood].sprite;
        }

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
        //Kiem tra xem order nao da duoc hoan thanh
        if (isOnEndDrag && orderedFoods.Count != 0)
        {
            orderedFoods[orderFoodChoose - 1] = "";//Loi khi dua cho cus mon an dau tien thi mon an thu 2 se bi dua len tro thanh mon 1, lam cho imageFood bi khong thay doi duoc
            imageOrderFoods[orderFoodChoose - 1].sprite = imageTick;
            isOnEndDrag = false;

            //Kiem tra con order nao khong
            foreach (var order in orderedFoods)
            {
                if (order != "")
                {
                    return;
                }
            }
            //Khi duyet qua het cac order thi luc nay da hoan thanh xong order
            isAnyOrder = false;
        }

        //Khi nguoi choi hoan thanh order cho cus hoac het thoi gian
        if (!isAnyOrder || isOutOfTime)
        {
            if (isContinueMoving)
            {
                this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                //Di chuyen nhap nho
                float sin = Mathf.Sin(this.transform.position.x * frequency) * amplitude;
                this.transform.position = new Vector3(this.transform.position.x, sinCenterY + sin, this.transform.position.z);
                Destroy(this.gameObject, 2.8f);
            }

            if (isAlreadyDone)
            {
                return;
            }

            isAlreadyDone = true;

            //Tinh diem cho player
            if (isOutOfTime)//Neu het gio doi cua cus thi khong tinh diem
            {
                Debug.Log("Het gio, tru tim khong cong diem");

                //Them am thanh
                if (isMale)
                {
                    AudioManager.audioInstance.PlaySFX("MaleTimeOut");
                }
                else
                {
                    AudioManager.audioInstance.PlaySFX("FemaleTimeOut");
                }
            }
            else
            {
                //Kiem tra trang thai cua cus
                if (customerEmotion == "normal")
                {
                    Gameplay.score += highScore;
                    
                }
                else if (customerEmotion == "impatient")
                {
                    Gameplay.score += mediumScore;
                }
                else if (customerEmotion == "angry")
                {
                    Gameplay.score += lowScore;

                    //Them am thanh
                    if (isMale)
                    {
                        AudioManager.audioInstance.PlaySFX("MaleAngry");
                    }
                    else
                    {
                        AudioManager.audioInstance.PlaySFX("FemaleAngry");
                    }
                }

                //Update text score
                Gameplay gameplay = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gameplay>();
                gameplay.UpdateTextScore();

                //Hien coin
                Instantiate(coin, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }

            //imageOrderFood1.sprite = imageTick;
            StartCoroutine(SetAnimForCus());
        }

        //Cus dang doi
        if (isWaiting)
        {
            customerTime -= Time.deltaTime;
            if (customerTime > safeTime)
            {
                imageSlider.color = Color.green;

                if (!animator.GetBool("isNormal"))//Kiem tra xem da set int chua, neu chua set thi lam, da set roi thi khong can
                {
                    animator.SetInteger("statesCustomer", 1);
                    StartCoroutine(SetDefaultStatusAnimator());
                    animator.SetBool("isNormal", true);

                    //Them am thanh
                    if (isMale)
                    {
                        AudioManager.audioInstance.PlaySFX("MaleNormal");
                    }
                    else
                    {
                        AudioManager.audioInstance.PlaySFX("FemaleNormal");
                    }
                }
            }
            else if (customerTime > warningTime)
            {
                imageSlider.color = Color.yellow;

                if (!animator.GetBool("isImpatient"))
                {
                    animator.SetInteger("statesCustomer", 2); 
                    StartCoroutine(SetDefaultStatusAnimator());
                    animator.SetBool("isImpatient", true);

                    //Them am thanh
                    if (isMale)
                    {
                        AudioManager.audioInstance.PlaySFX("MaleImpatient");
                    }
                    else
                    {
                        AudioManager.audioInstance.PlaySFX("FemaleImpatient");
                    }
                }

                customerEmotion = "impatient";
            }
            else if (customerTime > 0)
            {
                imageSlider.color = Color.red;

                if (!animator.GetBool("isAngry"))
                {
                    animator.SetInteger("statesCustomer", 3);
                    StartCoroutine(SetDefaultStatusAnimator());
                    animator.SetBool("isAngry", true);

                    //Them am thanh
                    if (isMale)
                    {
                        AudioManager.audioInstance.PlaySFX("MaleAngry");
                    }
                    else
                    {
                        AudioManager.audioInstance.PlaySFX("FemaleAngry");
                    }
                }

                customerEmotion = "angry";
            }
            //Khi customerTime = 0 thi het thoi gian 
            else if (customerTime <= 0)
            {
                isOutOfTime = true;
            }
            timerSlider.value = customerTime;
        }
    }

    IEnumerator SetDefaultStatusAnimator()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("statesCustomer", 0);
    }

    IEnumerator SetAnimForCus()
    {
        Debug.Log("Bat anim");
        if (customerEmotion == "normal")
        {
            animator.SetInteger("statesCustomer", 1);
            StartCoroutine(SetDefaultStatusAnimator());

            //Them am thanh
            if (isMale)
            {
                AudioManager.audioInstance.PlaySFX("MaleNormal");
            }
            else
            {
                AudioManager.audioInstance.PlaySFX("FemaleNormal");
            }
        }
        else if (customerEmotion == "impatient")
        {
            animator.SetInteger("statesCustomer", 2);
            StartCoroutine(SetDefaultStatusAnimator());

            //Them am thanh
            if (isMale)
            {
                AudioManager.audioInstance.PlaySFX("MaleImpatient");
            }
            else
            {
                AudioManager.audioInstance.PlaySFX("FemaleImpatient");
            }
        }
        else if (customerEmotion == "angry")
        {
            animator.SetInteger("statesCustomer", 3);
            StartCoroutine(SetDefaultStatusAnimator());
        }

        yield return new WaitForSeconds(1);
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
        //Di chuyen nhap nho
        float sin = Mathf.Sin(this.transform.position.x * frequency) * amplitude;
        this.transform.position = new Vector3(this.transform.position.x, sinCenterY + sin, this.transform.position.z);
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
