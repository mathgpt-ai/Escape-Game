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

            other.transform.position = teleportDestination.position;

            zeroGrav zeroGrav = other.GetComponent<zeroGrav>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroGrav != null)
            {
                zeroGrav.enabled = false;
            }
            if (zeroGravMvt != null)
            {
                zeroGravMvt.enabled = false;
            }

            if (normalMove != null)
            {
                normalMove.enabled = true;
            }
        }
    }
}

