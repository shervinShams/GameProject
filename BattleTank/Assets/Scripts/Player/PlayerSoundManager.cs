using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{

    public AudioClip shootAudio;
    public AudioClip rideAudio;
    public AudioClip deathAudio;
    public AudioClip hurtAudio;

    private AudioSource player;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        player.clip = shootAudio;
        player.Play();
    }

    public void PlayHurt()
    {
        player.clip = hurtAudio;
        player.Play();
    }

    public void PlayRide()
    {
        if (!player.isPlaying)
        {
            player.clip = rideAudio;
            player.Play();
        }
    }

    public void PlayDeath()
    {
        player.clip = deathAudio;
        player.Play();
    }
}
