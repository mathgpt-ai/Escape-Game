using UnityEngine;

public class Spineuse : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Rotation speed around X axis in degrees per second")]
    [SerializeField] private float rotationSpeedX = 0f;

    [Tooltip("Rotation speed around Y axis in degrees per second")]
    [SerializeField] private float rotationSpeedY = 30f;

    [Tooltip("Rotation speed around Z axis in degrees per second")]
    [SerializeField] private float rotationSpeedZ = 0f;

    [Header("Advanced Options")]
    [Tooltip("If true, rotation will be relative to the object's local axes")]
    [SerializeField] private bool useLocalRotation = true;

    [Tooltip("If true, rotation speed will smoothly change direction over time")]
    [SerializeField] private bool oscillate = false;

    [Tooltip("How quickly the rotation changes direction when oscillating (seconds)")]
    [SerializeField] private float oscillationPeriod = 5f;

    private Vector3 rotationPerSecond;
    private float oscillationTimer = 0f;
    private float oscillationMultiplier = 1f;

    void Start()
    {
        // Initialize rotation vector
        rotationPerSecond = new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
    }

    void Update()
    {
        // Handle oscillation if enabled
        if (oscillate)
        {
            oscillationTimer += Time.deltaTime;

            if (oscillationTimer >= oscillationPeriod)
            {
                oscillationTimer = 0f;
                oscillationMultiplier *= -1f; // Reverse direction
            }

            // Create a smooth transition using a sine wave
            float smoothFactor = oscillationMultiplier * Mathf.Sin(oscillationTimer / oscillationPeriod * Mathf.PI);

            // Apply rotation with time-based movement and oscillation
            if (useLocalRotation)
            {
                transform.Rotate(rotationPerSecond * Time.deltaTime * smoothFactor, Space.Self);
            }
            else
            {
                transform.Rotate(rotationPerSecond * Time.deltaTime * smoothFactor, Space.World);
            }
        }
        else
        {
            // Apply constant rotation with time-based movement
            if (useLocalRotation)
            {
                transform.Rotate(rotationPerSecond * Time.deltaTime, Space.Self);
            }
            else
            {
                transform.Rotate(rotationPerSecond * Time.deltaTime, Space.World);
            }
        }
    }
}
