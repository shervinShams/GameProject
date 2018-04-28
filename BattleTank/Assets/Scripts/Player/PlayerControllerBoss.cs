using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBoss : MonoBehaviour {

    public bool SecondPlayer = false;

    public GameObject playerHealthObject;
    public GameObject playerReloadObject;

    public float Velocity = 7.0f;
    public float MaximalSpeed = 40.0f;
    public float RotationSpeed = 2.0f;
    public float FrictionScale = 10.0f;

    public float ReloadSpeed = 1.0f;
    public float ShootingKick = 70.0f;

    public int damage = 10;

    public GameObject Bullet;
    public ParticleSystem trackParticles;
    public ParticleSystem burnParticles;
    public ParticleSystem explodeParticles;

    public GameManagerBoss gameManager;

    private Rigidbody2D rb;
    private float timer = 0;
    private PlayerHealth playerHealth;
    private PlayerReload playerReload;
    private PlayerSoundManager soundManager;
    private Animator animator;

    private GameObject nextBullet;

    private string vAxisName = "Vertical";
    private string hAxisName = "Horizontal";
    private string attackAxisName = "Attack";

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = playerHealthObject.GetComponent<PlayerHealth>();
        playerReload = playerReloadObject.GetComponent<PlayerReload>();
        playerReload.reloadSpeed = ReloadSpeed;
        animator = GetComponent<Animator>();
        soundManager = GetComponentInChildren<PlayerSoundManager>();
        nextBullet = Bullet;

        if (SecondPlayer)
        {
            vAxisName += "Second";
            hAxisName += "Second";
            attackAxisName += "Second";
        }
    }

    void FixedUpdate()
    {

        if (GameManagerBoss.isGamePaused())
            return;

        float vertical = Input.GetAxisRaw(vAxisName);
        float horizontal = Input.GetAxisRaw(hAxisName);
        float shoot = Input.GetAxisRaw(attackAxisName);

        Vector2 localUp = new Vector2(transform.up.x, transform.up.y);

        if (vertical != 0)
        {
            Vector2 engineForce = Vector2.Lerp(Vector2.zero, localUp * vertical * MaximalSpeed, Velocity * Time.deltaTime);
            rb.AddForce(engineForce);
            soundManager.PlayRide();
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        SetupParticles(vertical);

        Vector2 velocityVector = rb.velocity.normalized;
        float dot = Vector2.Dot(velocityVector, localUp);

        if (horizontal != 0)
        {
            if (dot < 0 && rb.velocity.magnitude > 0 && vertical < 0)
            {
                rb.MoveRotation(rb.rotation + RotationSpeed * horizontal);
            }
            else
            {
                rb.MoveRotation(rb.rotation - RotationSpeed * horizontal);
            }
        }


        if ((dot != 1) && (dot != -1) && (velocityVector.magnitude != 0))
        {
            rb.velocity = Vector2.Lerp(rb.velocity, localUp * dot * rb.velocity.magnitude, FrictionScale * Time.deltaTime);
        }

        if (shoot != 0 && timer >= ReloadSpeed)
        {
            Shoot(localUp);
        }

        timer += Time.deltaTime;

    }

    void SetupParticles(float vertical)
    {
        if (vertical == 1)
        {
            if (!trackParticles.isPlaying)
            {
                trackParticles.Play();
            }
        }
        else
        {
            trackParticles.Stop();
        }
    }

    void Shoot(Vector2 localUp)
    {
        Instantiate(nextBullet, transform.position + new Vector3(localUp.x, localUp.y, 0.0f) * 0.5f, transform.rotation);
        timer = 0.0f;
        rb.AddForce(-localUp * ShootingKick);
        playerReload.Reload();
        soundManager.PlayShoot();

        nextBullet = Bullet;
    }

    /*
	 * 
	 * Public functions
	 * 
	 */

    public void TakeDamage(int damage)
    {
        int current = playerHealth.curHealth;

        Debug.Log(current);

        playerHealth.AdjustCurrentHealth(-damage);

        current -= damage;


        if (current >= 40 && burnParticles.isPlaying)
        {

            // playerHealth.curHealth = current;
            burnParticles.Stop();

        }

        if (current <= 40 && !burnParticles.isPlaying)
        {
            //playerHealth.curHealth = current;
            burnParticles.Play();
        }

        if (playerHealth.curHealth == 0 && !GameManagerBoss.isGamePaused())
        {
            //GameOver
            animator.SetBool("isMoving", false);
            animator.SetBool("isKilled", true);
            explodeParticles.Play();
            gameManager.FinishGame(SecondPlayer);
            soundManager.PlayDeath();

        }
        else
        {
            soundManager.PlayHurt();
        }
    }

    public void Heal(int health)
    {
        playerHealth.AdjustCurrentHealth(health);
    }

    public void ResetPosition()
    {
        try
        {

            transform.localPosition = Vector3.zero;
            transform.localRotation = new Quaternion(0.0f, 0.0f, (SecondPlayer) ? 180.0f : 0.0f, 0.0f);
            animator.SetBool("isKilled", false);
            playerHealth.Show();
            playerReload.Show();
            if (burnParticles.isPlaying)
                burnParticles.Stop();

        }
        catch (NullReferenceException e)
        {


            Debug.Log(e);
        }
    }

    public void SetNextBullet(GameObject bullet)
    {
        nextBullet = bullet;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Lava"))
        {
            //audioPlayer.Play();
            soundManager.PlayHurt();
            //collision.gameObject.SendMessage("TakeDamage", Damage);
            TakeDamage(damage);

        }
    }

    /*
     * 
     * Collider delegates
     *
     */

    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Cover"))
    //    {
    //        playerHealth.Hide();
    //        playerReload.Hide();
    //    }
    //}

    //void OnTriggerStay2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Cover"))
    //    {
    //        playerHealth.Hide();
    //        playerReload.Hide();
    //    }
    //}

    //void OnTriggerExit2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Cover"))
    //    {
    //        playerHealth.Show();
    //        playerReload.Show();
    //    }
    //}
}