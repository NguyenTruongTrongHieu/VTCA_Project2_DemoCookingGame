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

    private Vector2 firstPositionOnCuttingBoard = new Vector2(-1, -1.25f);
    private Vector2 secondPositionOnCuttingBoard = new Vector2(0, -1.25f);
    private Vector2 thirdPositionOnCuttingBoard = new Vector2(1, -1.25f);

    private Vector2 firstPositionOnGrill = new Vector2(-6f, -1.25f);
    private Vector2 secondPositionOnGrill = new Vector2(-5f, -1.25f);
    private Vector2 thirdPositionOnGrill = new Vector2(-4f, -1.25f);

    private bool isMaterials;
    private bool isFullSlot;
    private bool isDragging;
    private bool isSausage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Chinh bien kiem tra xem co full slot o thot hoac vi nuong chua ve false
        //Neu slot can xet da full thi se xet lai o cac ham if ben trong
        isFullSlot = false;
        //Neu nguoi choi chi click chuot thi isDragging = false
        isDragging = false;
        //De kiem tra xem bin duoc cham vao la gi, truoc tien can tat cac bien nay
        isSausage = false;

        if (gameObject.tag == "bun bin")
        {
            //Xac dinh xem nguyen lieu duoc chon la cook food hay materials
            isMaterials = true;
            //Set slot cho nguyen lieu vua duoc chon
            if (Gameplay.cuttingboardS1 != "empty" && Gameplay.cuttingboardS2 != "empty" && Gameplay.cuttingboardS3 != "empty")
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
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 3;
            }

            //Sinh ra obj
            objDrag = Instantiate(materialBunObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }
        if (gameObject.tag == "meat bin")
        {
            isMaterials = false;

            if (Gameplay.grillS1 != "empty" && Gameplay.grillS2 != "empty" && Gameplay.grillS3 != "empty")
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
            else if (Gameplay.grillS3 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 3;
            }

            //Sinh ra obj
            objDrag = Instantiate(burgersObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }
        if (gameObject.tag == "roll bin")
        {
            isMaterials = true;

            if (Gameplay.cuttingboardS1 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
                objDrag = Instantiate(materialRollObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
                objDrag = Instantiate(materialRollObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 3;
                objDrag = Instantiate(materialRollObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else
            {
                isFullSlot = true;
            }
        }
        if (gameObject.tag == "sausage bin")
        {
            isSausage = true;

            objDrag = Instantiate(sausageObj, this.transform.position, Quaternion.identity);
            objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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
        //Neu het cho roi thi khong can lam
        if (isFullSlot)
        {
            return;
        }

        if (isSausage)
        {
            var sausage = objDrag.gameObject.GetComponent<Sausage>();

            if (sausage.isOntheRoll)
            {
                if (sausage.slotInCuttingBoard == 1)
                {
                    hotdogObj.gameObject.GetComponent<Foods>().slotInCuttingboard = 1;
                    Instantiate(hotdogObj, firstPositionOnCuttingBoard, Quaternion.identity);
                    Gameplay.cuttingboardS1 = "FullBun";
                }
                else if (sausage.slotInCuttingBoard == 2)
                {
                    hotdogObj.gameObject.GetComponent<Foods>().slotInCuttingboard = 2;
                    Instantiate(hotdogObj, secondPositionOnCuttingBoard, Quaternion.identity);
                    Gameplay.cuttingboardS2 = "FullBun";
                }
                else if (sausage.slotInCuttingBoard == 3)
                {
                    hotdogObj.gameObject.GetComponent<Foods>().slotInCuttingboard = 3;
                    Instantiate(hotdogObj, thirdPositionOnCuttingBoard, Quaternion.identity);
                    Gameplay.cuttingboardS3 = "FullBun";
                }
            }

            Destroy(sausage.gameObject);
            return;
        }


        if (isMaterials)
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
                    objDrag.transform.position = firstPositionOnCuttingBoard;
                    Gameplay.cuttingboardS1 = "JustBun";
                }
                else if (material.slotInCuttingboard == 2)
                {
                    objDrag.transform.position = secondPositionOnCuttingBoard;
                    Gameplay.cuttingboardS2 = "JustBun";
                }
                else if (material.slotInCuttingboard == 3)
                {
                    objDrag.transform.position = thirdPositionOnCuttingBoard;
                    Gameplay.cuttingboardS3 = "JustBun";
                }
            }
        }
        else
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

                if (cookFood.slotInGrill == 1)
                {
                    objDrag.transform.position = firstPositionOnGrill;
                    Gameplay. grillS1 = "full";
                }
                else if (cookFood.slotInGrill == 2)
                {
                    objDrag.transform.position = secondPositionOnGrill;
                    Gameplay.grillS2 = "full";
                }
                else if (cookFood.slotInGrill == 3)
                {
                    objDrag.transform.position = thirdPositionOnGrill;
                    Gameplay.grillS3 = "full";
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isFullSlot)
        {
            Debug.Log("Het cho");
            return;
        }
        objDragRect.anchoredPosition += eventData.delta;
    }
}
