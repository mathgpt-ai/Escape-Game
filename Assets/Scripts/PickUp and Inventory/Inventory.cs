using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots; // Les emplacements de l'inventaire (cadres visuels)
    [SerializeField] private Image[] inventoryItems; // Les images des objets dans l'inventaire
    [SerializeField] private Color selectedColor = Color.yellow; // Couleur du slot sélectionné
    [SerializeField] private Color slotColor = Color.white; // Couleur des autres slots
    [SerializeField] private AudioClip itemSwitchSound; // Son lors du changement de slot

    private int selected = 0; // Slot actuellement sélectionné
    private Sprite[] itemSprites; // Pour stocker les sprites des objets

    public int SelectedSlot => selected;

    // Touches pour changer d'objet dans l'inventaire
    private KeyCode[] Ikeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    private void Start()
    {
        // Initialiser le tableau de sprites 
        itemSprites = new Sprite[inventoryItems.Length];

        // Désactiver tous les slots d'items
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].enabled = false;
        }
    }

    private void Update()
    {
        // Vérifie si le joueur appuie sur une des touches pour changer de slot
        for (int i = 0; i < Ikeys.Length; i++)
        {
            if (Input.GetKeyDown(Ikeys[i])) // Change seulement à l'appui, évite le spam
            {
                // Jouer le son de changement si disponible
                if (itemSwitchSound != null && i != selected)
                {
                    AudioSource.PlayClipAtPoint(itemSwitchSound, Camera.main.transform.position);
                }

                selected = i;
            }
        }

        // Met à jour les couleurs des slots pour montrer lequel est sélectionné
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].color = (i == selected) ? selectedColor : slotColor;
        }
    }

    public int AddItem(Sprite newItem)
    {
        // Vérifie si l'objet est déjà dans l'inventaire
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].enabled && inventoryItems[i].sprite == newItem)
            {
                // Si l'objet est déjà dans l'inventaire, on retourne l'index
                Debug.Log("Cet objet est déjà dans l'inventaire.");
                return i; // Retourne l'index de l'emplacement où se trouve l'objet
            }
        }

        // Tente d'abord d'ajouter au slot sélectionné
        if (!inventoryItems[selected].enabled)
        {
            itemSprites[selected] = newItem;
            inventoryItems[selected].sprite = newItem;
            inventoryItems[selected].enabled = true;
            return selected;
        }

        // Si le slot sélectionné est occupé, cherche un slot libre
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (!inventoryItems[i].enabled)
            {
                itemSprites[i] = newItem;
                inventoryItems[i].sprite = newItem;
                inventoryItems[i].enabled = true;
                return i;
            }
        }

        // Si aucun slot libre n'est trouvé
        Debug.Log("L'inventaire est plein.");
        return -1;
    }

    public void RemoveItem()
    {
        if (inventoryItems[selected].enabled)
        {
            itemSprites[selected] = null;
            inventoryItems[selected].sprite = null;
            inventoryItems[selected].enabled = false;
        }
    }

    public Sprite GetSelectedItemSprite()
    {
        return itemSprites[selected];
    }

    public bool IsSlotOccupied(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
        {
            return inventoryItems[slotIndex].enabled;
        }
        return false;
    }
}