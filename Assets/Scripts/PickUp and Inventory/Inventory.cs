using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots; // Les emplacements de l'inventaire (cadres visuels)
    [SerializeField] private Image[] inventoryItems; // Les images des objets dans l'inventaire
    [SerializeField] private Color selectedColor = Color.yellow; // Couleur du slot s�lectionn�
    [SerializeField] private Color slotColor = Color.white; // Couleur des autres slots
    [SerializeField] private AudioClip itemSwitchSound; // Son lors du changement de slot

    private int selected = 0; // Slot actuellement s�lectionn�
    private int previousSelected = 0; // Keep track of the previous selection

    public int SelectedSlot => selected;

    // Touches pour changer d'objet dans l'inventaire
    private KeyCode[] Ikeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    private void Start()
    {
        // D�sactiver tous les slots d'items
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].enabled = false;
        }
    }

    private void Update()
    {
        // V�rifie si le joueur appuie sur une des touches pour changer de slot
        for (int i = 0; i < Ikeys.Length; i++)
        {
            if (Input.GetKeyDown(Ikeys[i])) // Change seulement � l'appui, �vite le spam
            {
                previousSelected = selected;
                selected = i;
                // Only play sound if we actually changed slots
                if (previousSelected != selected && itemSwitchSound != null)
                {
                    AudioSource.PlayClipAtPoint(itemSwitchSound, Camera.main.transform.position);
                }
                // Update visibility of all held objects
                ObjectPickUp.UpdateAllObjectVisibility(selected);
            }
        }

        // Met � jour les couleurs des slots pour montrer lequel est s�lectionn�
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].color = (i == selected) ? selectedColor : slotColor;
        }
    }

    public int AddItem(Sprite newItem)
    {
        // V�rifie si l'objet est d�j� dans l'inventaire
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].enabled && inventoryItems[i].sprite == newItem)
            {
                // Si l'objet est d�j� dans l'inventaire, on retourne l'index
                Debug.Log("Cet objet est d�j� dans l'inventaire.");
                return i; // Retourne l'index de l'emplacement o� se trouve l'objet
            }
        }

        // Tente d'abord d'ajouter au slot s�lectionn�
        if (!inventoryItems[selected].enabled)
        {
            inventoryItems[selected].sprite = newItem;
            inventoryItems[selected].enabled = true;
            return selected;
        }

        // Si le slot s�lectionn� est occup�, cherche un slot libre
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (!inventoryItems[i].enabled)
            {
                inventoryItems[i].sprite = newItem;
                inventoryItems[i].enabled = true;
                return i;
            }
        }

        // Si aucun slot libre n'est trouv�
        Debug.Log("L'inventaire est plein.");
        return -1;
    }

    public void RemoveItem()
    {
        if (inventoryItems[selected].enabled)
        {
            inventoryItems[selected].sprite = null;
            inventoryItems[selected].enabled = false;
        }
    }

    // New method to check if a specific sprite is in the inventory
    public bool HasItem(Sprite itemSprite)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].enabled && inventoryItems[i].sprite == itemSprite)
            {
                return true;
            }
        }
        return false;
    }

    // Get all inventory items (for external access)
    public Image[] GetInventoryItems()
    {
        return inventoryItems;
    }
}