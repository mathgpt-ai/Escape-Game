using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MonoBehaviour fpsControllerScript;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float rotationspeed = 30f;
    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource valveAudioSource;
    [SerializeField] private AudioClip valveTurnSound;
    [SerializeField] private Collider cameraTargetZone;
    [SerializeField] private Transform cameraExitPosition;

    private Quaternion returnRotation;
    private bool isInteracting = false;
    private float currentRotation;
    private Quaternion startRotation;
    private float Timer = 0.0f;
    Vector3 returnPosition;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        if (isInteracting)
        {
            Timer += Time.deltaTime;
            // ici on veroille la cam pour sur la valve avec une petite transition
            playerCamera.position = Vector3.Lerp(playerCamera.position, cameraPivot.position, Time.deltaTime * transitionSpeed);
            playerCamera.rotation = Quaternion.Lerp(playerCamera.rotation, cameraPivot.rotation, Time.deltaTime * transitionSpeed);

            if (Input.GetKey(KeyCode.R))
            {
                float delta = rotationspeed * Time.deltaTime;
                transform.Rotate(Vector3.up, delta);
                currentRotation += delta;

                if(!valveAudioSource.isPlaying)
                {
                    valveAudioSource.clip = valveTurnSound;
                    valveAudioSource.loop = true;
                    valveAudioSource.Play();
                }
            }
            else if (Input.GetKey(KeyCode.L))
            {
                float delta = -rotationspeed * Time.deltaTime;
                transform.Rotate(Vector3.up, delta);
                currentRotation += delta;

                if(!valveAudioSource.isPlaying)
                {
                    valveAudioSource.clip = valveTurnSound;
                    valveAudioSource.loop = true;
                    valveAudioSource.Play();
                }
            }
            else
            {
                if(valveAudioSource.isPlaying)
                {
                    valveAudioSource.Stop();
                }
            }

            currentRotation = Mathf.Repeat(currentRotation, 360f);

            if (Input.GetKeyDown(KeyCode.E) && Timer > 1.0f)
            {
                EndInteraction();
                Timer = 0.0f;
            }
        }
    }

    public void Interact()
    {
        returnPosition = playerCamera.position;
        returnRotation = playerCamera.rotation;

        isInteracting = true;
        Timer = 0f;

        if (canvas != null)
            canvas.gameObject.SetActive(true);

        if (fpsControllerScript != null)
            fpsControllerScript.enabled = false;
    }

    private void EndInteraction()
    {
        isInteracting = false;

        if (canvas != null) canvas.gameObject.SetActive(false);

        if (fpsControllerScript != null) fpsControllerScript.enabled = true;

        if (valveAudioSource != null && valveAudioSource.isPlaying)
            valveAudioSource.Stop();

        if (cameraExitPosition != null)
        {
            playerCamera.position = cameraExitPosition.position;
            playerCamera.rotation = cameraExitPosition.rotation;
        }
        StartCoroutine(CorrectCameraAfterDelay(0.2f));
    }

    public Canvas GetCanvas() => canvas;

    public void SetStartValue(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f);
        float angle = percent / 100f * 360f;
        currentRotation = angle;
    }
    private IEnumerator CorrectCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!cameraTargetZone.bounds.Contains(playerCamera.position))
        {
            playerCamera.position = returnPosition;
            playerCamera.rotation = returnRotation;
        }
    }
    public float GetCurrentRotation() => currentRotation;
}
