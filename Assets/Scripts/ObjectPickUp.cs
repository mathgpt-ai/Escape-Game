using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite sp;
    [SerializeField] private Transform Holdpoint;
    [SerializeField] private GameObject itemToHoldPrefab;

    private GameObject heldCopy;
    private bool isHolding = false;
    public bool IsHolding => isHolding;

    private Canvas canvas;
    private Inventory inventory;

    public Canvas GetCanvas() => canvas;
    public void Interact()
    {
        if (inventory != null)
        {
            inventory.AddItem(sp); // Ajoute l'item à l'inventaire si y'est trouvé

            if (canvas != null)
            {
                canvas.gameObject.SetActive(false); // Optionally hide UI
            }

            gameObject.GetComponent<BoxCollider>().enabled = false; 
            isHolding = true;
            Instantiate(gameObject, Holdpoint);

<<<<<<< HEAD
            heldCopy = Instantiate(gameObject, holdPoint.position, holdPoint.rotation, holdPoint);
            heldCopy.GetComponent<ObjectPickUp>().enabled = false;
            heldCopy.GetComponent<Collider>().enabled =false;

            Rigidbody rb = heldCopy.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Destroy(gameObject); // Détruit l'objet initial
=======
            Destroy(gameObject);
            
>>>>>>> nigamergelab
        }
    }
    public void Drop(Transform holdPoint)
    {
        if(inventory != null)
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

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        // Démarre une coroutine pour chercher l'inventaire après 3 secondes
        StartCoroutine(FindInventoryAfterDelay());
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Attend 3 secondes avant d’exécuter la suite

        GameObject hotbar = GameObject.Find("Hotbar"); // Cherche l’objet "Hotbar" dans la scène
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>(); // Récupère le script Inventory
        }

        if (inventory == null)
        {
            Debug.LogError("Tabarnak! L'inventaire est pas trouvé dans Hotbar après l'attente!");
        }
        else
        {
            Debug.Log("Bon! L'inventaire est trouvé après 3 secondes.");
        }
    }
    
}
