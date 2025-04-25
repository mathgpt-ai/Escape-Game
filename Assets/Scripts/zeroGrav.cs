using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zeroGrav : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }
}
