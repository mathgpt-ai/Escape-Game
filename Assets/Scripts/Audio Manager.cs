using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Volume Settings")]
    [SerializeField] private float defaultMasterVolume = 1.0f;
    [SerializeField] private float defaultMusicVolume = 0.8f;

    // Reference to background music audio source (optional)
    [SerializeField] private AudioSource backgroundMusic;

    // PlayerPrefs keys to save settings
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    void Start()
    {
        // Load saved values or use defaults
        LoadSettings();

        // Set up slider event listeners
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void LoadSettings()
    {
        // Load saved settings from PlayerPrefs if they exist
        float masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, defaultMasterVolume);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);

        // Set UI sliders if they exist
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = masterVolume;

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = musicVolume;

        // Apply settings to the static properties in MazeGenerator
        MazeGenerator.TickSoundVolume = masterVolume;
        MazeGenerator.TimeUpSoundVolume = masterVolume;
        MazeGenerator.TenSecondsSoundVolume = masterVolume;

        // Apply to background music if it exists
        if (backgroundMusic != null)
            backgroundMusic.volume = musicVolume;
    }

    // Callback for master volume slider
    public void OnMasterVolumeChanged(float value)
    {
        // Apply master volume to all sound effects
        MazeGenerator.TickSoundVolume = value;
        MazeGenerator.TimeUpSoundVolume = value;
        MazeGenerator.TenSecondsSoundVolume = value;

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    // Callback for music volume slider
    public void OnMusicVolumeChanged(float value)
    {
        if (backgroundMusic != null)
            backgroundMusic.volume = value;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    // Public method to reset volumes to defaults
    public void ResetToDefaults()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = defaultMasterVolume;

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = defaultMusicVolume;

        // This will trigger the onValueChanged events which will update everything
    }
}
