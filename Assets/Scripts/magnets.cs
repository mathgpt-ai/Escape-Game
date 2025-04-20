using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnets : MonoBehaviour
{
    public float attractionForce = 1000f;
    public float maxRange = 10f; 
    public bool isPositiveMagnet = true;

    private Rigidbody rb;
    private bool playerIsPositive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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
    //void ApplyMagneticForces()
    //{
    //    ZeroGravityMovement player = FindObjectOfType<ZeroGravityMovement>();
    //    if (player == null) return;

    //    float charge = player.isPositiveCharge ? 1f : -1f;
    //    Vector3 direction = player.transform.position - transform.position;
    //    float distance = direction.magnitude;

    //    if (distance < maxRange && distance > 1f)
    //    {
    //        float forceMagnitude = (attractionForce * charge) / (distance * distance);
    //        Vector3 force = direction.normalized * forceMagnitude;

    //        // 🔄 Attraction si charges opposées, répulsion sinon
    //        if ((playerIsPositive && isPositiveMagnet) || (!playerIsPositive && !isPositiveMagnet))
    //        {
    //            force = -force; // 🔄 Inversion si charges identiques (repulsion)
    //        }

    //        Debug.Log($"Aimant {gameObject.name} applique {force.magnitude}N au joueur.");

    //        player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
    //    }
    //}
}
    //public float magnetConstant = 20f; // Ajuste l'intensité de la force
    //private bool isAttracting = true; // Mode attraction par défaut

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F)) // Change entre attraction et répulsion
    //    {
    //        isAttracting = !isAttracting;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Charge charged = other.GetComponent<Charge>();
    //    if (charged != null)
    //    {
    //        Rigidbody objRb = other.GetComponent<Rigidbody>();
    //        if (objRb != null)
    //        {
    //            float distance = Vector3.Distance(transform.position, charged.transform.position);
    //            float forceMagnitude = (magnetConstant * Mathf.Abs(charged.chargeAmount)) / (distance * distance + 0.1f); // Évite la division par zéro

    //            Vector3 direction = (charged.transform.position - transform.position).normalized;
    //            float force = 0f;

    //            // Logique attraction/répulsion correcte
    //            if (isAttracting)
    //            {
    //                if (charged.chargeAmount > 0) // Objet positif
    //                    force = -forceMagnitude; // Aimant négatif → Attire
    //                else // Objet négatif
    //                    force = forceMagnitude; // Aimant positif → Attire
    //            }
    //            else
    //            {
    //                if (charged.chargeAmount > 0) // Objet positif
    //                    force = forceMagnitude; // Aimant positif → Repousse
    //                else // Objet négatif
    //                    force = -forceMagnitude; // Aimant négatif → Repousse
    //            }

    //            objRb.AddForce(direction * force);
    //        }
    //    }
    //}
    //[SerializeField] 
    //private float magnetIntensity = 20f;

    //private bool isAttracting = true;

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.F))
    //    {
    //        isAttracting = !isAttracting;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    Charge charge = other.GetComponent<Charge>();
    //    if(charge != null)
    //    {
    //        Rigidbody rb = other.GetComponent<Rigidbody>();
    //        if(rb != null)
    //        {
    //            float distance = Vector3.Distance(transform.position, charge.transform.position);
    //            float forceMagnet = (magnetIntensity * Mathf.Abs(charge.chargeObjet)) / (distance * distance + 0.1f);

    //            Vector3 dir = (charge.transform.position - transform.position).normalized;
    //            float force = 0f;

    //            if(isAttracting)
    //            {
    //                if (charge.chargeObjet > 0)
    //                    force = -forceMagnet;
    //                else
    //                    force = -forceMagnet;
    //            }
    //            else
    //            {
    //                if(charge.chargeObjet > 0)
    //                    force = -forceMagnet;
    //                else
    //                    force = -forceMagnet;
    //            }

    //            rb.AddForce(dir * force);
    //        }
    //    }
    //}

