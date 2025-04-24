using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeleportToPuzzle : MonoBehaviour
{
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.useGravity = false;

            // Téléporte le joueur
            other.transform.position = teleportDestination.position;

            // Gère les scripts
            zeroGrav zeroGrav = other.GetComponent<zeroGrav>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroGrav != null)
            {
                zeroGrav.enabled = true;
                Debug.Log("zeroGrav activé.");
            }
            if (zeroGravMvt != null)
            {
                zeroGravMvt.enabled = true;
                Debug.Log("ZeroGravityMovement activé.");
            }

            if (normalMove != null)
            {
                normalMove.enabled = false;
                Debug.Log("NormalMovement désactivé.");
            }

            Debug.Log("Téléportation vers énigme réussie !");
        }
    }
}
