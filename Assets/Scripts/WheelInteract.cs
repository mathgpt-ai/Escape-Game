using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour , IInteractable
{
    public float rotationSpeed = 90f; // Vitesse de rotation en degrés par seconde
    private bool isInteracting = false; // Suivi de l'état d'interaction

    private void Update()
    {
        if (isInteracting)
        {
            // Tourner vers la droite avec R
            if (Input.GetKey(KeyCode.R))
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            // Tourner vers la gauche avec L
            else if (Input.GetKey(KeyCode.L))
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void Interact()
    {
        // Alterner l'état d'interaction avec "E"
        isInteracting = !isInteracting;
        Debug.Log("Interaction avec la valve : " + (isInteracting ? "Activée" : "Désactivée"));
    }

    public string GetText()
    {
        return isInteracting ? "Appuyez sur L ou R pour tourner, E pour arrêter"
                             : "Appuyez sur E pour interagir avec la valve";
    }
}
