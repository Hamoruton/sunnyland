using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogManager : MonoBehaviour
{
    [SerializeField] float changeStatusTime;
    [SerializeField] GameObject explosionEffect;

    Rigidbody2D rb2D;
    Animator animator;
    GameManager gameManager;

    public enum MOVE_STATUS
    {
        STOP,
        JUMP
    }
    MOVE_STATUS moveStatus = MOVE_STATUS.STOP;

    private float countTime;
    private float jumpSpeed = 450;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        countTime = 0;
    }

    void Update()
    {
        switch (moveStatus)
        {
            case MOVE_STATUS.STOP:
                Stop();
                break;
            case MOVE_STATUS.JUMP:
                Jump();
                break;
        }
    }

    private void Stop()
    {
        countTime += Time.deltaTime;
        //Debug.Log("countTime" + countTime);
        if (countTime >= changeStatusTime)
        {
            moveStatus = MOVE_STATUS.JUMP;
            animator.SetBool("isJumping", true);
            countTime = 0;
        }
    }

    private void Jump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(Vector2.up * jumpSpeed);
        moveStatus = MOVE_STATUS.STOP;
        animator.SetBool("isJumping", false);
    }

    public void DestroyEnemy()
    {
        gameManager.AddScore(100);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
