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
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
<<<<<<< HEAD
    private IInteractable lastInteractable = null; // Pour �viter d'activer/d�sactiver en boucle
    private Canvas lastCanvas = null; // R�f�rence directe au dernier Canvas

    private void Update()
    {
        bool foundInteractable = false; // V�rifier si on a trouv� un interactable
=======
    private IInteractable lastInteractable = null;
    public Transform HoldPoint;

    private void Update()
    {
        bool foundInteractable = false;

>>>>>>> FirstPerson
        Ray ray = new Ray(Source.position, Source.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
<<<<<<< HEAD
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
=======
            GameObject targetObject = hitInfo.collider.gameObject;
            if (targetObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
                foundInteractable = true;

                Canvas objCanvas = interactObj.GetCanvas();
                if (objCanvas != null)
>>>>>>> FirstPerson
                {
                    objCanvas.gameObject.SetActive(true);

<<<<<<< HEAD
                    // Si le joueur appuie sur "E", on interagit
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        try
                        {
                            interactObj.Interact();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"Error during interaction: {e.Message}");
                        }
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
=======
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactObj is IPickable pickableObj)
                    {
                        pickableObj.Interact(HoldPoint); // Passe le holdPoint pour les objets ramassables
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
>>>>>>> FirstPerson

                    lastInteractable = interactObj;
                    lastCanvas = objCanvas;
                }
            }
        }

<<<<<<< HEAD
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
=======
        if (!foundInteractable && lastInteractable != null)
        {
            if (lastInteractable.GetCanvas() != null)
            {
                lastInteractable.GetCanvas().gameObject.SetActive(false);
            }

            lastInteractable = null;
>>>>>>> FirstPerson
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