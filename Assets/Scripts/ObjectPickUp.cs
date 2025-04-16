using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;

    private bool isHolding = false;
    public bool IsHolding => isHolding;

    private Canvas canvas;
    private Inventory inventory;

    public Canvas GetCanvas() => canvas;

    public void Interact()
    {
        Debug.LogWarning("Utilisez Interact(Transform holdPoint) au lieu de ça.");
    }

    public void Interact(Transform holdPoint)
    {
        if (inventory != null)
        {
            inventory.AddItem(sp); // Ajoute l'item à l'inventaire

            if (canvas != null)
            {
                canvas.gameObject.SetActive(false);
            }

            isHolding = true;

            GameObject heldCopy = Instantiate(gameObject, holdPoint.position, holdPoint.rotation, holdPoint);
            Destroy(heldCopy.GetComponent<ObjectPickUp>());
            Destroy(heldCopy.GetComponent<Collider>());

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Destroy(gameObject); // Détruit l'objet initial
        }
    }

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        StartCoroutine(FindInventoryAfterDelay());
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
