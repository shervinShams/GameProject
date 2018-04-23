using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{

    public int lifeGiving = 20;
    public float removingTime = 0.45f;

    private ParticleSystem particles;
    private AudioSource audioPlayer;

    private bool toRemove = false;
    private float removeTimer = 0.0f;

    // Use this for initialization
    void Awake()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        audioPlayer = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (toRemove)
        {
            removeTimer += Time.deltaTime;

            if (removeTimer >= removingTime)
            {
                Destroy(gameObject);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();

            PlayerHeart(player);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }

        if (collider.gameObject.CompareTag("PlayerSnow"))
        {
            PlayeContorollerSnow playerSnow = collider.gameObject.GetComponent<PlayeContorollerSnow>();

            PlayerSnowHeart(playerSnow);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }

        if (collider.gameObject.CompareTag("PlayerLava"))
        {
            PlayerControllerLava playerSnow = collider.gameObject.GetComponent<PlayerControllerLava>();

            PlayerLavaHeart(playerSnow);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }


        if (collider.gameObject.CompareTag("PlayerBoss"))
        {
            PlayerControllerBoss playerBoss = collider.gameObject.GetComponent<PlayerControllerBoss>();

            PlayerBossHeart(playerBoss);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }
    }

    private void PlayerHeart(PlayerController player)
    {
        player.Heal(lifeGiving);
    }


    private void PlayerSnowHeart(PlayeContorollerSnow playerSnow)
    {
        playerSnow.Heal(lifeGiving);
    }

    private void PlayerLavaHeart(PlayerControllerLava playerLava)
    {
        playerLava.Heal(lifeGiving);
    }

    private void PlayerBossHeart(PlayerControllerBoss playerBoss)
    {
        playerBoss.Heal(lifeGiving);
    }

}
