using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Foods : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Food position")]
    public int slotInCuttingboard;
    private bool isDragging;

    public bool isReturnStartPosition;
    private bool isOnTrashBin = false;
    private Vector3 startPosition;

    [Header("Drag and drop")]
    //Keo tha
    [SerializeField] private RectTransform rectTransform;
    private Vector2 screenPosition;
    private Vector3 worldPosition;

    [Header("Minus score")]
    [SerializeField] private int minusScore = 3;

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
            isReturnStartPosition = false;
            isOnTrashBin = true;
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

        //Tuong tu voi bun
        if (collision.gameObject.CompareTag("hamburger"))
        {
            if (collision.GetComponent<Foods>().slotInCuttingboard == slotInCuttingboard)
            {
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log("cus");
            var customer = collision.gameObject.GetComponent<Customers>();
            if (!customer.isWaiting || customer.isAlreadyDone || customer.isOutOfTime)
            {
                return;
            }

            for (int i = 0; i < customer.orderedFoods.Count; i++)
            {
                if (customer.orderedFoods[i] == this.gameObject.tag)
                {
                    Debug.Log("Dung mon");
                    customer.havingFood = true;
                    customer.orderFoodChoose = i + 1;
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
            isReturnStartPosition = true;
            isOnTrashBin = false;
        }

        if (collision.gameObject.CompareTag("Customer"))
        {
            var customer = collision.gameObject.GetComponent<Customers>();

            if (!customer.isWaiting || customer.isAlreadyDone)
            {
                return;
            }

            //Lap de tim order phu hop voi mon an duoc keo den khach hang
            for (int i = 0; i < customer.orderedFoods.Count; i++)
            {
                if (customer.orderedFoods[i] == this.gameObject.tag)
                {
                    Debug.Log("roi cus");
                    customer.havingFood = false;
                    isReturnStartPosition = true;
                    return;
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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

            //Neu food dang o trash bin
            if (isOnTrashBin)
            {
                //Tru diem khi vut vao thung rac
                Gameplay gameplay = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gameplay>();
                Gameplay.score = Mathf.Max(Gameplay.score - minusScore, 0);
                gameplay.UpdateTextScore();

                //Them am thanh
                AudioManager.audioInstance.PlaySFX("TrashBin");
            }

            //Tim tat ca customer dang hoat dong, customer nao co havingFood = true
            //tuc la dang keo do an toi do thi bien isOnEndDrag cua cus do ve true, neu khong thi bo qua
            var customers = GameObject.FindGameObjectsWithTag("Customer");
            foreach (var customer in customers) {
                if (customer.GetComponent<Customers>().havingFood)
                {
                    customer.GetComponent<Customers>().isOnEndDrag = true;

                    //Them am thanh
                    AudioManager.audioInstance.PlaySFX("ReceiveFood");
                }
            }

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
