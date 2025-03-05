using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour , IInteractable
{
    public float rotationSpeed = 5f;
    private bool isInteracting = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        if (isInteracting)
        {
            // Get mouse
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            // Calculate rotation
            float rotationAmount = (mouseDelta.x + mouseDelta.y) * rotationSpeed;

            // Apply rotation on the Z-axis
            transform.Rotate(Vector3.forward, rotationAmount);

            // Update last mouse position
            lastMousePosition = Input.mousePosition;
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
            lastMousePosition = Input.mousePosition;
        }
    }

    public string GetText()
    {
        return "Hold Left Click and Move Mouse to Rotate the Wheel";
    }
}
