using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicPickup : MonoBehaviour
{

    public float removingTime = 0.45f;
    public GameObject atomicBullet;

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

            PlayerAtomic(player);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }

        if (collider.gameObject.CompareTag("PlayerSnow"))
        {
            PlayeContorollerSnow playerSnow = collider.gameObject.GetComponent<PlayeContorollerSnow>();

            PlayerSnowAtomic(playerSnow);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }

        if (collider.gameObject.CompareTag("PlayerLava"))
        {
            PlayerControllerLava playerSnow = collider.gameObject.GetComponent<PlayerControllerLava>();

            PlayerLavaAtomic(playerSnow);

            audioPlayer.Play();
            particles.Play();

            toRemove = true;
        }
    }

    private void PlayerAtomic(PlayerController player)
    {
        player.SetNextBullet(atomicBullet);
    }


    private void PlayerSnowAtomic(PlayeContorollerSnow playerSnow)
    {
        playerSnow.SetNextBullet(atomicBullet);
    }

    private void PlayerLavaAtomic(PlayerControllerLava playerLava)
    {
        playerLava.SetNextBullet(atomicBullet);
    }


}