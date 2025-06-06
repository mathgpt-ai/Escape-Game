using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IPickable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private GameObject itemToHoldPrefab;

    private GameObject heldCopy;
    private bool isHolding = false;
    public bool IsHolding => isHolding;

    private Canvas canvas;
    private Inventory inventory;
    public int moveSpeed = 100;
    public Canvas GetCanvas() => canvas;
    public void Interact()
    {
        Debug.LogWarning("mauvais interact");
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

            heldCopy = Instantiate(gameObject, holdPoint.position, holdPoint.rotation, holdPoint);
            heldCopy.GetComponent<ObjectPickUp>().enabled = false;
            heldCopy.GetComponent<Collider>().enabled = false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Destroy(gameObject); // Détruit l'objet initial
        }
    }
    public void Drop(Transform holdPoint)
    {
        if (inventory != null)
        {
            inventory.RemoveItem();
            heldCopy.transform.parent = null;
            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();

            rb.useGravity = true;

            isHolding = false;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(holdPoint.forward * 2f, ForceMode.Impulse); // Optional toss
                Debug.Log("tossed");
            }
            heldCopy.GetComponent<ObjectPickUp>().enabled = true;
            heldCopy.GetComponent<Collider>().enabled = true;
        }
    }
    public void Inspect(Transform FrontholdPoint)
    {
        heldCopy.transform.position = Vector3.MoveTowards(heldCopy.transform.position, FrontholdPoint.position, moveSpeed * Time.deltaTime);
        heldCopy.transform.rotation = Quaternion.RotateTowards(heldCopy.transform.rotation, FrontholdPoint.rotation, moveSpeed * 100f * Time.deltaTime);

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
