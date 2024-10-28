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

    private float firstPositionOnCuttingBoard = -1;
    private float secondPositionOnCuttingBoard = 0;
    private float thirdPositionOnCuttingBoard = 1;

    private float firstPositionOnGrill = 5.5f;
    private float secondPositionOnGrill = 6.5f;
    private float thirdPositionOnGrill = 7.5f;

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
                Instantiate(bottomBunObj, new Vector2(firstPositionOnCuttingBoard, -1f), Quaternion.identity);
                Gameplay.cuttingboardS1 = "JustBun";
            }
            else if (Gameplay.cuttingboardS2 == "empty")
            {
                bottomBunObj.gameObject.GetComponent<MoveSandwich>().slotInCuttingboard = 2;
                Instantiate(bottomBunObj, new Vector2(secondPositionOnCuttingBoard, -1f), Quaternion.identity);
                Gameplay.cuttingboardS2 = "JustBun";
            }
            else if (Gameplay.cuttingboardS3 == "empty")
            {
                bottomBunObj.gameObject.GetComponent<MoveSandwich>().slotInCuttingboard = 3;
                Instantiate(bottomBunObj, new Vector2(thirdPositionOnCuttingBoard, -1f), Quaternion.identity);
                Gameplay.cuttingboardS3 = "JustBun";
            }
        }
        if (gameObject.tag == "meat bin")
        {
            if (Gameplay.grillS1 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 1;
                var meat = Instantiate(burgersObj, new Vector2(firstPositionOnGrill, -0.74f), Quaternion.identity);
                Gameplay.grillS1 = "full";
            }
            else if (Gameplay.grillS2 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 2;
                Instantiate(burgersObj, new Vector2(secondPositionOnGrill, -0.74f), Quaternion.identity);
                Gameplay.grillS2 = "full";
            }
            else if (Gameplay.grillS3 == "empty")
            {
                burgersObj.gameObject.GetComponent<CookFood>().slotInGrill = 3;
                Instantiate(burgersObj, new Vector2(thirdPositionOnGrill, -0.74f), Quaternion.identity);
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
