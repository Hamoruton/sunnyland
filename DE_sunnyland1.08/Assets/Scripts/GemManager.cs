using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    [SerializeField] float gemPower;

    Rigidbody2D rb2D;
    GameManager gameManager;

    private bool power = true;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
       
    }

    void Update()
    {
        {
            Move();
        }
    }

    public void Move()
    {
        if (!power)
        {
            return;
        }
        rb2D.AddForce(Vector2.up * gemPower);
        GetGem();
        power = false;
    }

    public void GetGem()
    {
        gameManager.AddGemScore(1);
        gameManager.AddScore(200);
        Destroy(gameObject, 0.5f);
    }
}
