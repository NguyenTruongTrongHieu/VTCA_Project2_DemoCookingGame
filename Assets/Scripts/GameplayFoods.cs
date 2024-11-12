using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayFoods : MonoBehaviour
{
    public static GameplayFoods instance;

    public OrderFood[] orderFoods;

    private void Awake()
    {
        instance = this;
    }
}
