using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite sp;
    Canvas can;
    GameObject go;

    private Inventory inventory;

    public string GetText()
    {
        return "yolo";
    }

    public void Interact()
    {
        if (inventory != null)
        {
            inventory.AddItem(sp); // Ajoute l'item � l'inventaire si y'est trouv�
            
        }
    }

    void Start()
    {
        // D�marre une coroutine pour chercher l'inventaire apr�s 3 secondes
        StartCoroutine(FindInventoryAfterDelay());
    }

    IEnumerator FindInventoryAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Attend 3 secondes avant d�ex�cuter la suite

        GameObject hotbar = GameObject.Find("Hotbar"); // Cherche l�objet "Hotbar" dans la sc�ne
        if (hotbar != null)
        {
            inventory = hotbar.GetComponent<Inventory>(); // R�cup�re le script Inventory
        }

        if (inventory == null)
        {
            Debug.LogError("Tabarnak! L'inventaire est pas trouv� dans Hotbar apr�s l'attente!");
        }
        else
        {
            Debug.Log("Bon! L'inventaire est trouv� apr�s 3 secondes.");
        }
    }
}
