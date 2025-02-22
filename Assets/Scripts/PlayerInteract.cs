using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


interface IInteractable
{
    public void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    public Transform InteractorSource;
    public float interactRange = 3f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);

            if(Physics.Raycast(ray, out RaycastHit hitInfo, interactRange)) 
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) 
                {
                  interactObj.Interact();
                }
            }
                
        }
    }
    
}
