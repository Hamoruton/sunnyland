using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    [SerializeField] GameObject getEffect;

    GameManager gameManager;

    public enum MOVE_DIRECTION
    {
        STOP,
        LEFT,
        RIGHT
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.LEFT;

    private float speed;
    private Rigidbody2D rb2D;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (moveDirection)
        {
            case MOVE_DIRECTION.STOP:
                speed = 0;
                break;
            case MOVE_DIRECTION.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(5, 5, 1);
                break;
            case MOVE_DIRECTION.LEFT:
                speed = -3;
                transform.localScale = new Vector3(-5, 5, 1);
                break;
        }
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }
    
    private void ChangeDirection()
    {
        if (moveDirection == MOVE_DIRECTION.RIGHT)
        {
            moveDirection = MOVE_DIRECTION.LEFT;
        }
        else if (moveDirection == MOVE_DIRECTION.LEFT)
        {
            moveDirection = MOVE_DIRECTION.RIGHT;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Opossum" || collision.gameObject.tag == "Frog" ||
            collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "GBlock")  
        {
            //Debug.Log("collision");
            ChangeDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "NoCollider")
        {
            Debug.Log("collision");
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            Destroy(boxCollider2D);
            Destroy(gameObject, 1.0f);
        }
    }

    public void GetCherry()
    {
        Instantiate(getEffect, transform.position, transform.rotation);
        gameManager.AddScore(1000);
        Destroy(this.gameObject);
    }
}
