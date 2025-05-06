using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialiserEnigmeLab : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject bridgePrefab;
    [SerializeField] private Transform bridgeSpawnPoint;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private float baseVolume = 1f;
    [SerializeField] private Sprite planetSprite; // Reference to the planet sprite to check for

    private bool firstuse = true;
    private bool hasBeenActivated = false;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Interact()
    {
        // Play sound if available, using the AudioManager for volume adjustment
        if (interactionSound != null)
        {
            // Get the adjusted volume based on master volume setting
            float adjustedVolume = baseVolume;
            if (AudioManager.Instance != null)
            {
                adjustedVolume = AudioManager.GetAdjustedVolume(baseVolume);
            }
            // Play the sound at the adjusted volume
            AudioSource.PlayClipAtPoint(interactionSound, transform.position, adjustedVolume);
        }

        if (firstuse)
        {
            // Create bridge
            if (bridgePrefab != null && bridgeSpawnPoint != null)
            {
                Instantiate(bridgePrefab, bridgeSpawnPoint.position, bridgeSpawnPoint.rotation);
                Debug.Log("Bridge created at specified location");
            }
            else
            {
                Debug.LogWarning("Bridge prefab or spawn point not assigned!");
            }

            // Load scene
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                StartCoroutine(LoadSceneAfterDelay());
            }

            firstuse = false;

            // Start the coroutine to delete objects after 20 seconds
            StartCoroutine(DeleteObjectsAfterDelay(20f));
        }
        else
        {
            // Check if the player's inventory has the planet
            Inventory playerInventory = FindInventory();

            if (playerInventory != null && playerInventory.HasItem(planetSprite))
            {
                Debug.Log("Player has the planet in their inventory!");
                // Perform actions when player has the planet
                // For example: unlock something, play a special sound, show a message

                // You could also remove the item if needed
                // playerInventory.RemoveItem();
            }
            else
            {
                Debug.Log("Player doesn't have the planet in their inventory yet!");
                // Maybe show a hint or a message to the player
                if (canvas != null)
                {
                    // You could set a Text component in the canvas to display a message
                    TMPro.TextMeshProUGUI text = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = "You need to find the planet first!";
                    }
                }
            }
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // Small delay to allow for bridge instantiation
        yield return new WaitForSeconds(0.5f);
        // Load the scene additively (so it's ready to play)
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        Debug.Log($"Scene {sceneToLoad} loaded additively");
        // Short delay to make sure scene is fully loaded
        yield return new WaitForSeconds(0.1f);
    }

    // New coroutine to delete objects after a delay
    private IEnumerator DeleteObjectsAfterDelay(float delayInSeconds)
    {
        Debug.Log($"Will delete objects with 'delete' tag after {delayInSeconds} seconds");

        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Find and destroy all objects with the "delete" tag
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("delete");
        int count = objectsToDelete.Length;

        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }

        Debug.Log($"Deleted {count} objects with 'delete' tag after {delayInSeconds} seconds");
    }

    // Helper method to find the player's inventory
    private Inventory FindInventory()
    {
        // Try to find the hotbar first since that's where the inventory is according to your code
        GameObject hotbar = GameObject.Find("Hotbar");
        if (hotbar != null)
        {
            Inventory inventory = hotbar.GetComponent<Inventory>();
            if (inventory != null)
            {
                return inventory;
            }
        }

        // If not found in the hotbar, try a general search
        return FindObjectOfType<Inventory>();
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}