using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
    Canvas GetCanvas(); // Ajout d'une méthode pour récupérer le Canvas
}
interface IPickable : IInteractable
{
    void Interact(Transform holdPoint); // Méthode spécifique pour ramasser l'objet avec un point de prise
    void Drop(Transform hodPoint);
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
    private IInteractable lastInteractable = null;
    public Transform HoldPoint;
    private IPickable heldItem = null;
    private void Update()
    {
        bool foundInteractable = false;
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (heldItem != null)
            {
                heldItem.Drop(HoldPoint);
                heldItem = null;
            }
        }
        Ray ray = new Ray(Source.position, Source.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            GameObject targetObject = hitInfo.collider.gameObject;
            if (targetObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
                foundInteractable = true;

                Canvas objCanvas = interactObj.GetCanvas();
                if (objCanvas != null)
                {
                    objCanvas.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactObj is IPickable pickableObj)
                    {
                        pickableObj.Interact(HoldPoint);
                        heldItem = pickableObj;
                    }
                    else
                    {
                        interactObj.Interact(); // Pour les autres types d'objets
                    }
                }
                


                if (lastInteractable != null && lastInteractable != interactObj)
                {
                    lastInteractable.GetCanvas()?.gameObject.SetActive(false);
                }

                lastInteractable = interactObj;
            }
        }

        if (!foundInteractable && lastInteractable != null)
        {
            if (lastInteractable.GetCanvas() != null)
            {
                lastInteractable.GetCanvas().gameObject.SetActive(false);
            }

            lastInteractable = null;
        }
    }
}
