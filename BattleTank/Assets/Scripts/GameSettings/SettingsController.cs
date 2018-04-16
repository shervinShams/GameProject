using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    public AudioMixer masterMixer;

    public Slider efxSlider;
    public Slider musicSlider;

    private string efxVol = "EfxVol";
    private string musicVol = "MusicVol";

    void Start()
    {
        float efx = PlayerPrefs.GetFloat(efxVol);
        float music = PlayerPrefs.GetFloat(musicVol);
        masterMixer.SetFloat(efxVol, efx);
        masterMixer.SetFloat(musicVol, music);
        efxSlider.value = efx;
        musicSlider.value = music;
    }

    public void SetEfxVolume(float vol)
    {
        masterMixer.SetFloat(efxVol, vol);
        PlayerPrefs.SetFloat(efxVol, vol);
    }

    public void SetMusicVolume(float vol)
    {
        masterMixer.SetFloat(musicVol, vol);
        PlayerPrefs.SetFloat(musicVol, vol);
    }

}
