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
    

    private bool hasBeenActivated = false;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Interact()
    {

        Console.WriteLine("ya");
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

        hasBeenActivated = true;

        // Destroy this object if needed
       
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // Small delay to allow for bridge instantiation
        yield return new WaitForSeconds(0.5f);

        // Load the scene additively (so it's ready to play)
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        Debug.Log($"Scene {sceneToLoad} loaded additively");
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}