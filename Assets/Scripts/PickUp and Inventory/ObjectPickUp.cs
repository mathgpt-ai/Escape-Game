using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip dropSound;

    private GameObject heldCopy;
    private bool isHolding = false;
    private Canvas canvas;
    private Inventory inventory;
    private int inventorySlotIndex = -1; // To track which slot this item is in

    public bool IsHolding => isHolding;
    public int moveSpeed = 100;
    public Canvas GetCanvas() => canvas;

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        StartCoroutine(FindInventoryAfterDelay());
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Reduced delay time for better responsiveness
        GameObject hotbar = GameObject.Find("Hotbar");
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>();
        }
        else
        {
            Debug.LogWarning("Hotbar not found. Make sure there's a GameObject named 'Hotbar' with Inventory component.");
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

        if (inventorySlotIndex >= 0) // Check if adding to inventory was successful
        {
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
            heldCopy = Instantiate(gameObject, holdPoint.position, holdPoint.rotation, holdPoint);

            // Disable components on held copy
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null) pickupComponent.enabled = false;

            Collider col = heldCopy.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

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
        }
        else
        {
            Debug.Log("Cannot drop item - not currently selected in inventory");
        }
    }

    public void Inspect(Transform frontHoldPoint)
    {
        // Only allow inspection if this item is in the currently selected slot
        if (inventory != null && inventorySlotIndex == inventory.SelectedSlot && heldCopy != null)
        {
            heldCopy.transform.position = Vector3.MoveTowards(
                heldCopy.transform.position,
                frontHoldPoint.position,
                moveSpeed * Time.deltaTime
            );

            heldCopy.transform.rotation = Quaternion.RotateTowards(
                heldCopy.transform.rotation,
                frontHoldPoint.rotation,
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