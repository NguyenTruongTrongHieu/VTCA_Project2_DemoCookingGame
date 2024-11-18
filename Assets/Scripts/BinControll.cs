using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BinControll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject materialBunObj;
    [SerializeField]
    private GameObject burgersObj;
    [SerializeField]
    private GameObject materialRollObj;
    [SerializeField]
    private GameObject sausageObj;

    [SerializeField]
    private GameObject hotdogObj;

    [SerializeField] private GameObject objDrag;
    [SerializeField] private RectTransform objDragRect;

    [SerializeField] private GameObject positionOnCuttingBoard;
    [SerializeField] private GameObject positionOnGrill;

    private bool isMaterials;
    private bool isSausage;
    private bool isCookFoods;
    private bool isFullSlot;
    private bool isDragging;
    private bool isAbleToDrag;//Co the keo duoc khong

    // Start is called before the first frame update
    void Start()
    {
        isAbleToDrag = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Gameplay.isChooseBin)//Phan nay va isChooseBin dung de check xem ng choi co dang keo 2 bin cung luc khong
        {
            isAbleToDrag = false;
            return;
        }

        //Chuyen bien isChooseBin thanh true
        Gameplay.isChooseBin = true;
        //Chinh bien kiem tra xem co full slot o thot hoac vi nuong chua ve false
        //Neu slot can xet da full thi se xet lai o cac ham if ben trong
        isFullSlot = false;
        //Neu nguoi choi chi click chuot thi isDragging = false
        isDragging = false;
        //De kiem tra xem bin duoc cham vao la gi, truoc tien can tat cac bien nay
        isSausage = false;
        isMaterials = false;
        isCookFoods = false;

        if (gameObject.tag == "bun bin")
        {
            //Xac dinh xem nguyen lieu duoc chon la cook food hay materials
            isMaterials = true;
            //Set slot cho nguyen lieu vua duoc chon
            if (Gameplay.cuttingboardS1 != "empty" && Gameplay.cuttingboardS2 != "empty")
            {
                isFullSlot = true;
                return;
            }
            else if (Gameplay.cuttingboardS1 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
            }

            //Sinh ra obj
            objDrag = Instantiate(materialBunObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }

        else if (gameObject.tag == "meat bin")
        {
            isCookFoods = true;

            if (Gameplay.grillS1 != "empty" && Gameplay.grillS2 != "empty")
            {
                isFullSlot = true;
                return;
            }
            else if (Gameplay.grillS1 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 1;
            }
            else if (Gameplay.grillS2 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 2;
            }

            //Sinh ra obj
            objDrag = Instantiate(burgersObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }

        else if (gameObject.tag == "roll bin")
        {
            isMaterials = true;

            if (Gameplay.cuttingboardS1 != "empty" && Gameplay.cuttingboardS2 != "empty")
            {
                isFullSlot = true;
                return;
            }
            else if (Gameplay.cuttingboardS1 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
            }

            //Sinh ra obj
            objDrag = Instantiate(materialRollObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }

        else if (gameObject.tag == "sausage bin")
        {
            isSausage = true;

            objDrag = Instantiate(sausageObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isAbleToDrag)
        {
            isAbleToDrag = true;
        }

        Gameplay.isChooseBin = false;
        if (!isDragging)
        {
            Destroy(objDrag.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On begin drag");
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Neu khong the keo thi o End Drag can chinh ve co the keo lai
        if (!isAbleToDrag)
        { 
            isAbleToDrag = true;
            return;
        }

        //Neu het cho roi thi khong can lam
        if (isFullSlot)
        {
            return;
        }

        //Chinh lai isChooseBin de tiep tuc chon bin khac
        Gameplay.isChooseBin = false;

        if (isSausage)
        {
            var sausage = objDrag.gameObject.GetComponent<Sausage>();

            if (sausage.isOntheRoll)
            {
                if (sausage.slotInCuttingBoard == 1)
                {
                    hotdogObj.gameObject.GetComponent<Foods>().slotInCuttingboard = 1;
                    Instantiate(hotdogObj, positionOnCuttingBoard.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
                    Gameplay.cuttingboardS1 = "FullBun";
                }
                else if (sausage.slotInCuttingBoard == 2)
                {
                    hotdogObj.gameObject.GetComponent<Foods>().slotInCuttingboard = 2;
                    Instantiate(hotdogObj, positionOnCuttingBoard.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                    Gameplay.cuttingboardS2 = "FullBun";
                }
            }

            Destroy(sausage.gameObject);
            return;
        }
        else if (isMaterials)
        {
            var material = objDrag.gameObject.GetComponent<Materials>();
            if (!material.isOnTheCuttingBoard)
            {
                Destroy(material.gameObject);
            }
            else
            {
                if (material.slotInCuttingboard == 1)
                {
                    objDrag.transform.position = positionOnCuttingBoard.transform.position + new Vector3(-1, 0, 0);
                    Gameplay.cuttingboardS1 = "JustBun";
                }
                else if (material.slotInCuttingboard == 2)
                {
                    objDrag.transform.position = positionOnCuttingBoard.transform.position + new Vector3(1, 0, 0);
                    Gameplay.cuttingboardS2 = "JustBun";
                }
            }
        }
        else if (isCookFoods)
        { 
            var cookFood = objDrag.gameObject.GetComponent<CookFood>();

            if (!cookFood.isOnTheGrill)
            { 
                Destroy (cookFood.gameObject);
            }
            else
            {
                //Bat dau nuong
                cookFood.isChoose = false;
                cookFood.cookFoodPanel.SetActive(true);//Bat panel cua cook food len de nguoi choi xem thoi gian 

                if (cookFood.slotInGrill == 1)
                {
                    objDrag.transform.position = positionOnGrill.transform.position + new Vector3(-1, 0, 0);
                    Gameplay. grillS1 = "full";
                }
                else if (cookFood.slotInGrill == 2)
                {
                    objDrag.transform.position = positionOnGrill.transform.position + new Vector3(1, 0, 0);
                    Gameplay.grillS2 = "full";
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isAbleToDrag)
        {
            Debug.Log("khong the keo");
            return;
        }

        if (isFullSlot)
        {
            Debug.Log("Het cho");
            return;
        }
        objDragRect.anchoredPosition += eventData.delta;
    }
}
