//using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour, IInteractable
{
    [Header("Camera")]
    [SerializeField] private Transform cameraPivot;            // Où la caméra se place pour interagir
    [SerializeField] private Transform cameraExitPosition;     // Où la caméra se replace après interaction
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MonoBehaviour fpsControllerScript;

    [Header("UI & Audio")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource valveAudioSource;
    [SerializeField] private AudioClip valveTurnSound;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 30f;

    private bool isInteracting = false;
    private float currentRotation = 0f;
    private float timer = 0f;

    private void Start()
    {
        if (canvas != null)
            canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isInteracting) return;

        timer += Time.deltaTime;

        // Rotation vers la droite
        if (Input.GetKey(KeyCode.R))
        {
            RotateValve(rotationSpeed);
        }
        // Rotation vers la gauche
        else if (Input.GetKey(KeyCode.L))
        {
            RotateValve(-rotationSpeed);
        }
        else
        {
            if (valveAudioSource.isPlaying)
                valveAudioSource.Stop();
        }

        currentRotation = Mathf.Repeat(currentRotation, 360f);

        // Quitter l’interaction
        if (Input.GetKeyDown(KeyCode.E) && timer > 1f)
        {
            EndInteraction();
        }
    }

    public void Interact()
    {
        isInteracting = true;
        timer = 0f;

        // Téléporte la caméra au pivot
        playerCamera.position = cameraPivot.position;
        playerCamera.rotation = cameraPivot.rotation;

        if (canvas != null)
            canvas.gameObject.SetActive(true);

        if (fpsControllerScript != null)
            fpsControllerScript.enabled = false;
    }

    private void EndInteraction()
    {
        isInteracting = false;

        // Replace la caméra à la sortie
        if (cameraExitPosition != null)
        {
            playerCamera.position = cameraExitPosition.position;
            playerCamera.rotation = cameraExitPosition.rotation;
        }

        if (canvas != null)
            canvas.gameObject.SetActive(false);

        if (fpsControllerScript != null)
            fpsControllerScript.enabled = true;

        if (valveAudioSource != null && valveAudioSource.isPlaying)
            valveAudioSource.Stop();
    }

    private void RotateValve(float speed)
    {
        float delta = speed * Time.deltaTime;
        transform.Rotate(Vector3.up, delta);
        currentRotation += delta;

        if (!valveAudioSource.isPlaying)
        {
            valveAudioSource.clip = valveTurnSound;
            valveAudioSource.loop = true;
            valveAudioSource.Play();
        }
    }

    public Canvas GetCanvas() => canvas;
    public void SetStartValue(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);
        float angle = percent / 100f * 360f;
        currentRotation = angle;
    }
    public float GetCurrentRotation() => currentRotation;
}
