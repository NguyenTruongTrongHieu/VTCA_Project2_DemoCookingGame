using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookFood : MonoBehaviour
{
    public int slotInGrill;
    [SerializeField] private GameObject hamburger;

    [SerializeField]
    private float cookingTime = 0;
    private string ripeness;

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
        cookingTime += Time.deltaTime;
        if (cookingTime <= 3)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            ripeness = "notYet";
        }
        if ((cookingTime > 3 && cookingTime <= 6) && (transform.position.x > 5))
        { 
            GetComponent<SpriteRenderer>().color = new Color (1, 1, 0);
            ripeness = "ripe";
        }
        if ((cookingTime > 6) && (transform.position.x > 5))
        {
            ripeness = "burn";
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }

    private void OnMouseDown()
    {
        //Neu chua chin thi chua duoc dem di
        if (ripeness == "notYet")
        {
            return;
        }
        //Kiem tra thit o slot nao de lam trong slot do
        if (slotInGrill == 1)
        {
            Gameplay.grillS1 = "empty";
        }
        if (slotInGrill == 2)
        {
            Gameplay.grillS2 = "empty";
        }
        if (slotInGrill == 3)
        {
            Gameplay.grillS3 = "empty";
        }
        //Neu bi khet thi huy
        if (ripeness == "burn")
        {
            Debug.Log("khet");
            Destroy(gameObject);
            return;
        }

        if ((Gameplay.cuttingboardS1 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 1;
            Instantiate(hamburger, new Vector2(firstPositionOnCuttingBoard, -1f), Quaternion.identity);
            Destroy(gameObject);
            Gameplay.cuttingboardS1 = "FullBun";
        }
        else
            if ((Gameplay.cuttingboardS2 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 2;
            Instantiate(hamburger, new Vector2(secondPositionOnCuttingBoard, -1f), Quaternion.identity);
            Destroy(gameObject);
            Gameplay.cuttingboardS2 = "FullBun";
        }
        else
            if ((Gameplay.cuttingboardS3 == "JustBun"))
        {
            hamburger.gameObject.GetComponent<MoveTopping>().slotInCuttingboard = 3;
            Instantiate(hamburger, new Vector2(thirdPositionOnCuttingBoard, -1f), Quaternion.identity);
            Destroy(gameObject);
            Gameplay.cuttingboardS3 = "FullBun";
        }
    }
}
