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
            Debug.Log($"Aimant {gameObject.name} bloque");
            return;
        }

        bool isAttracting = (playerCharge.isPositiveCharge != isPositive);
        float forceDirection = isAttracting ? 1f : -1f;
        float forceMagnitude = chargeStrength / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude * forceDirection;

        playerRb.AddForce(force, ForceMode.Acceleration);

        Debug.Log($" {gameObject.name} applique {force.magnitude}N au joueur");
    }
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }
}
//public class MagnetsObjects : MonoBehaviour
//{
//    public float chargeStrength = 5f; // Intensité de la charge
//    public bool isPositive = true; // Charge de l'objet (true = +, false = -)


//    private void FixedUpdate()
//    {
//        ApplyMagneticForces();
//    }

//    void ApplyMagneticForces()
//    {
//        GameObject player = GameObject.FindGameObjectWithTag("Player");
//        if (player == null) return;

//        Rigidbody playerRb = player.GetComponent<Rigidbody>();
//        ZeroGravityMovement playerCharge = player.GetComponent<ZeroGravityMovement>();

//        if (playerRb == null || playerCharge == null) return;

//        Vector3 direction = player.transform.position - transform.position;
//        float distance = direction.magnitude;


//        int wallLayerMask = LayerMask.GetMask("wall");

//        // ✅ Vérifie si un mur bloque la ligne de vue
//        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, wallLayerMask))
//        {
//            Debug.Log($"⚠️ Aimant bloqué par {hit.collider.gameObject.name} !");
//            return; // ❗ STOP ici, ne pas appliquer la force
//        }


//        // ✅ Si on arrive ici, c'est qu'il n'y a PAS de mur entre le joueur et l'aimant
//        bool isAttracting = (playerCharge.isPositiveCharge != isPositive);
//        float forceDirection = isAttracting ? 1f : -1f;

//        float forceMagnitude = chargeStrength / Mathf.Pow(distance, 2);
//        Vector3 force = direction.normalized * forceMagnitude * forceDirection;
//        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit2, distance, wallLayerMask))
//        {
//            Debug.Log($"🚧 Aimant bloqué par {hit2.collider.gameObject.name} !");
//             playerRb.velocity = Vector3.zero;
//        playerRb.angularVelocity = Vector3.zero;

//            return;
//        }

//        playerRb.AddForce(force, ForceMode.Acceleration);

//        Debug.Log($"🧲 Objet {gameObject.name} applique {force.magnitude}N au joueur | Attraction: {isAttracting}");
//    }
//}
