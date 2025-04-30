using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport3forme : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private float teleportDelay = 0.5f;

    [Header("Scene Management")]
    [SerializeField] private string sceneToDeactivate;
    [SerializeField] private float sceneDeactivationDelay = 1f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private float teleportSoundVolume = 1f;

    [Header("Additional Settings")]
    [SerializeField] private bool resetVelocity = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool isTeleporting = false;
    private bool hasBeenTriggered = false;

    private void Start()
    {
        if (teleportDestination == null)
        {
            Debug.LogWarning("No teleport destination set for " + gameObject.name);
        }

        if (string.IsNullOrEmpty(sceneToDeactivate))
        {
            Debug.LogWarning("No scene to deactivate specified for " + gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleTrigger(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other.gameObject);
    }

    private void HandleTrigger(GameObject collidingObject)
    {
        // Check if this is the player and we're not already teleporting
        if (!isTeleporting && collidingObject.CompareTag(playerTag))
        {
            // If set to trigger only once and has already been triggered, don't do anything
            if (triggerOnlyOnce && hasBeenTriggered)
                return;

            StartCoroutine(TeleportPlayerAndDeactivateScene(collidingObject));

            if (triggerOnlyOnce)
                hasBeenTriggered = true;
        }
    }

    private IEnumerator TeleportPlayerAndDeactivateScene(GameObject player)
    {
        isTeleporting = true;

        // Play teleport sound if available
        if (teleportSound != null)
        {
            // Check if AudioManager exists for volume adjustment
            float volume = teleportSoundVolume;
            if (AudioManager.Instance != null)
            {
                volume = AudioManager.GetAdjustedVolume(teleportSoundVolume);
            }

            AudioSource.PlayClipAtPoint(teleportSound, transform.position, volume);
        }

        // Wait for the specified delay
        yield return new WaitForSeconds(teleportDelay);

        if (player != null && teleportDestination != null)
        {
            // Reset velocity if specified
            if (resetVelocity)
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            // Teleport player
            player.transform.position = teleportDestination.position;
            player.transform.rotation = teleportDestination.rotation;

            Debug.Log("Player teleported to " + teleportDestination.name);
        }
        else
        {
            Debug.LogError("Player or destination is null!");
        }

        // Wait for the specified scene deactivation delay
        yield return new WaitForSeconds(sceneDeactivationDelay);

        // Unload the specified scene if it exists and is loaded
        if (!string.IsNullOrEmpty(sceneToDeactivate))
        {
            Scene scene = SceneManager.GetSceneByName(sceneToDeactivate);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneToDeactivate);
                Debug.Log("Unloaded scene: " + sceneToDeactivate);
            }
            else
            {
                Debug.LogWarning("Scene '" + sceneToDeactivate + "' is not currently loaded.");
            }
        }

        isTeleporting = false;
    }
}