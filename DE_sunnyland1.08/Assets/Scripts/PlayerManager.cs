using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum MOVE_DIRECTION
    {
        STOP,
        LEFT,
        RIGHT
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

    [SerializeField] LayerMask blockLayer;
    [SerializeField] LayerMask objLayer;
    [SerializeField] GameManager gameManager;
    //SE
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip stampSound;
    [SerializeField] AudioClip cherrySound;
    [SerializeField] AudioClip moveSound;

    private float speed;
    private float jumpSpeed = 550;
    private bool isClear = false;
    private bool isDead = false;
    private bool isBlock = false;

    Rigidbody2D rb2D;
    Animator animator;
    AudioSource audioSource;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();

        Jump();
    }

    private void Move()
    {
        if (isDead || isClear) 
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(x));

        if (x == 0)
        {
            moveDirection = MOVE_DIRECTION.STOP;
        }
        else if (x > 0)
        {
            moveDirection = MOVE_DIRECTION.RIGHT;
        }
        else if (x < 0) 
        {
            moveDirection = MOVE_DIRECTION.LEFT;
        }

        switch (moveDirection)
        {
            case MOVE_DIRECTION.STOP:
                speed = 0;
                break;
            case MOVE_DIRECTION.RIGHT:
                speed = 5;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case MOVE_DIRECTION.LEFT:
                speed = -5;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        Vector2 pos = transform.position;
        if (pos.x < -17.5f)
        {
            pos.x = -17.5f;
            transform.position = pos;
        }
    }

    private void Jump()
    {
        if (isClear)
        {
            return;
        }
        if (IsGround() || IsObject())
        {
            if (Input.GetKeyDown("space"))
            {
                rb2D.AddForce(Vector2.up * jumpSpeed);
                animator.SetBool("isJumping", true);
                audioSource.PlayOneShot(jumpSound);
            }
            isBlock = false;
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private bool IsGround()
    {
        Debug.DrawLine(transform.position - transform.right * 0.4f, transform.position - transform.right * 0.1f - transform.up * 0.1f);
        Debug.DrawLine(transform.position + transform.right * 0.3f, transform.position - transform.right * 0.1f - transform.up * 0.1f);
        return Physics2D.Linecast(transform.position - transform.right * 0.4f, transform.position - transform.right * 0.1f - transform.up * 0.1f, blockLayer) ||
               Physics2D.Linecast(transform.position + transform.right * 0.3f, transform.position - transform.right * 0.1f - transform.up * 0.1f, blockLayer);
    }

    private bool IsObject()
    {
        return Physics2D.Linecast(transform.position - transform.right * 0.3f, transform.position - transform.up * 0.1f, objLayer) ||
               Physics2D.Linecast(transform.position + transform.right * 0.3f, transform.position - transform.up * 0.1f, objLayer);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDead || isClear) 
        {
            return;
        }

        if (collider.gameObject.tag == "Opossum")
        {
            OpossumManager opossum = collider.gameObject.GetComponent<OpossumManager>();
            if (transform.position.y + 0.2f > opossum.transform.position.y)
            {
                //垂直衝突→playerを浮上
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                rb2D.AddForce(Vector2.up * 300);
                audioSource.PlayOneShot(stampSound);
                opossum.DestroyEnemy();
            }
            else
            {
                //水平衝突
                PlayerDeath();
            }
        }

        if (collider.gameObject.tag == "GameOver")
        {
            PlayerDeath();
        }

        if (collider.gameObject.tag == "GameClear")
        {
            isClear = true;
            rb2D.velocity = new Vector2(0, 0);
            animator.Play("playerIdle");
            gameManager.GameClear();
        }

        if (collider.gameObject.tag == "GBlock")
        {
            GBlockManager gBlockManager = collider.gameObject.GetComponent<GBlockManager>();
            if (!isBlock && transform.position.y + 0.9f < collider.gameObject.transform.position.y - 0.5f) 
            {
                gBlockManager.Move();
                isBlock = true;
            }
        }

        if (collider.gameObject.tag == "CBlock")
        {
            CBlockManager cBlockManager = collider.gameObject.GetComponent<CBlockManager>();
            if (transform.position.y + 0.9f < collider.gameObject.transform.position.y - 0.5f) 
            {
                cBlockManager.Move();
            }
        }
        if (collider.gameObject.tag == "Hide")
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //SceneManager.LoadScene();     2秒
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Frog")
        {
            FrogManager frog = collision.gameObject.GetComponent<FrogManager>();
            if (transform.position.y + 0.2f > frog.transform.position.y) 
            {
                //垂直衝突→playerを浮上
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                rb2D.AddForce(Vector2.up * 300);
                audioSource.PlayOneShot(stampSound);
                frog.DestroyEnemy();
            }
            else
            {
                //水平衝突
                PlayerDeath();
            }
        }

        if (collision.gameObject.tag == "Cherry")
        {
            collision.gameObject.GetComponent<CherryManager>().GetCherry();
            audioSource.PlayOneShot(cherrySound);
        }
    }

    private void PlayerDeath()
    {
        isDead = true;

        rb2D.velocity = new Vector2(0, 0);
        animator.Play("playerDeathAnimation");
        rb2D.AddForce(Vector2.up * jumpSpeed);

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        Destroy(boxCollider2D);
        Destroy(circleCollider2D);

        gameManager.GameOver();
    }
}



