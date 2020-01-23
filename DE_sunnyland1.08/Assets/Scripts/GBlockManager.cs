using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBlockManager : MonoBehaviour
{
    [SerializeField] GameObject gem;
    //SE
    [SerializeField] AudioClip gemSound;

    SpriteRenderer spriteRenderer;
    GemManager gemManager;
    AudioSource audioSource;

    public float bounceHeight = 0.5f;
    public float bounceSpeed = 4.0f;
    public int maxGetGem;

    private bool canBounce = true;
    private Vector2 originalPosition;
    private int countGem = 0;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gemManager = gem.GetComponent<GemManager>();
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.localPosition;
    }

    public void Move()
    {
        if (canBounce)
        {
            Instantiate(gem, transform.position + transform.up, transform.rotation);
            audioSource.PlayOneShot(gemSound);

            if (countGem == maxGetGem - 1)
            {
                spriteRenderer.color = new Color32(150, 110, 110, 255);
                canBounce = false;
            }

            StartCoroutine(Bounce());
            countGem += 1;
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
