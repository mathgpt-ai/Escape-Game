using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
    Canvas GetCanvas(); // Ajout d'une méthode pour récupérer le Canvas
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
    GameObject gameObject;
    private IInteractable lastInteractable = null; // Pour éviter d’activer/désactiver en boucle

    private void Update()
    {
        bool foundInteractable = false; // Vérifier si on a trouvé un interactable

        Ray ray = new Ray(Source.position, Source.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            gameObject = hitInfo.collider.gameObject;

            if (gameObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
                foundInteractable = true;

                // Récupérer et activer le Canvas de l’objet interactif
                Canvas objCanvas = interactObj.GetCanvas();
                if (objCanvas != null)
                {
                    objCanvas.gameObject.SetActive(true);
                }

                // Si le joueur appuie sur "E", on interagit
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }

                // Désactiver le dernier interactable si c'est un autre
                if (lastInteractable != null && lastInteractable != interactObj && lastInteractable is ObjectPickUp pickup)
                {
                    if(!pickup.IsHolding)
                        lastInteractable.GetCanvas()?.gameObject.SetActive(false);
                }

                lastInteractable = interactObj;
            }
        }

        // Si aucun interactable trouvé, désactiver l'ancien Canvas affiché

        if (!foundInteractable && lastInteractable != null)
        {
            if (lastInteractable is ObjectPickUp pickup)
            {
                if (!pickup.IsHolding)
                {
                    lastInteractable.GetCanvas()?.gameObject.SetActive(false);
                }
                lastInteractable = null;
            }

        }
    }
}
