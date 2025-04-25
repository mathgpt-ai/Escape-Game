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

            other.transform.position = teleportDestination.position;

            zeroGrav zeroGrav = other.GetComponent<zeroGrav>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroGrav != null)
            {
                zeroGrav.enabled = true;
            }
            if (zeroGravMvt != null)
            {
                zeroGravMvt.enabled = true;
            }
            if (normalMove != null)
            {
                normalMove.enabled = false;
            }
        }
    }
}
