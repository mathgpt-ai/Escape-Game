using System.Collections;
using UnityEngine;
public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;
    [SerializeField] private float holdDistance = 0.5f; // Reduced distance for closer hold
    [SerializeField] private Vector3 holdOffset = new Vector3(0, 0, 0); // Additional offset for fine-tuning position

    private GameObject heldCopy;
    private bool isHolding = false;
    public bool IsHolding => isHolding;
    private Canvas canvas;
    private Inventory inventory;
    public int moveSpeed = 100;
    private int inventorySlotIndex = -1;

    public Canvas GetCanvas() => canvas;

    public void Interact()
    {
        Debug.LogWarning("mauvais interact");
    }

    public void Interact(Transform holdPoint)
    {
        if (inventory != null)
        {
            // Store the slot index
            inventorySlotIndex = inventory.SelectedSlot;

            // Add item to inventory
            inventory.AddItem(sp);

            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }

            isHolding = true;

            // Create the held copy at the hold point first
            heldCopy = Instantiate(gameObject, holdPoint.position, holdPoint.rotation, holdPoint);

            // Position adjustment can be done after instantiation
            // This ensures it's in front of the player (uses local position since parented to holdPoint)
            heldCopy.transform.localPosition = new Vector3(0, 0, holdDistance) + holdOffset;

            // Disable components on the held copy
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null) pickupComponent.enabled = false;

            Collider collider = heldCopy.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Destroy(gameObject); // Destroy the original object
        }
    }

    public void Drop(Transform holdPoint)
    {
        // Only allow dropping if this item's slot is currently selected
        if (inventory != null &&
            inventory.SelectedSlot == inventorySlotIndex &&
            inventory.HasItemInCurrentSlot() &&
            heldCopy != null &&
            heldCopy.activeSelf)
        {
            inventory.RemoveItem();

            heldCopy.transform.parent = null;

            // Re-enable components
            ObjectPickUp pickupComponent = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupComponent != null) pickupComponent.enabled = true;

            Collider collider = heldCopy.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(holdPoint.forward * 2f, ForceMode.Impulse);
            }

            isHolding = false;
            inventorySlotIndex = -1;
        }
    }

    public void Inspect(Transform frontHoldPoint)
    {
        // Only inspect if the correct slot is selected
        if (inventory != null &&
            inventory.SelectedSlot == inventorySlotIndex &&
            inventory.HasItemInCurrentSlot() &&
            heldCopy != null &&
            heldCopy.activeSelf)
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

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        StartCoroutine(FindInventoryAfterDelay());
    }

    private void Update()
    {
        // Toggle visibility based on selected slot
        if (isHolding && heldCopy != null && inventory != null)
        {
            // Show object only when its slot is selected
            bool shouldBeVisible = (inventory.SelectedSlot == inventorySlotIndex);

            if (heldCopy.activeSelf != shouldBeVisible)
            {
                heldCopy.SetActive(shouldBeVisible);
            }
        }
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        GameObject hotbar = GameObject.Find("Hotbar");
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>();
        }
    }
}