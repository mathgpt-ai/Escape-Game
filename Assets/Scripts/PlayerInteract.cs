using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.UI;
using UnityEngine.UIElements;
>>>>>>> main

interface IInteractable
{
    void Interact();
    Canvas GetCanvas(); // Ajout d'une méthode pour récupérer le Canvas
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
<<<<<<< HEAD

    private IInteractable lastInteractable = null; // Pour éviter d’activer/désactiver en boucle
=======
    public Text interactionText;
    public GameObject panel;
>>>>>>> main

    private void Update()
    {
        bool foundInteractable = false; // Vérifier si on a trouvé un interactable

        Ray ray = new Ray(Source.position, Source.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.GetComponent<MonoBehaviour>() is IInteractable interactObj)
            {
<<<<<<< HEAD
                foundInteractable = true;

                // Récupérer et activer le Canvas de l’objet interactif
                Canvas objCanvas = interactObj.GetCanvas();
                if (objCanvas != null)
                {
                    objCanvas.gameObject.SetActive(true);
                }

                // Si le joueur appuie sur "E", on interagit
=======

                interactionText.text = interactObj.GetText();
                interactionText.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);

>>>>>>> main
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }
<<<<<<< HEAD

                // Désactiver le dernier interactable si c'est un autre
                if (lastInteractable != null && lastInteractable != interactObj)
                {
                    lastInteractable.GetCanvas()?.gameObject.SetActive(false);
                }

                lastInteractable = interactObj;
=======
            }
            else
            {
                interactionText.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
>>>>>>> main
            }
        }

        // Si aucun interactable trouvé, désactiver l'ancien Canvas affiché
        if (!foundInteractable && lastInteractable != null)
        {
<<<<<<< HEAD
            lastInteractable.GetCanvas()?.gameObject.SetActive(false);
            lastInteractable = null;
=======
            interactionText.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
>>>>>>> main
        }
    }
}
