using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool isHolding = false;
    [SerializeField] private float throwForce = 600f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float floatSpeed = 5f;  
    [SerializeField] private float floatHeight = 0.2f;
    private float distance;

    private TempParent tempParent;
    private Rigidbody rb;
    private Vector3 initialOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    void Update()
    {
        if (isHolding)
            Hold();
    }

    private void OnMouseDown()
    {
        if (tempParent != null)
        {
            distance = Vector3.Distance(transform.position, tempParent.transform.position);
            if (distance <= maxDistance)
            {
                isHolding = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                initialOffset = tempParent.transform.forward * 1.5f;
            }
        }
    }

    private void OnMouseUp()
    {
        Drop();
    }

    private void Hold()
    {
        distance = Vector3.Distance(transform.position, tempParent.transform.position);
        if (distance >= maxDistance)
        {
            Drop();
            return;
        }

        
        Vector3 targetPosition = tempParent.transform.position + initialOffset;
        targetPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
        transform.rotation = Quaternion.Lerp(transform.rotation, tempParent.transform.rotation, Time.deltaTime * 5f);

        
        if (Input.GetMouseButtonDown(1))
        {
            rb.AddForce(tempParent.transform.forward * throwForce, ForceMode.Impulse);
            Drop();
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            transform.SetParent(null);
            rb.useGravity = true;
        }
    }
}

