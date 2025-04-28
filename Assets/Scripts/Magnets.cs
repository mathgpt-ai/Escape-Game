using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnets : MonoBehaviour
{
    public float attractionForce = 1000f;
    public float maxRange = 10f;
    public bool isPositiveMagnet = true;

    private Rigidbody rb;
    private bool playerIsPositive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        ApplyMagneticForces();
    }

    public void UpdatePlayerCharge(bool isPositive)
    {
        playerIsPositive = isPositive;
    }

    void ApplyMagneticForces()
    {
        ZeroGravityMovement player = FindObjectOfType<ZeroGravityMovement>();
        if (player == null) return;

        float charge = player.isPositiveCharge ? 1f : -1f;
        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        int wallLayerMask = LayerMask.GetMask("wall");
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, wallLayerMask))
        {
            Debug.Log($"Aimant {gameObject.name} bloqué par {hit.collider.gameObject.name}");
            return;
        }

        if (distance < maxRange && distance > 1f)
        {
            float forceMagnitude = (attractionForce * charge) / (distance * distance);
            Vector3 force = direction.normalized * forceMagnitude;


            if ((playerIsPositive && isPositiveMagnet) || (!playerIsPositive && !isPositiveMagnet))
            {
                force = -force;
            }

            Debug.Log($" Aimant {gameObject.name} applique {force.magnitude}N au joueur.");

            player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        ZeroGravityMovement player = other.GetComponent<ZeroGravityMovement>();
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < 1.2f)
            {
                player.freezeCameraRotation = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ZeroGravityMovement player = other.GetComponent<ZeroGravityMovement>();
        if (player != null)
        {
            player.freezeCameraRotation = false;
        }

    }
}
