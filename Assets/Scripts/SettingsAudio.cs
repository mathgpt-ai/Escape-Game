using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    //[SerializeField] private AudioSource musicSource;
    //[SerializeField] private AudioSource sfxSource;



    private void Start()
    {
        // Récupère la valeur des volumes
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyVolumes();

        // Ajoute les listeners
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }



    private void SetMusicVolume(float volume)
    {
       // musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }


    private void SetSFXVolume(float volume)
    {
        //sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


    private void ApplyVolumes()
    {
        //musicSource.volume = musicSlider.value;
        //sfxSource.volume = sfxSlider.value;
    }
}
