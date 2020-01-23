using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBlockManager : MonoBehaviour
{
    [SerializeField] GameObject cherry;
    //SE
    [SerializeField] AudioClip cherryAppearSound;

    SpriteRenderer spriteRenderer;
    CherryManager cherryManager;
    AudioSource audioSource;

    public float bounceHeight = 0.5f;
    public float bounceSpeed = 4.0f;
    public bool isInvisible;

    private bool canBounce = true;
    private Vector2 originalPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cherryManager = cherry.GetComponent<CherryManager>();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.localPosition;
        if (isInvisible)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void Move()
    {
        if (canBounce)
        {
            spriteRenderer.enabled = true;
            Instantiate(cherry, transform.position + transform.up, transform.rotation);
            audioSource.PlayOneShot(cherryAppearSound);
            spriteRenderer.color = new Color32(150, 110, 110, 255);
            canBounce = false;
            StartCoroutine(Bounce());
        }
    }

    IEnumerator Bounce()
    {
        while (true)
        {
            transform.localPosition = new Vector2(transform.position.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);
            if (transform.localPosition.y >= originalPosition.y + bounceHeight)
                break;

            yield return null;
        }

        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);
            if (transform.localPosition.y <= originalPosition.y)
            {
                transform.localPosition = originalPosition;
                break;
            }
            yield return null;
        }
    }
}
