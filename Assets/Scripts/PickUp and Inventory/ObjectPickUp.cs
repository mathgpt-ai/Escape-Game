using System.Collections;
using UnityEngine;
public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;
    [SerializeField] private float holdDistance = 0.5f; // Controls only forward distance

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
            // Add item to inventory
            inventory.AddItem(sp);

            // Find which slot contains our item
            for (int i = 0; i < 4; i++)
            {
                if (inventory.HasItemInSlot(i) && inventory.GetCurrentItem() == sp)
                {
                    inventorySlotIndex = i;
                    break;
                }
            }

            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }

            isHolding = true;

            // Create held copy at the hold point
            heldCopy = Instantiate(gameObject, holdPoint);

            // Set position - ONLY modify Z (forward) axis based on holdDistance
            // Keep X and Y at zero to maintain alignment with hold point
            heldCopy.transform.localPosition = new Vector3(0, 0, holdDistance);

            // Disable components
            Collider collider = heldCopy.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            ObjectPickUp pickupScript = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupScript != null) pickupScript.enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Set visibility based on currently selected slot
            bool shouldBeVisible = (inventory.SelectedSlot == inventorySlotIndex);
            heldCopy.SetActive(shouldBeVisible);

            Destroy(gameObject);
        }
    }

    public void Drop(Transform holdPoint)
    {
        if (inventory != null &&
            inventory.SelectedSlot == inventorySlotIndex &&
            inventory.HasItemInCurrentSlot() &&
            heldCopy != null &&
            heldCopy.activeSelf)
        {
            inventory.RemoveItem();

            heldCopy.transform.parent = null;

            // Re-enable components
            Collider collider = heldCopy.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;

            ObjectPickUp pickupScript = heldCopy.GetComponent<ObjectPickUp>();
            if (pickupScript != null) pickupScript.enabled = true;

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

    void Update()
    {
        // Toggle visibility based on selected inventory slot
        if (isHolding && heldCopy != null && inventory != null)
        {
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