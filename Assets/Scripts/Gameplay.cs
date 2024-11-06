using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    //slot tren thot
    public static string cuttingboardS1 = "empty";
    public static string cuttingboardS2 = "empty";
    public static string cuttingboardS3 = "empty";

    //slot tren vi nuong
    public static string grillS1 = "empty";
    public static string grillS2 = "empty";
    public static string grillS3 = "empty";

    //slot hang doi cua customer
    public static string queueS1 = "empty";
    public static string queueS2 = "empty";
    public static string queueS3 = "empty";

    //list do an
    public static List<string> orderedFood = new List<string>() { "hamburger", "hotdog" };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
