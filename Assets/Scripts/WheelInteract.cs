using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float rotationSpeed = 90f; // Vitesse de rotation en degrés par seconde
    [SerializeField]
    private Canvas canvas;
    private bool isInteracting = false; // Suivi de l'état d'interaction=======
    private float currentRotation;
    private Quaternion startRotation;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        startRotation = transform.localRotation;
    }
    private void Update()
    {
        if (isInteracting)
        {
            if (isInteracting)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    float delta = rotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, delta);
                    currentRotation += delta;
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    float delta = -rotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, delta);
                    currentRotation += delta;
                }

                currentRotation = Mathf.Repeat(currentRotation, 360f); // Pour rester entre 0 et 360
            }
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
        percent = Mathf.Clamp(percent, 0f, 100f); // Securité
        float angle = percent / 100f * 360f;
        currentRotation = angle;
    }

    public float GetCurrentRotation()
    {
        return currentRotation;
    }

    
}
