using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact();
    public string GetText();
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
    public Text interactionText; // Assign a UI Text in the Inspector

    private void Update()
    {
        Ray ray = new Ray(Source.position, Source.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // Update interaction text dynamically
                interactionText.text = interactObj.GetText();
                interactionText.gameObject.SetActive(true);

                // If player presses 'E', interact with the object
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

}
