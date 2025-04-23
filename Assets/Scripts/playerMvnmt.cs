using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public bool isPositiveCharge = true;
    public float freezeLookDistance = 1.5f; 
    public LayerMask magnetLayer;
    public Transform spawnPoint;
    [HideInInspector] public bool freezeCameraRotation = false;

    private bool rotationLocked = false;
    private Rigidbody rb;
    private float rotX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
        else
        {
            Debug.LogWarning("⚠️ Aucun spawn point assigné au joueur.");
        }
        rb.useGravity = false;
        rb.drag = 0.5f;
        Cursor.lockState = CursorLockMode.Locked;

        UpdateChargeEffect();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            rotationLocked = !rotationLocked;
            Debug.Log("Rotation lock : " + rotationLocked);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRotation();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            RespawnPlayer();
        }

        if (!rotationLocked)
        {
            RotateCamera();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isPositiveCharge = !isPositiveCharge;
            UpdateChargeEffect();
        }
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (rotationLocked)
        {
            // Empêche les rotations accidentelles en maintenant le joueur droit
            rb.angularVelocity = Vector3.zero;

            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, euler.y, 0f);

        }
    }
    void RespawnPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        Debug.Log("Player respawned!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadly"))
        {
            Debug.Log("dead trash bozo");
            RespawnPlayer();
        }
        if (other.CompareTag("TrapTP"))
        {
            Debug.Log("DUMBASS");
            RespawnPlayer();
        }
    }
    void ResetRotation()
    {
        Vector3 currentEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentEuler.y, 0f);

        rotX = 0f;
        Camera.main.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        rb.angularVelocity = Vector3.zero;

        Debug.Log("Rotation recentrée !");
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);

        if (!rotationLocked)
        {
            // Le joueur peut tourner normalement
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + mouseX, 0f);
        }
        else
        {
            // Seulement la caméra tourne sur Y (on ne bouge pas le corps)
            Camera.main.transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    bool IsLookingAtMagnet()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, freezeLookDistance, magnetLayer))
        {
            return true;
        }
        return false;
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Jump") - Input.GetAxis("Fire3");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
        rb.AddForce(move * moveSpeed, ForceMode.Acceleration);
    }

    void UpdateChargeEffect()
    {
        Color newColor = isPositiveCharge ? Color.red : Color.blue;
        GetComponent<Renderer>().material.color = newColor;

        Debug.Log($"🔄 Charge du joueur changée: {(isPositiveCharge ? "POSITIVE (Rouge)" : "NÉGATIVE (Bleu)")}");

        magnets[] allMagnets = FindObjectsOfType<magnets>();
        foreach (var magnet in allMagnets)
        {
            magnet.UpdatePlayerCharge(isPositiveCharge);
        }
    }
}