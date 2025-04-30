using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objFlo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 0.5f;
        rb.AddForce(Random.insideUnitSphere * 50f, ForceMode.Impulse);
    }

}
