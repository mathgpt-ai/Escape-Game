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
    [SerializeField] private float baseVolume = 2f;
    [SerializeField] private Sprite planetSprite; // Reference to the planet sprite to check for
    [SerializeField] private string defaultCanvasText = "Press E to Interact"; // Default text to show

    private bool firstuse = true;
    private bool hasBeenActivated = false;
    private Canvas canvas;
    private TMPro.TextMeshProUGUI canvasText;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvasText = canvas?.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        // Store the original text if it exists
        if (canvasText != null && string.IsNullOrEmpty(defaultCanvasText))
        {
            defaultCanvasText = canvasText.text;
        }
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
                ActivatePower();
                // Perform actions when player has the planet
                // For example: unlock something, play a special sound, show a message

                // You could also remove the item if needed
                // playerInventory.RemoveItem();
            }
            else
            {
                Debug.Log("Player doesn't have the planet in their inventory yet!");
                // Show a message to the player that they need the planet
                if (canvasText != null)
                {
                    canvasText.text = "You need to find the planet first!";

                    // Reset text after 20 seconds
                    StartCoroutine(ResetCanvasTextAfterDelay(20f));
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

    // New coroutine to reset canvas text after a delay
    private IEnumerator ResetCanvasTextAfterDelay(float delayInSeconds)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Reset the text to the default value
        if (canvasText != null)
        {
            canvasText.text = defaultCanvasText;
            Debug.Log("Reset canvas text to default after " + delayInSeconds + " seconds");
        }
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


    public void ActivatePower()
    {
        // Find all inactive LabRoom objects in the current scene
        GameObject[] labRooms = FindInactiveObjectsWithTag("LabRoom");

        foreach (GameObject labRoom in labRooms)
        {
            // Activate the objects
            labRoom.SetActive(true);
            Debug.Log("Activated LabRoom object: " + labRoom.name);
        }
    }

    // Method to find both active and inactive objects with a specific tag
    private GameObject[] FindInactiveObjectsWithTag(string tag)
    {
        List<GameObject> results = new List<GameObject>();

        // Get all root GameObjects in the active scene
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // Search through all objects including inactive ones
        foreach (GameObject rootObject in rootObjects)
        {
            // Check if this root object has the tag
            if (rootObject.CompareTag(tag))
            {
                results.Add(rootObject);
            }

            // Get all children (including inactive ones)
            Transform[] childrenTransforms = rootObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform childTransform in childrenTransforms)
            {
                // Skip the root object which we already checked
                if (childTransform == rootObject.transform)
                    continue;

                // Check if this child object has the tag
                if (childTransform.CompareTag(tag))
                {
                    results.Add(childTransform.gameObject);
                }
            }
        }

        return results.ToArray();
    }
}