using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IPickable
{
    [Header("Visual Settings")]
    [SerializeField] private Sprite sp;
    [Tooltip("Optional alternate prefab to spawn when object is held (if null, a copy of this object is used)")]
    [SerializeField] private GameObject itemToHoldPrefab;

    [Header("Hold Position Settings")]
    [SerializeField] private float holdPositionOffsetX = 0f;
    [SerializeField] private float holdPositionOffsetY = 0f;
    [SerializeField] private float holdPositionOffsetZ = 0f;
    [SerializeField] private float holdRotationOffsetX = 0f;
    [SerializeField] private float holdRotationOffsetY = 0f;
    [SerializeField] private float holdRotationOffsetZ = 0f;

    [Header("Scale Settings")]
    [Tooltip("Scale multiplier for the object when held (1 = original size)")]
    [SerializeField] private float heldScaleX = 1.0f;
    [SerializeField] private float heldScaleY = 1.0f;
    [SerializeField] private float heldScaleZ = 1.0f;

    [Header("Behavior Settings")]
    [Tooltip("If false, this object cannot be inspected (brought closer to view)")]
    [SerializeField] private bool canBeInspected = true;
    [SerializeField] private int moveSpeed = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dropSound;

    //[Header("Quest")]
    //[SerializeField] private TaskObject TaskObject;
    //[SerializeField] private ChecklistManager checklistManager;

    // Dictionary to store original scales
    private static Dictionary<string, Vector3> prefabOriginalScales = new Dictionary<string, Vector3>();

    private GameObject heldCopy;
    private bool isHolding = false;
    private Canvas canvas;
    private Inventory inventory;
    private int inventorySlotIndex = -1;
    private GameObject customHoldPointObj;
    private string objectId;

    // Keep track of all held items for visibility management
    private static Dictionary<int, GameObject> slotToHeldObject = new Dictionary<int, GameObject>();

    public bool IsHolding => isHolding;
    public Canvas GetCanvas() => canvas;
    public bool CanBeInspected => canBeInspected;

    void Awake()
    {
        // Create a unique identifier for this prefab type
        objectId = gameObject.name.Replace("(Clone)", "");

        // Only store the original scale once per prefab type
        if (!prefabOriginalScales.ContainsKey(objectId))
        {
            prefabOriginalScales[objectId] = transform.localScale;
        }
    }

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        StartCoroutine(FindInventoryAfterDelay());
    }

    void Update()
    {
        // Check if we need to update visibility based on selected slot
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (inventory != null && heldCopy != null && inventorySlotIndex >= 0)
        {
            bool shouldBeVisible = (inventorySlotIndex == inventory.SelectedSlot);

            // Only update if visibility state has changed
            if (heldCopy.activeSelf != shouldBeVisible)
            {
                heldCopy.SetActive(shouldBeVisible);
            }
        }
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        GameObject hotbar = GameObject.Find("Hotbar");
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>();
            if (inventory != null)
            {
                // Once we find the inventory, immediately update visibility
                UpdateVisibility();
            }
        }
    }

    public void Interact()
    {
        Debug.LogWarning("Use Interact(Transform) instead");
    }

    public void Interact(Transform holdPoint)
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory reference not found");
            return;
        }

        // Add item to inventory and track which slot it was added to
        inventorySlotIndex = inventory.AddItem(sp);

        //if (TaskObject != null && checklistManager != null)
        //{
        //    checklistManager.ForcerComplétion(TaskObject);
        //}

        if (inventorySlotIndex >= 0)
        {
            // Create custom hold point
            customHoldPointObj = new GameObject("CustomHoldPoint_" + gameObject.name);
            customHoldPointObj.transform.parent = holdPoint;

            // Apply the offsets to position
            Vector3 offsetPosition = new Vector3(holdPositionOffsetX, holdPositionOffsetY, holdPositionOffsetZ);
            customHoldPointObj.transform.localPosition = offsetPosition;

            // Apply the offsets to rotation
            Vector3 offsetRotation = new Vector3(holdRotationOffsetX, holdRotationOffsetY, holdRotationOffsetZ);
            customHoldPointObj.transform.localRotation = Quaternion.Euler(offsetRotation);

            // Play pickup sound if available
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Hide canvas prompt
            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }

            // Create held copy
            isHolding = true;

            if (itemToHoldPrefab != null)
            {
                heldCopy = Instantiate(
                    itemToHoldPrefab,
                    customHoldPointObj.transform.position,
                    customHoldPointObj.transform.rotation,
                    customHoldPointObj.transform
                );
            }
            else
            {
                heldCopy = Instantiate(
                    gameObject,
                    customHoldPointObj.transform.position,
                    customHoldPointObj.transform.rotation,
                    customHoldPointObj.transform
                );

                ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
                if (pickupComponent != null)
                {
                    pickupComponent.enabled = false;
                }
            }

            // Clean up any previous object in this slot
            if (slotToHeldObject.ContainsKey(inventorySlotIndex) && slotToHeldObject[inventorySlotIndex] != null)
            {
                Destroy(slotToHeldObject[inventorySlotIndex]);
            }

            // Register this object in our slot tracking dictionary
            slotToHeldObject[inventorySlotIndex] = heldCopy;

            // Get the original scale from our dictionary and apply the scale multipliers
            Vector3 trueOriginalScale = prefabOriginalScales[objectId];
            Vector3 newScale = new Vector3(
                trueOriginalScale.x * heldScaleX,
                trueOriginalScale.y * heldScaleY,
                trueOriginalScale.z * heldScaleZ
            );

            heldCopy.transform.localScale = newScale;

            // Disable components on held copy
            Collider col = heldCopy.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Set initial visibility based on selected slot
            bool shouldBeVisible = (inventorySlotIndex == inventory.SelectedSlot);
            heldCopy.SetActive(shouldBeVisible);

            // Destroy original object
            Destroy(gameObject);
        }
    }

    public void Drop(Transform holdPoint)
    {
        if (inventory == null || heldCopy == null) return;

        // Only allow dropping if this item is in the currently selected slot
        if (inventorySlotIndex == inventory.SelectedSlot)
        {
            // Remove from inventory
            inventory.RemoveItem();

            // Remove from our tracking dictionary
            if (slotToHeldObject.ContainsKey(inventorySlotIndex))
            {
                slotToHeldObject.Remove(inventorySlotIndex);
            }

            // Ensure object is active before dropping
            heldCopy.SetActive(true);

            // Detach from parent
            heldCopy.transform.parent = null;

            // Get the true original scale from our dictionary
            Vector3 trueOriginalScale = prefabOriginalScales[objectId];

            // Reset to the true original scale
            heldCopy.transform.localScale = trueOriginalScale;

            // Re-enable components
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null)
            {
                pickupComponent.enabled = true;
            }

            Collider col = heldCopy.GetComponent<Collider>();
            if (col != null) col.enabled = true;

            // Enable physics
            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(holdPoint.forward * 2f, ForceMode.Impulse);
            }

            // Play drop sound
            if (dropSound != null)
            {
                AudioSource.PlayClipAtPoint(dropSound, heldCopy.transform.position);
            }

            isHolding = false;
            inventorySlotIndex = -1;

            // Destroy the custom hold point
            if (customHoldPointObj != null)
            {
                Destroy(customHoldPointObj);
                customHoldPointObj = null;
            }
        }
        else
        {
            Debug.Log("Cannot drop item - not currently selected in inventory");
        }
    }

    public void Inspect(Transform inspectPoint)
    {
        // Only allow inspection if:
        // 1. Object is in the selected inventory slot
        // 2. The object exists
        // 3. The object can be inspected
        if (inventory != null && inventorySlotIndex == inventory.SelectedSlot && heldCopy != null && canBeInspected)
        {
            heldCopy.transform.position = Vector3.MoveTowards(
                heldCopy.transform.position,
                inspectPoint.position,
                moveSpeed * Time.deltaTime
            );

            heldCopy.transform.rotation = Quaternion.RotateTowards(
                heldCopy.transform.rotation,
                inspectPoint.rotation,
                moveSpeed * 100f * Time.deltaTime
            );
        }
    }

    public GameObject GetHeldCopy()
    {
        return heldCopy;
    }

    public int GetInventorySlotIndex()
    {
        return inventorySlotIndex;
    }

    // This method can be called externally to force visibility update
    public void ForceVisibilityUpdate()
    {
        UpdateVisibility();
    }

    // Static method to update visibility of all held objects
    public static void UpdateAllObjectVisibility(int selectedSlot)
    {
        foreach (var entry in slotToHeldObject)
        {
            if (entry.Value != null)
            {
                entry.Value.SetActive(entry.Key == selectedSlot);
            }
        }
    }
}