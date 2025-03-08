using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

interface IInteractable
{
    public void Interact();
    public string GetText();
}

public class PlayerInteract : MonoBehaviour
{
    public Transform Source;
    public float interactRange = 3f;
    public Text interactionText;
    public GameObject panel;

    private void Update()
    {
        Ray ray = new Ray(Source.position, Source.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {

                interactionText.text = interactObj.GetText();
                interactionText.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }
            }
            else
            {
                interactionText.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
    }

}
