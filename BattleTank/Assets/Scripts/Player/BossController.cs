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
    private float lastAttackTime = 1f;
    public float attackDelay = 1f;
    public float shotForce = 250f;

    public GameObject playerHealthObject;
    public GameObject playerReloadObject;

    //public float Velocity = 7.0f;
    //public float MaximalSpeed = 40.0f;
    //public float RotationSpeed = 2.0f;
    //public float FrictionScale = 10.0f;

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
    private AudioSource audioPlayer;

    private GameObject nextBullet;

    //private string vAxisName = "Vertical";
    //private string hAxisName = "Horizontal";
    //private string attackAxisName = "Attack";

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

        //if (SecondPlayer)
        //{
        //    vAxisName += "Second";
        //    hAxisName += "Second";
        //    attackAxisName += "Second";
        //}
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
            //int shoot = 1;
            //if (shoot != 0 && timer >= ReloadSpeed)
            //{
            //    Shoot(localUp);
            //}

            timer += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer < attackRange)
            {
                Vector3 targetDir2 = target.position - transform.position;
                float angleShoot = Mathf.Atan2(targetDir2.y, targetDir2.x) * Mathf.Rad2Deg -90f;
                Quaternion q3 = Quaternion.AngleAxis(angleShoot, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q3, 180f);

                //if (timer > lastAttackTime + attackDelay)
                //{
                //    
                //    Shoot(localUp);

                //}

                int shoot = 1;
                if (shoot != 0 && timer >= ReloadSpeed)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, attackRange);
                    Shoot(localUp);    
                    
                }


            }

        }

        //float vertical = Input.;


        //float vertical = Input.GetAxisRaw(vAxisName);
        //float horizontal = Input.GetAxisRaw(hAxisName);
        //float shoot = Input.GetAxisRaw(attackAxisName);

        // Vector2 localUp = new Vector2(transform.up.x, transform.up.y);

        //if (vertical != 0)
        //{
        //    Vector2 engineForce = Vector2.Lerp(Vector2.zero, localUp * vertical * MaximalSpeed, Velocity * Time.deltaTime);
        //    rb.AddForce(engineForce);
        //    soundManager.PlayRide();
        //    animator.SetBool("isMoving", true);
        //}
        //else
        //{
        //    animator.SetBool("isMoving", false);
        //}

        //SetupParticles(vertical);

        //Vector2 velocityVector = rb.velocity.normalized;
        //float dot = Vector2.Dot(velocityVector, localUp);

        //if (horizontal != 0)
        //{
        //    if (dot < 0 && rb.velocity.magnitude > 0 && vertical < 0)
        //    {
        //        rb.MoveRotation(rb.rotation + RotationSpeed * horizontal);
        //    }
        //    else
        //    {
        //        rb.MoveRotation(rb.rotation - RotationSpeed * horizontal);
        //    }
        //}


        //if ((dot != 1) && (dot != -1) && (velocityVector.magnitude != 0))
        //{
        //    rb.velocity = Vector2.Lerp(rb.velocity, localUp * dot * rb.velocity.magnitude, FrictionScale * Time.deltaTime);
        //}

        //if (shoot != 0 && timer >= ReloadSpeed)
        //{
        //    Shoot(localUp);
        //}

        //timer += Time.deltaTime;

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