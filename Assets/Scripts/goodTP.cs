using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMainMap : MonoBehaviour
{
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = true;

            // 🔁 Téléporte le joueur
            other.transform.position = teleportDestination.position;

            // 🔄 Gère les scripts
            zeroGrav zeroGrav = other.GetComponent<zeroGrav>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
        
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroGrav != null)
            {
                zeroGrav.enabled = false;
                Debug.Log("ZeroGravityMovement désactivé.");
            }
            if (zeroGravMvt != null)
            {
                zeroGravMvt.enabled = false;
                Debug.Log("ZeroGravityMovement désactivé.");
            }

            if (normalMove != null)
            {
                normalMove.enabled = true;
                Debug.Log("NormalMovement activé.");
            }

            Debug.Log("Téléportation réussie !");
        }
    }
}

