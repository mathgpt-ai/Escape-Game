using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidKill : MonoBehaviour
{
    public Transform spawnPoint;
    private FirstPersonController player;

    void Start()
    {
        player = FindObjectOfType<FirstPersonController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.transform.position = spawnPoint.position;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero; // Stopper la chute
        }
    }
}
