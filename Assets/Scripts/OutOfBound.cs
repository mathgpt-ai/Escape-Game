using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBound : MonoBehaviour
{
    [SerializeField] GameObject transform;
    private void OnCollisionEnter(Collision collision)
    {
        

        if (collision.transform.tag == "Player")
        {
            collision.transform.position = transform.transform.position;
        }
    }
}
