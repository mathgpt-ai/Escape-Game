using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : IInteractable
{
    public Transform objectToInspect;

    public float rotationSpeed = 100f;

    private Vector3 MousePos;

    [SerializeField] Canvas canvas;

    public void Interact()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 deltaMousePos = Input.mousePosition - MousePos;
            float rotationX = deltaMousePos.x * rotationSpeed * Time.deltaTime;
            float rotationY = deltaMousePos.y * rotationSpeed * Time.deltaTime;
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

            MousePos = Input.mousePosition;
        }
    }
    public Canvas GetCanvas()
    {
        return canvas;
    }

}
