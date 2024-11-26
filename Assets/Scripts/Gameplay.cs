using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    //Diem so trong man choi
    public static uint score;
    public TextMeshProUGUI textScore;

    //slot tren thot
    public static string cuttingboardS1 = "empty";
    public static string cuttingboardS2 = "empty";

    //slot tren vi nuong
    public static string grillS1 = "empty";
    public static string grillS2 = "empty";

    //slot hang doi cua customer
    public static string queueS1 = "empty";
    public static string queueS2 = "empty";
    public static string queueS3 = "empty";

    //check player cham vao bin
    public static bool isChooseBin;

    private void Awake()
    {
        score = 0;

        isChooseBin = false;

        cuttingboardS1 = "empty";
        cuttingboardS2 = "empty";

        grillS1 = "empty";
        grillS2 = "empty";

        queueS1 = "empty";
        queueS2 = "empty";
        queueS3 = "empty";
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextScore();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateTextScore()
    {
        textScore.text = $"$: {score}";
    }
}
