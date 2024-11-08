using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BinControll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private GameObject materialBunObj;
    [SerializeField]
    private GameObject burgersObj;
    [SerializeField]
    private GameObject materialRollObj;
    [SerializeField]
    private GameObject sausageObj;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Chinh bien kiem tra xem co full slot o thot hoac vi nuong chua ve false
        //Neu slot can xet da full thi se xet lai o cac ham if ben trong
        isFullSlot = false;

        if (gameObject.tag == "bun bin")
        {
            //Xac dinh xem nguyen lieu duoc chon la cook food hay materials
            isMaterials = true;
            //Set slot cho nguyen lieu vua duoc chon
            if (Gameplay.cuttingboardS1 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
                objDrag = Instantiate(materialBunObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
                objDrag = Instantiate(materialBunObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 3;
                objDrag = Instantiate(materialBunObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else
            { 
                isFullSlot = true;
            }
        }
        if (gameObject.tag == "meat bin")
        {
            isMaterials = false;

            if (Gameplay.grillS1 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 1;
                objDrag = Instantiate(burgersObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.grillS2 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 2;
                objDrag = Instantiate(burgersObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else if (Gameplay.grillS3 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 3;
                objDrag = Instantiate(burgersObj, this.transform.position, Quaternion.identity);
                objDragRect = objDrag.gameObject.GetComponent<RectTransform>();
            }
            else
            {
                isFullSlot = true;
            }
        }
        if (gameObject.tag == "roll bin")
        {
            if (Gameplay.cuttingboardS1 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
                Instantiate(materialRollObj, firstPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS1 = "JustRoll";
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
                Instantiate(materialRollObj, secondPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS2 = "JustRoll";
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 3;
                Instantiate(materialRollObj, thirdPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS3 = "JustRoll";
            }
        }
        if (gameObject.tag == "sausage bin")
        {
            if (Gameplay.grillS1 == "empty")
            {
                sausageObj.gameObject.GetComponent<CookFood>().slotInGrill = 1;
                var meat = Instantiate(sausageObj, firstPositionOnGrill, Quaternion.identity);
                Gameplay.grillS1 = "full";
            }
            else if (Gameplay.grillS2 == "empty")
            {
                sausageObj.gameObject.GetComponent<CookFood>().slotInGrill = 2;
                Instantiate(sausageObj, secondPositionOnGrill, Quaternion.identity);
                Gameplay.grillS2 = "full";
            }
            else if (Gameplay.grillS3 == "empty")
            {
                sausageObj.gameObject.GetComponent<CookFood>().slotInGrill = 3;
                Instantiate(sausageObj, thirdPositionOnGrill, Quaternion.identity);
                Gameplay.grillS3 = "full";
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
