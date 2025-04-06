using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    GameObject Player;
    [SerializeField] float CamHeight;
    private float fixedXRotation;
    private float fixedZRotation;

    // Static reference to this script for global access
    public static MiniMap Instance { get; private set; }

    // Camera component reference
    private Camera miniMapCamera;

    void Awake()
    {
        // Set up the singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Get the camera component
        miniMapCamera = GetComponent<Camera>();
        if (miniMapCamera == null)
        {
            miniMapCamera = GetComponentInChildren<Camera>();
            if (miniMapCamera == null)
            {
                Debug.LogWarning("No Camera component found on MiniMap or its children!");
            }
        }
    }

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
        fixedXRotation = transform.rotation.eulerAngles.x;
        fixedZRotation = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (Player != null)
        {
            this.gameObject.transform.position = new Vector3(Player.transform.position.x, CamHeight, Player.transform.position.z);
            transform.rotation = Quaternion.Euler(fixedXRotation, Player.transform.eulerAngles.y, fixedZRotation);
        }
    }

    IEnumerator FindPlayerCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Public function to toggle the minimap's visibility
    /// </summary>
    /// <param name="isEnabled">True to enable, false to disable</param>
    public void SetMiniMapEnabled(bool isEnabled)
    {
        // Enable/disable the GameObject itself if no camera component
        if (miniMapCamera == null)
        {
            gameObject.SetActive(isEnabled);
            return;
        }

        // Enable/disable just the camera component
        miniMapCamera.enabled = isEnabled;

        // If any UI elements are children of this object, you might want to toggle them too
        Canvas[] canvases = GetComponentsInChildren<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = isEnabled;
        }

        Debug.Log($"MiniMap is now {(isEnabled ? "enabled" : "disabled")}");
    }

    /// <summary>
    /// Convenience function to enable the minimap
    /// </summary>
    public void EnableMiniMap()
    {
        SetMiniMapEnabled(true);
    }

    /// <summary>
    /// Convenience function to disable the minimap
    /// </summary>
    public void DisableMiniMap()
    {
        SetMiniMapEnabled(false);
    }

    /// <summary>
    /// Toggles the current state of the minimap
    /// </summary>
    public void ToggleMiniMap()
    {
        bool currentState = miniMapCamera != null ? miniMapCamera.enabled : gameObject.activeSelf;
        SetMiniMapEnabled(!currentState);
    }
}