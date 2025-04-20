using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
    Canvas GetCanvas(); // Ajout d'une m�thode pour r�cup�rer le Canvas
}
interface IPickable : IInteractable
{
    void Interact(Transform holdPoint); // M�thode sp�cifique pour ramasser l'objet avec un point de prise
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
            if (hitInfo.collider.gameObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
                foundInteractable = true;

                // R�cup�rer le Canvas de l'objet interactif
                Canvas objCanvas = null;
                try
                {
                    objCanvas = interactObj.GetCanvas();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error accessing Canvas: {e.Message}");
                }

                // Activer le Canvas si valide
                if (objCanvas != null && objCanvas.gameObject != null)
                {
                    objCanvas.gameObject.SetActive(true);

                    // Si le joueur appuie sur "E", on interagit
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        pickableObj.Interact(HoldPoint);
                        heldItem = pickableObj;
                    }

                    // D�sactiver le dernier Canvas si diff�rent
                    if (lastCanvas != null && lastCanvas != objCanvas)
                    {
                        try
                        {
                            if (lastCanvas.gameObject != null)
                                lastCanvas.gameObject.SetActive(false);
                        }
                        catch (System.Exception)
                        {
                            // Le Canvas a d�j� �t� d�truit, ne rien faire
                        }
                    }
                }
                


                    lastInteractable = interactObj;
                    lastCanvas = objCanvas;
                }
            }
        }

        // Si aucun interactable trouv�, d�sactiver l'ancien Canvas affich�
        if (!foundInteractable && lastCanvas != null)
        {
            try
            {
                if (lastCanvas.gameObject != null)
                    lastCanvas.gameObject.SetActive(false);
            }
            catch (System.Exception)
            {
                // Le Canvas a d�j� �t� d�truit, ne rien faire
            }

            lastInteractable = null;
            lastCanvas = null;
        }
    }

    // Gestion suppl�mentaire en cas de d�sactivation du joueur
    private void OnDisable()
    {
        // Nettoyage des r�f�rences
        if (lastCanvas != null)
        {
            try
            {
                if (lastCanvas.gameObject != null)
                    lastCanvas.gameObject.SetActive(false);
            }
            catch (System.Exception)
            {
                // Ignore les erreurs si le Canvas est d�j� d�truit
            }
        }

        lastInteractable = null;
        lastCanvas = null;
    }
}