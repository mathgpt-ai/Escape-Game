using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshProUGUI

public class LockedDoor : MonoBehaviour
{
    private Canvas canvas;
    public bool isLock = true;
    private Animator Animator;
    private int dragonsRemaining = 3; // Total number of dragons
    [SerializeField] private AudioClip doorUnlocked;
    [SerializeField] private float doorUnlockedVolume = 5f;

    void Start()
    {
        Animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        if (!isLock)
            canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLock)
            Animator.SetTrigger("open");
    }

    // Called when a dragon is defeated
    public void DragonDefeated()
    {
        dragonsRemaining--;

        if (dragonsRemaining <= 0)
        {
            UnlockDoor();
        }
    }

    // Unlocks the door
    private void UnlockDoor()
    {
        isLock = false;
        AudioSource.PlayClipAtPoint(doorUnlocked, Camera.main.transform.position, doorUnlockedVolume);

        // Enable the minimap
        if (MiniMap.Instance != null)
        {
            MiniMap.Instance.EnableMiniMap();
        }

        // Stop and destroy any active timer
        StopAllTimers();

        // Find and destroy any timer text in the scene
        DestroyTimerText();
    }

    // Find and destroy all TextMeshProUGUI components that might be timer texts
    private void DestroyTimerText()
    {
        // Look for all TextMeshProUGUI components in the scene
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI text in allTexts)
        {
            // Check if this text component might be a timer text
            // We can look for typical timer formats like "00:00" or check for specific names
            if (text.name.Contains("Timer") || text.name.Contains("Clock") ||
                IsTimerFormatText(text.text))
            {
                // Make it invisible
                text.gameObject.SetActive(false);

                // Optional: Actually destroy the GameObject containing the text
                // Destroy(text.gameObject);
            }
        }
    }

    // Helper method to check if a text appears to be in timer format (MM:SS)
    private bool IsTimerFormatText(string text)
    {
        // Simple check for timer format (MM:SS)
        if (text.Length == 5 && text[2] == ':')
        {
            // Check if the text consists of numbers separated by a colon
            bool isTimer = true;
            for (int i = 0; i < 5; i++)
            {
                if (i == 2) continue; // Skip the colon

                if (!char.IsDigit(text[i]))
                {
                    isTimer = false;
                    break;
                }
            }
            return isTimer;
        }
        return false;
    }

    // Find and stop all DoorTimer components in the scene
    private void StopAllTimers()
    {
        DoorTimer[] timers = FindObjectsOfType<DoorTimer>();
        foreach (DoorTimer timer in timers)
        {
            if (timer != null)
            {
                // Call the method to stop and destroy the timer
                timer.StopAndDestroyTimer();
            }
        }
    }
}