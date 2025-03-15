using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour , IInteractable
{
    public float rotationSpeed = 5f;
    private bool isInteracting = false;

    private void Update()
    {
        if (isInteracting)
        {
            // Get mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Combine horizontal and vertical movement into a single rotation value
            float rotationAmount = (mouseX + mouseY) * rotationSpeed;

            // Apply rotation ONLY on the Z-axis
            transform.Rotate(Vector3.forward, rotationAmount);
        }

        // Stop interaction when left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
        }



    }

    public void Interact()
    {
        // Start interaction only when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            isInteracting = !isInteracting;
        }
    }

    public string GetText()
    {
        return "Hold Left Click and Move Mouse to Rotate the Wheel";
    }
}
