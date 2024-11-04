using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTopping : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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
        else if (slotInCuttingboard == 3)
        {
            Gameplay.cuttingboardS3 = "empty";
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
            if (collision.GetComponent<MoveSandwich>().slotInCuttingboard == slotInCuttingboard)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log("Cham phai khach");
            var customer = collision.gameObject.GetComponent<Patron>();
            if (customer.orderedFood == this.gameObject.tag)
            {
                customer.havingFood = true;
                isReturnStartPosition = false;
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
            var customer = collision.gameObject.GetComponent<Patron>();
            if (customer.orderedFood == this.gameObject.tag)
            {
                customer.havingFood = false;
                isReturnStartPosition = true;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On begin drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On end drag");
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
                if (customer.GetComponent<Patron>().havingFood)
                {
                    customer.GetComponent<Patron>().isOnEndDrag = true;
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
