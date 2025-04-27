using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vents : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 1000, 0);
    void Update()
    {
        gameObject.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
