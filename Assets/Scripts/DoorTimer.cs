using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timerDuration = 300f; // 5 minutes by default
    [SerializeField] private TextMeshProUGUI timerText; // Reference to UI text

    [Header("Player Respawn Settings")]
    [SerializeField] private Transform spawnPoint; // Spawn point to teleport player back to
    [SerializeField] private float respawnDelay = 2f; // Delay before respawning

    [Header("Audio Settings")]
    [SerializeField] private AudioClip timerStartSound;
    [SerializeField] private AudioClip timerEndSound;
    [SerializeField] private AudioClip timerWarningSound;
    [SerializeField] private AudioClip timerTickSound; // Ticking sound
    [SerializeField] private float tickInterval = 1f; // How often the tick sound plays
    [SerializeField] private float audioVolume = 1f;

    [Header("Visual Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private Color criticalColor = new Color(1f, 0f, 0f);
    [SerializeField] private float warningThreshold = 60f; // 1 minute warning
    [SerializeField] private float criticalThreshold = 30f; // 30 seconds critical

    private bool timerActive = false;
    private float currentTime;
    private bool hasTimerStarted = false;
    private float nextTickTime = 0f; // For tracking when to play the next tick
    private GameObject player;

    void Start()
    {
        // Initialize timer value but don't start counting
        currentTime = timerDuration;

        // Set up timer UI if available
        if (timerText != null)
        {
            UpdateTimerDisplay();
            timerText.gameObject.SetActive(false);
        }

        // Find the player (assuming it has the "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player");

        // Check if spawn point is assigned
        if (spawnPoint == null)
        {
            Debug.LogWarning("No spawn point assigned to DoorTimer! Player won't respawn correctly.");
            // Try to find a spawn point in the scene
            var possibleSpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (possibleSpawnPoint != null)
            {
                spawnPoint = possibleSpawnPoint.transform;
                Debug.Log("Found spawn point in scene: " + spawnPoint.name);
            }
        }
    }

    void Update()
    {
        // Only run timer logic if it's active
        if (timerActive)
        {
            // Decrease time
            currentTime -= Time.deltaTime;

            // Update display
            UpdateTimerDisplay();

            // Play tick sound at regular intervals
            if (timerTickSound != null && currentTime > 0 && Time.time >= nextTickTime)
            {
                PlaySound(timerTickSound);
                nextTickTime = Time.time + tickInterval;
            }

            // Check for warning threshold
            if (currentTime <= warningThreshold && currentTime > criticalThreshold)
            {
                timerText.color = warningColor;
            }
            // Check for critical threshold
            else if (currentTime <= criticalThreshold && currentTime > 0)
            {
                timerText.color = criticalColor;

                // Play warning sound when first entering critical threshold
                if (Math.Abs(currentTime - criticalThreshold) < 0.1f)
                {
                    PlaySound(timerWarningSound);
                }

                // Optional: Make text flash when critical
                if (Mathf.Floor(currentTime) % 2 == 0)
                {
                    timerText.alpha = 0.5f;
                }
                else
                {
                    timerText.alpha = 1f;
                }
            }

            // Check if timer has ended
            if (currentTime <= 0)
            {
                TimerEnded();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Find the LockedDoor component to check if it's unlocked
        LockedDoor lockedDoor = GetComponent<LockedDoor>();

        // Only start timer if:
        // 1. It's the player
        // 2. Timer hasn't already started
        // 3. Door is still locked (if the LockedDoor component exists)
        if (other.CompareTag("Player") && !hasTimerStarted &&
            (lockedDoor == null || lockedDoor.isLock))
        {
            StartTimer();
        }
    }

    void StartTimer()
    {
        // Mark timer as started to prevent multiple starts
        hasTimerStarted = true;
        timerActive = true;

        // Initialize tick timer
        nextTickTime = Time.time + tickInterval;

        // Show timer UI
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.color = normalColor;
            timerText.alpha = 1f;
        }

        // Play start sound if available
        if (timerStartSound != null)
        {
            PlaySound(timerStartSound);
        }

        Debug.Log("Maze Timer Started - " + timerDuration + " seconds remaining!");
    }

    void TimerEnded()
    {
        // Stop the timer
        timerActive = false;
        currentTime = 0;

        // Play end sound if available
        if (timerEndSound != null)
        {
            PlaySound(timerEndSound);
        }

        // Start respawn process
        StartCoroutine(RespawnPlayerAndResetMaze());

        Debug.Log("Timer Ended! Respawning player...");
    }

    IEnumerator RespawnPlayerAndResetMaze()
    {
        // Disable player movement (optional - if you have a player controller component)
        DisablePlayerMovement();

        // Optional: fade screen, show message, etc.

        // Wait for delay
        yield return new WaitForSeconds(respawnDelay);

        // Respawn player at spawn point
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }

        // Find and regenerate the maze
        RegenerateMaze();

        // Hide timer text
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        // Reset timer
        ResetTimer();

        // Re-enable player movement
        EnablePlayerMovement();
    }

    // Find and regenerate the maze
    private void RegenerateMaze()
    {
        // Find the MazeGenerator
        MazeGenerator mazeGenerator = FindObjectOfType<MazeGenerator>();

        if (mazeGenerator != null)
        {
            // Call the RegenerateMaze method on the MazeGenerator
            mazeGenerator.RegenerateMaze();
            Debug.Log("Maze regeneration triggered!");
        }
        else
        {
            Debug.LogError("No MazeGenerator found in the scene!");
        }
    }

    // Disable player movement - adjust based on your player controller
    private void DisablePlayerMovement()
    {
        if (player != null)
        {
            // Option 1: If using a Character Controller
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            // Option 2: If using a custom player controller
            MonoBehaviour[] components = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour comp in components)
            {
                if (comp.GetType().Name.Contains("Controller") ||
                    comp.GetType().Name.Contains("Movement"))
                {
                    comp.enabled = false;
                }
            }

            // Option 3: If using Rigidbody physics
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    // Enable player movement - adjust based on your player controller
    private void EnablePlayerMovement()
    {
        if (player != null)
        {
            // Option 1: If using a Character Controller
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = true;
            }

            // Option 2: If using a custom player controller
            MonoBehaviour[] components = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour comp in components)
            {
                if (comp.GetType().Name.Contains("Controller") ||
                    comp.GetType().Name.Contains("Movement"))
                {
                    comp.enabled = true;
                }
            }

            // Option 3: If using Rigidbody physics
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Format as MM:SS
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Public methods for external control

    public void PauseTimer()
    {
        timerActive = false;
    }

    public void ResumeTimer()
    {
        timerActive = true;
    }

    public void ResetTimer()
    {
        currentTime = timerDuration;
        timerActive = false;
        hasTimerStarted = false;

        // Reset visual aspects
        if (timerText != null)
        {
            timerText.color = normalColor;
            timerText.alpha = 1f;
            timerText.gameObject.SetActive(false);
        }

        UpdateTimerDisplay();
    }

    public void StopAndDestroyTimer()
    {
        // Stop the timer
        timerActive = false;

        // Hide timer text
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        // Reset any visual effects
        if (timerText != null)
        {
            timerText.color = normalColor;
            timerText.alpha = 1f;
        }

        // Destroy the timer component itself
        Destroy(this);
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }

    public void AddTime(float secondsToAdd)
    {
        currentTime += secondsToAdd;
    }

    public void RemoveTime(float secondsToRemove)
    {
        currentTime -= secondsToRemove;
        if (currentTime < 0) currentTime = 0;

        // Optional: Play a warning sound for time penalty
        if (timerWarningSound != null)
        {
            PlaySound(timerWarningSound);
        }
    }

    // Helper method to play sounds with volume adjustment
    private void PlaySound(AudioClip clip)
    {
        float volume = AudioManager.Instance != null ?
            AudioManager.GetAdjustedVolume(audioVolume) : audioVolume;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}