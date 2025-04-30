using System;
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

            zerograv zeroG = other.GetComponent<zerograv>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroG != null)
            {
                zeroG.enabled = false;
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
