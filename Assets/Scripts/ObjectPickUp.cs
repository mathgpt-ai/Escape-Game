using System.Collections;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite sp;

    Canvas canvas;

    private Inventory inventory;

   
    public Canvas GetCanvas()
    {
        
        return canvas;
    }
    public void Interact()
    {
        if (inventory != null)
        {
            inventory.AddItem(sp); // Ajoute l'item à l'inventaire si y'est trouvé
            
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
