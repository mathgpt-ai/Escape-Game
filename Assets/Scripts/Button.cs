using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public Vector3 moveOffset = new Vector3(0, -0.1f, 0); // Move down when pressed
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void Interact()
    {
        transform.position += moveOffset;
        Debug.Log("Button pressed!");

        // Start the timer to reset after 2 seconds
        StartCoroutine(ResetButton());
    }

    private IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(2f);
        transform.position = originalPosition;
        Debug.Log("Button reset!");
    }
}
