using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private float volume = 1f;

    private void OnCollisionEnter(Collision collision)
    {
        // Teleport the colliding object to the spawn point
        collision.gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;

        // Play the teleport sound if one is assigned
        if (teleportSound != null)
        {
            // Play the sound at the player's new position
            AudioSource.PlayClipAtPoint(teleportSound, collision.gameObject.transform.position, volume);
        }
    }
}