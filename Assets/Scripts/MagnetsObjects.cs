using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetsObjects : MonoBehaviour
{
    public float chargeStrength = 5f;
    public bool isPositive = true;

    private void FixedUpdate()
    {
        ApplyMagneticForces();
    }

    void ApplyMagneticForces()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        ZeroGravityMovement playerCharge = player.GetComponent<ZeroGravityMovement>();

        if (playerRb == null || playerCharge == null) return;

        if (MagnetSelector.selectedMagnet != null && MagnetSelector.selectedMagnet != this) return;

        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        int wallLayerMask = LayerMask.GetMask("wall");

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, wallLayerMask))
        {
            
            return;
        }

        bool isAttracting = (playerCharge.isPositiveCharge != isPositive);
        float forceDirection = isAttracting ? 1f : -1f;
        float forceMagnitude = chargeStrength / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude * forceDirection;

        playerRb.AddForce(force, ForceMode.Acceleration);

       
    }
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

}
