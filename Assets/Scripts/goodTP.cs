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

            // Désactive scripts gravité zéro
            zerograv zeroG = other.GetComponent<zerograv>();
            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

            if (zeroG != null) zeroG.enabled = false;
            if (zeroGravMvt != null) zeroGravMvt.enabled = false;
            if (normalMove != null) normalMove.enabled = true;

            // 🔇 Désactive les lumières s'il y en a
            Transform posLight = other.transform.Find("PLight");
            Transform negLight = other.transform.Find("NLight");

            if (posLight != null && posLight.TryGetComponent(out Light pl)) pl.enabled = false;
            if (negLight != null && negLight.TryGetComponent(out Light nl)) nl.enabled = false;

            Debug.Log("Lumières désactivées après le portail.");
        }
    }
}
//public class TeleportToMainMap : MonoBehaviour
//{
//    public Transform teleportDestination;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Rigidbody rb = other.GetComponent<Rigidbody>();
//            rb.useGravity = true;

//            other.transform.position = teleportDestination.position;

//            zerograv zeroG = other.GetComponent<zerograv>();
//            ZeroGravityMovement zeroGravMvt = other.GetComponent<ZeroGravityMovement>();
//            FirstPersonController normalMove = other.GetComponent<FirstPersonController>();

//            if (zeroG != null)
//            {
//                zeroG.enabled = false;
//            }
//            if (zeroGravMvt != null)
//            {
//                zeroGravMvt.enabled = false;
//            }

//            if (normalMove != null)
//            {
//                normalMove.enabled = true;
//            }

//            Light positiveLight = other.GetComponentInChildren<Light>(true);
//            Light negativeLight = other.GetComponentInChildren<Light>(true);
//            if (positiveLight != null && positiveLight.name == "PLight") positiveLight.enabled = false;
//            if (negativeLight != null && negativeLight.name == "NLight") negativeLight.enabled = false;
//        }
//    }
//}
