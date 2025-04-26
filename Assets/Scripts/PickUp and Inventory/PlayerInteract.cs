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
    void Drop(Transform holdPoint);
    void Inspect(Transform fromtHoldpoint);

}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
    private IInteractable lastInteractable = null;
    public Transform HoldPoint;
    private IPickable heldItem = null;
    public Transform frontHoldPoint;
    private bool isInspecting = false;
    private bool isHolding = false;
    private bool wasPressed = false;
    private void Update()
    {
        bool foundInteractable = false;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (heldItem != null)
            {
                heldItem.Drop(HoldPoint);
                heldItem = null;
                isHolding = false;
            }
        }
        if (heldItem != null && isHolding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isInspecting = true;
                wasPressed = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isInspecting = false;
            }

            if (isInspecting)
            {
                heldItem.Inspect(frontHoldPoint);
            }
            else if(wasPressed)
            {
                heldItem.Inspect(HoldPoint);
            }
        }

        Ray ray = new Ray(Source.position, Source.forward);
        Debug.DrawRay(Source.position, Source.forward * interactRange, Color.red);
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
                        isHolding = true;
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

