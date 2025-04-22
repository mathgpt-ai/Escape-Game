using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour, IInteractable
{
//<<<<<<< HEAD
    [SerializeField]
    private float rotationSpeed = 90f; // Vitesse de rotation en degrés par seconde
    [SerializeField]
    private Canvas canvas;
    private bool isInteracting = false; // Suivi de l'état d'interaction=======
    private float currentRotation;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!isInteracting) return;

        float delta = 0f;

        if (Input.GetKey(KeyCode.R))
        {
            delta = rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.L))
        {
            delta = -rotationSpeed * Time.deltaTime;
        }

        if (delta != 0f)
        {
            transform.Rotate(Vector3.up, delta);
            currentRotation += delta;
            currentRotation = Mathf.Repeat(currentRotation, 360f);
        }
    }

    public void Interact()
    {
        // Alterner l'état d'interaction avec "E"
        isInteracting = !isInteracting;
        Debug.Log("Interaction avec la valve : " + (isInteracting ? "Activée" : "Désactivée"));
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }

    public void SetStartValue(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f); // Sécurité
        float targetAngle = percent / 100f * 360f;
        currentRotation = targetAngle;
        transform.localEulerAngles = new Vector3(0f, currentRotation, 0f);
    }

    public float GetCurrentRotation()
    {
        return currentRotation;
    }

    
}
