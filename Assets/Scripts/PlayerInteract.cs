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
    private IInteractable lastInteractable = null; // Pour éviter d'activer/désactiver en boucle
    private Canvas lastCanvas = null; // Référence directe au dernier Canvas

    private void Update()
    {
        bool foundInteractable = false; // Vérifier si on a trouvé un interactable
        Ray ray = new Ray(Source.position, Source.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
                foundInteractable = true;

                // Récupérer le Canvas de l'objet interactif
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
                        try
                        {
                            interactObj.Interact();
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"Error during interaction: {e.Message}");
                        }
                    }

                    // Désactiver le dernier Canvas si différent
                    if (lastCanvas != null && lastCanvas != objCanvas)
                    {
                        try
                        {
                            if (lastCanvas.gameObject != null)
                                lastCanvas.gameObject.SetActive(false);
                        }
                        catch (System.Exception)
                        {
                            // Le Canvas a déjà été détruit, ne rien faire
                        }
                    }

                    lastInteractable = interactObj;
                    lastCanvas = objCanvas;
                }
            }
        }

        // Si aucun interactable trouvé, désactiver l'ancien Canvas affiché
        if (!foundInteractable && lastCanvas != null)
        {
            try
            {
                if (lastCanvas.gameObject != null)
                    lastCanvas.gameObject.SetActive(false);
            }
            catch (System.Exception)
            {
                // Le Canvas a déjà été détruit, ne rien faire
            }

            lastInteractable = null;
            lastCanvas = null;
        }
    }

    // Gestion supplémentaire en cas de désactivation du joueur
    private void OnDisable()
    {
        // Nettoyage des références
        if (lastCanvas != null)
        {
            try
            {
                if (lastCanvas.gameObject != null)
                    lastCanvas.gameObject.SetActive(false);
            }
            catch (System.Exception)
            {
                // Ignore les erreurs si le Canvas est déjà détruit
            }
        }

        lastInteractable = null;
        lastCanvas = null;
    }
}