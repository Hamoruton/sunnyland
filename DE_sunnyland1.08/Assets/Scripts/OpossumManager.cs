using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumManager : MonoBehaviour
{
    public enum MOVE_DIRECTION
    {
        STOP,
        LEFT,
        RIGHT
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.RIGHT;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] LayerMask objLayer;
    [SerializeField] GameObject explosionEffect;

    private float speed;
    Rigidbody2D rb2D;
    GameManager gameManager;

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
        if (!IsGround() || IsObject())
        {
            ChangeDirection();
        }

        switch (moveDirection)
        {
            case MOVE_DIRECTION.STOP:
                speed = 0;
                break;
            case MOVE_DIRECTION.RIGHT:
                speed = 1;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case MOVE_DIRECTION.LEFT:
                speed = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }

    private bool IsGround()
    {
        Vector3 startVec = transform.position + transform.right * 0.5f * transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec, endVec, blockLayer);
    }

    private bool IsObject()
    {
        Vector3 startVec = transform.position + transform.right * 0.5f * transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec, endVec, objLayer);
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

    public void DestroyEnemy()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        gameManager.AddScore(100);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Opossum")
        {
            ChangeDirection();
        }
        if (collider.gameObject.tag == "Frog")
        {
            ChangeDirection();
        }
    }
}
