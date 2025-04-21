using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Image[] inventorySlots; // Les emplacements de l'inventaire (cadres visuels)
    [SerializeField]
    private Image[] inventoryItems; // Les images des objets dans l'inventaire

    [SerializeField]
    private Color selectedColor; // Couleur du slot sélectionné
    [SerializeField]
    private Color slotColor; // Couleur des autres slots

    private int selected = 0; // Slot actuellement sélectionné
    public int SelectedSlot => selected;
    // Touches pour changer d'objet dans l'inventaire
    private KeyCode[] Ikeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    private void Start()
    {
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
            if (Input.GetKeyDown(Ikeys[i])) // Change seulement à l’appui, évite le spam
            {
                selected = i;
            }
        }

        // Met à jour les couleurs des slots pour montrer lequel est sélectionné
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].color = (i == selected) ? selectedColor : slotColor;
        }
    }

    public void AddItem(Sprite newItem)
    {
        // Vérifie si l'objet est déjà dans l'inventaire
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].enabled && inventoryItems[i].sprite == newItem)
            {
                // Si l'objet est déjà dans l'inventaire, on arrête l'ajout
                Debug.Log("Cet objet est déjà dans l'inventaire.");
                return; // Retourne immédiatement pour éviter d'ajouter à nouveau cet item
            }
        }



        if (!inventoryItems[selected].enabled) // Vérifie si le slot est vide
        {
            inventoryItems[selected].sprite = newItem; // Assigne l’image au slot
            inventoryItems[selected].enabled = true;   // Active l’image pour l’afficher
            return; // Arrête après avoir ajouté l'item
        }


        // Si aucun slot vide n'est trouvé, on peut afficher un message ou gérer la situation
        Debug.Log("L'inventaire est plein.");
    }
    public void RemoveItem()
    {
        if (inventoryItems[selected].enabled)
        {
            inventoryItems[selected].sprite = null;
            inventoryItems[selected].enabled = false;
        }
    }
}
