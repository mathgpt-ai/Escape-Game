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

    // Static property for other scripts to access
    public static float MasterVolume { get; private set; }

    // PlayerPrefs keys to save settings
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    // Singleton pattern for easy access
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Set up singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Load saved values or use defaults
        LoadSettings();

        // Set up slider event listeners
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // Apply volume to any existing DoorTimer objects
        UpdateAllDoorTimerVolumes();
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

        // Save to static property
        MasterVolume = masterVolume;

        // Apply to background music if it exists
        if (backgroundMusic != null)
            backgroundMusic.volume = musicVolume;
    }

    // Callback for master volume slider
    public void OnMasterVolumeChanged(float value)
    {
        // Store the value for other scripts to access
        MasterVolume = value;

        // Apply master volume to all DoorTimer objects
        UpdateAllDoorTimerVolumes();

        // Save to PlayerPrefs
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

    // Update all DoorTimer volumes using reflection
    private void UpdateAllDoorTimerVolumes()
    {
        DoorTimer[] doorTimers = FindObjectsOfType<DoorTimer>();

        foreach (DoorTimer timer in doorTimers)
        {
            // Use reflection to access the audioVolume field
            var volumeField = timer.GetType().GetField("audioVolume",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);

            if (volumeField != null)
            {
                volumeField.SetValue(timer, MasterVolume);
            }
        }
    }

    // Public method for other scripts to adjust their volume based on master volume
    public static float GetAdjustedVolume(float originalVolume)
    {
        return originalVolume * MasterVolume;
    }
}