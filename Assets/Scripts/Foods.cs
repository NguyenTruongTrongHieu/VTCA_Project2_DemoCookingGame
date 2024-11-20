using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Foods : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int slotInCuttingboard;
    private bool isDragging;

    public bool isReturnStartPosition;
    private Vector3 startPosition;

    [SerializeField] private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        //Dua doi tuong hien tai tro thanh con cua canvas dung de keo tha
        var dragCanvas = GameObject.FindGameObjectWithTag("UIDrag");
        this.transform.SetParent(dragCanvas.transform);
        //Chinh scale cua object lai
        this.transform.localScale = new Vector3(1, 1, 1);

        rectTransform = this.GetComponent<RectTransform>();
        startPosition = this.transform.position;
        isReturnStartPosition = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetSlotInCuttingBoard()
    {
        if (slotInCuttingboard == 1)
        {
            Gameplay.cuttingboardS1 = "empty";
        }
        else if (slotInCuttingboard == 2)
        {
            Gameplay.cuttingboardS2 = "empty";
        }
    }

    private void ReturnStartPosition()
    {
        transform.position = startPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TrashBin"))
        {
            Debug.Log("Cham phai thung rac");
            isReturnStartPosition = false;
        }

        //Neu cham phai phan nguyen lieu banh thi huy phan nguyen lieu di
        if (collision.gameObject.CompareTag("bun"))
        {
            //Xet 2 vi tri cua mon an hoan chinh va nguyen lieu ban dau, trung nhau thi moi huy
            if (collision.GetComponent<Materials>().slotInCuttingboard == slotInCuttingboard)
            {
                Destroy(collision.gameObject);
            }
        }

        //Tuong tu voi bun
        if (collision.gameObject.CompareTag("roll"))
        {
            if (collision.GetComponent<Materials>().slotInCuttingboard == slotInCuttingboard)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Customer"))
        {
            var customer = collision.gameObject.GetComponent<Customers>();
            if (!customer.isWaiting || customer.isAlreadyDone || customer.isOutOfTime)
            {
                return;
            }

            for (int i = 0; i < customer.orderedFoods.Count; i++)
            {
                if (customer.orderedFoods[i] == this.gameObject.tag)
                {
                    customer.havingFood = true;
                    customer.orderFoodChoose = i + 1;
                    Debug.Log(customer.orderFoodChoose);
                    isReturnStartPosition = false;
                    return;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TrashBin"))
        {
            isReturnStartPosition =false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TrashBin"))
        {
            //Dua object ve cho cu
            Debug.Log("Roi khoi thung rac");
            isReturnStartPosition = true;
        }

        if (collision.gameObject.CompareTag("Customer"))
        {
            var customer = collision.gameObject.GetComponent<Customers>();

            if (!customer.isWaiting || customer.isAlreadyDone)
            {
                return;
            }

            for (int i = 0; i < customer.orderedFoods.Count; i++)
            {
                if (customer.orderedFoods[i] == this.gameObject.tag)
                {
                    customer.havingFood = false;
                    isReturnStartPosition = true;
                    return;
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Kiem tra xem sau khi tha object co can quay ve cho cu khong
        if (isReturnStartPosition)
        {
            ReturnStartPosition();
        }
        //Neu khong quay ve thi huy object
        else 
        {
            //Khi huy object thi slot o cuttingboard se bi trong
            SetSlotInCuttingBoard();

            //Tim tat ca customer dang hoat dong, customer nao co havingFood = true
            //tuc la dang keo do an toi do thi bien isOnEndDrag cua cus do ve true, neu khong thi bo qua
            var customers = GameObject.FindGameObjectsWithTag("Customer");
            foreach (var customer in customers) {
                if (customer.GetComponent<Customers>().havingFood)
                {
                    customer.GetComponent<Customers>().isOnEndDrag = true;
                    Debug.Log("On end drag");
                }
            }

            Destroy(this.gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Lam object di theo con chuot hoac ngon tay
        rectTransform.anchoredPosition += eventData.delta;
    }
}
