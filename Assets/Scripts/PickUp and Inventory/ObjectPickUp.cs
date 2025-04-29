using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dropSound;

    // Custom hold point offset for this specific object
    [SerializeField] private float holdPositionOffsetX = 0f;
    [SerializeField] private float holdPositionOffsetY = 0f;
    [SerializeField] private float holdPositionOffsetZ = 0f;
    [SerializeField] private float holdRotationOffsetX = 0f;
    [SerializeField] private float holdRotationOffsetY = 0f;
    [SerializeField] private float holdRotationOffsetZ = 0f;

    // New field to control if the object can be inspected
    [SerializeField] private bool canBeInspected = true;
    [Tooltip("If set to false, this object cannot be inspected by the player")]

    public int moveSpeed = 100;

    private GameObject heldCopy;
    private bool isHolding = false;
    private Canvas canvas;
    private Inventory inventory;
    private int inventorySlotIndex = -1;
    private GameObject customHoldPointObj;

    public bool IsHolding => isHolding;
    public Canvas GetCanvas() => canvas;
    public bool CanBeInspected => canBeInspected;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        StartCoroutine(FindInventoryAfterDelay());
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        GameObject hotbar = GameObject.Find("Hotbar");
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>();
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

            // Create held copy at the custom hold point
            isHolding = true;
            heldCopy = Instantiate(
                gameObject,
                customHoldPointObj.transform.position,
                customHoldPointObj.transform.rotation,
                customHoldPointObj.transform
            );

            // Disable components on held copy
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null) pickupComponent.enabled = false;

            Collider col = heldCopy.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Only show if this is the currently selected slot
            heldCopy.SetActive(inventorySlotIndex == inventory.SelectedSlot);

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

            // Ensure object is active before dropping
            heldCopy.SetActive(true);

            // Detach from parent
            heldCopy.transform.parent = null;

            // Enable physics
            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(holdPoint.forward * 2f, ForceMode.Impulse);
            }

            // Re-enable components
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null) pickupComponent.enabled = true;

            Collider col = heldCopy.GetComponent<Collider>();
            if (col != null) col.enabled = true;

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
        // 3. The object can be inspected (NEW CHECK)
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
}