using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinControll : MonoBehaviour
{
    [SerializeField]
    private Transform materialBunObj;
    [SerializeField]
    private Transform burgersObj;
    [SerializeField]
    private Transform materialRollObj;
    [SerializeField]
    private Transform sausageObj;

    private Vector2 firstPositionOnCuttingBoard = new Vector2(-1, -1.25f);
    private Vector2 secondPositionOnCuttingBoard = new Vector2(0, -1.25f);
    private Vector2 thirdPositionOnCuttingBoard = new Vector2(1, -1.25f);

    private Vector2 firstPositionOnGrill = new Vector2(-6f, -1.25f);
    private Vector2 secondPositionOnGrill = new Vector2(-5f, -1.25f);
    private Vector2 thirdPositionOnGrill = new Vector2(-4f, -1.25f);

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
        if (gameObject.tag == "bun bin")
        {
            if (Gameplay.cuttingboardS1 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
                Instantiate(materialBunObj, firstPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS1 = "JustBun";
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 2;
                Instantiate(materialBunObj, secondPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS2 = "JustBun";
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                materialBunObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 3;
                Instantiate(materialBunObj, thirdPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS3 = "JustBun";
            }
        }
        if (gameObject.tag == "meat bin")
        {
            if (Gameplay.grillS1 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 1;
                var meat = Instantiate(burgersObj, firstPositionOnGrill, Quaternion.identity);
                Gameplay.grillS1 = "full";
            }
            else if (Gameplay.grillS2 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 2;
                Instantiate(burgersObj, secondPositionOnGrill, Quaternion.identity);
                Gameplay.grillS2 = "full";
            }
            else if (Gameplay.grillS3 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 3;
                Instantiate(burgersObj, thirdPositionOnGrill, Quaternion.identity);
                Gameplay.grillS3 = "full";
            }
        }
        if (gameObject.tag == "roll bin")
        {
            if (Gameplay.cuttingboardS1 == "empty")
            {
                materialRollObj.gameObject.GetComponent<Materials>().slotInCuttingboard = 1;
                Instantiate(materialRollObj, firstPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS1 = "JustRoll";
                Debug.Log(Gameplay.cuttingboardS1);
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
}
