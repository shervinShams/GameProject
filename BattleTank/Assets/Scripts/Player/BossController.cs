using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    public bool SecondPlayer = false;

    
    public Transform[] patrolPoints;
    public float patrolSpeed = 1f;
    public float chaseSpeed = 1f;
    Transform currentPatrolPoint;
    int currentPatrolIndex = 0;
    public Transform target;
    public float chaseRange = 10f;
    public float attackRange = 8f;

    public GameObject playerHealthObject;
    public GameObject playerReloadObject;

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

        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
    }

    void FixedUpdate()
    {

        if (GameManagerBoss.isGamePaused())
            return;

        transform.Translate(Vector3.up * Time.deltaTime * patrolSpeed);

        if (Vector3.Distance(transform.position , currentPatrolPoint.position) < 0.1f)
        {

            soundManager.PlayRide();
            animator.SetBool("isMoving", true);
            if (currentPatrolIndex + 1 < patrolPoints.Length)
            {
                currentPatrolIndex++;

            }
            else
            {
                currentPatrolIndex = 0;
            }

            currentPatrolPoint = patrolPoints[currentPatrolIndex];
 
        }

        Vector2 localUp = new Vector2(transform.up.x, transform.up.y);
        Vector3 patrolPointDir = currentPatrolPoint.position - transform.position;
        float angle = Mathf.Atan2(patrolPointDir.y, patrolPointDir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);
        SetupParticles(1);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < chaseRange) 
        {
            Vector3 targetDir = (target.position - transform.position ).normalized ;
            float angleE = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q2 = Quaternion.AngleAxis(angleE, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q2, 180f);
            transform.Translate(Vector3.up * Time.deltaTime * chaseSpeed);
           
            timer += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer < attackRange)
            {
                Vector3 targetDir2 = target.position - transform.position;
                float angleShoot = Mathf.Atan2(targetDir2.y, targetDir2.x) * Mathf.Rad2Deg -90f;
                Quaternion q3 = Quaternion.AngleAxis(angleShoot, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q3, 180f);

                int shoot = 1;
                if (shoot != 0 && timer >= ReloadSpeed)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, attackRange);
                    Shoot(localUp);    
                    
                }
            }
        }
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
        Instantiate(nextBullet ,transform.position + new Vector3(localUp.x , localUp.y ) * 0.5f ,transform.rotation );
        timer = 0.0f;
        rb.AddForce(-localUp * ShootingKick);
        playerReload.Reload();
        soundManager.PlayShoot();

        nextBullet = Bullet;
        //
    }

    /*
	 * 
	 * Public functions
	 * 
	 */

    public void TakeDamage(int damage)
    {
        playerHealth.AdjustCurrentHealth(-damage);
        if (playerHealth.curHealth <= 20 && !burnParticles.isPlaying)
        {
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
        if (collision.transform != this.transform)
        {

        }

        if (collision.gameObject.CompareTag("Lava"))
        {
           
            soundManager.PlayHurt();
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