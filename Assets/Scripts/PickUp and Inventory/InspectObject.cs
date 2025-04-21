using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public int moveSpeed = 10; 

    public void Inspect(Transform FrontholdPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, FrontholdPoint.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, FrontholdPoint.rotation, moveSpeed * 100f * Time.deltaTime);

    }
}
