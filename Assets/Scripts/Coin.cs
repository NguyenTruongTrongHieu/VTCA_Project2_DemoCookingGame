using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameObject target;
    private Vector3 targetDirection;
    private Rigidbody2D rb2D;

    private float speed = 10f;
    private float coinScale = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("TextScore");
        targetDirection = GameObject.FindGameObjectWithTag("TextScore").transform.position;
        this.transform.localScale = Vector3.zero;
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x < coinScale)//Phong to scale cua coin tu tu len den khi vua thi moi di chuyen ( hieu ung xuat hien )
        {
            this.transform.localScale += new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 2 * Time.deltaTime);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetDirection, speed);
            //Neu da den text score thi destroy
            if (this.transform.position.y >= targetDirection.y)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        //Them am thanh
        Gameplay.isDestroyStar = true;
        AudioManager.audioInstance.PlaySFX("EarnScore");
    }
}
