using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public AudioMixer myMixer;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider sfxSlider;

    private static bool instanceExists;

    private void Awake()
    {
        /*if (instanceExists)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instanceExists = true;
        }*/
    }

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        SetMusicVolume();
        SetSFXVolume();

        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume1 = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume1) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume1);
    }
}
