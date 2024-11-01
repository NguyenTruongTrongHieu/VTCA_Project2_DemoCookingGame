using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinControll : MonoBehaviour
{
    [SerializeField]
    private Transform bottomBunObj;
    [SerializeField]
    private Transform topBunObj;
    [SerializeField]
    private Transform burgersObj;
    [SerializeField]
    private Transform backRollObj;
    [SerializeField]
    private Transform frontRollObj;
    [SerializeField]
    private Transform hotDogObj;

    private Vector2 firstPositionOnCuttingBoard = new Vector2(-1, -1.25f);
    private Vector2 secondPositionOnCuttingBoard = new Vector2(0, -1.25f);
    private Vector2 thirdPositionOnCuttingBoard = new Vector2(1, -1.25f);

    private Vector2 firstPositionOnGrill = new Vector2(5.5f, -1.25f);
    private Vector2 secondPositionOnGrill = new Vector2(6.5f, -1.25f);
    private Vector2 thirdPositionOnGrill = new Vector2(7.5f, -1.25f);

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
                bottomBunObj.gameObject.GetComponent<MoveSandwich>().slotInCuttingboard = 1;
                Instantiate(bottomBunObj, firstPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS1 = "JustBun";
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                bottomBunObj.gameObject.GetComponent<MoveSandwich>().slotInCuttingboard = 2;
                Instantiate(bottomBunObj, secondPositionOnCuttingBoard, Quaternion.identity);
                Gameplay.cuttingboardS2 = "JustBun";
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                bottomBunObj.gameObject.GetComponent<MoveSandwich>().slotInCuttingboard = 3;
                Instantiate(bottomBunObj, thirdPositionOnCuttingBoard, Quaternion.identity);
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
            
        }
        if (gameObject.tag == "hotdog bin")
        {

        }
    }
}
