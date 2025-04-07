using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For accessing the timer text

public class LockedDoor : MonoBehaviour
{
    private Canvas canvas;
    public bool isLock = true;
    private Animator Animator;
    private int dragonsRemaining = 3; // 🔹 Nombre total de dragons
    [SerializeField] private AudioClip doorUnlocked;
    [SerializeField] private Text timerText; // Reference to the timer text (optional)

    void Start()
    {
        Animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();

        // If timer text wasn't assigned, try to find it
        if (timerText == null)
        {
            // Try to find the timer text in the scene - assumes it's the same as in MazeGenerator
            if (MazeGenerator.Instance != null)
            {
                // Use reflection to get the private timerText field from MazeGenerator
                var field = typeof(MazeGenerator).GetField("timerText",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (field != null)
                {
                    timerText = field.GetValue(MazeGenerator.Instance) as Text;
                }
            }
        }
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

    // 🔹 Appelé quand un dragon meurt
    public void DragonDefeated()
    {
        dragonsRemaining--;

        if (dragonsRemaining <= 0)
        {
            UnlockDoor();
        }
    }

    // 🔹 Déverrouille la porte
    private void UnlockDoor()
    {
        isLock = false;
        AudioSource.PlayClipAtPoint(doorUnlocked, transform.position, 40f);

        // Enable the minimap
        MiniMap.Instance.EnableMiniMap();

        // Stop the timer when the door is unlocked
        if (MazeGenerator.Instance != null)
        {
            MazeGenerator.Instance.StopTimer();

            
        }

        // Hide the timer text
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }
}