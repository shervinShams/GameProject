using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float BulletSpeed = 50.0f;
    public float AccelerationTime = 1.0f;
    public int Damage = 10;
    public int WallDamage = 1;
    public ParticleSystem blastParticles;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioPlayer;
    private bool blasting = false;
    private float timer = 0.0f;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer <= 1.0f && !blasting)
        {
            rb.AddForce(transform.up * BulletSpeed * Time.deltaTime);
        }

        timer += Time.deltaTime;

        if (blasting)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
        }
    }

    void Update()
    {
        if (GameManager.isGamePaused())
            return;
       

        if (blasting)
        {
            if (blastParticles.isStopped)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        audioPlayer.Play();
        if (boxCollider)

        {
            boxCollider.enabled = false;
        }

        if (collision.gameObject.CompareTag("Destructible"))
        {
            collision.gameObject.SendMessage("TakeDamage", WallDamage);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("TakeDamage", Damage);
        }

        if (collision.gameObject.CompareTag("PlayerSnow"))
        {
            collision.gameObject.SendMessage("TakeDamage", Damage);
        }

        if (collision.gameObject.CompareTag("PlayerLava"))
        {
            collision.gameObject.SendMessage("TakeDamage", Damage);
        }

        if (collision.gameObject.CompareTag("PlayerBoss"))
        {
            collision.gameObject.SendMessage("TakeDamage", 5);
        }

        if (spriteRenderer)
        {
            spriteRenderer.sprite = null;
            blasting = true;
            blastParticles.Play();
        }
    }
}
