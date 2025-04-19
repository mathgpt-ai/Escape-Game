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
    private Color selectedColor; // Couleur du slot s�lectionn�
    [SerializeField]
    private Color slotColor; // Couleur des autres slots

    private int selected = 0; // Slot actuellement s�lectionn�
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
        // V�rifie si le joueur appuie sur une des touches pour changer de slot
        for (int i = 0; i < Ikeys.Length; i++)
        {
            if (Input.GetKeyDown(Ikeys[i])) // Change seulement � l�appui, �vite le spam
            {
                selected = i;
            }
        }

        // Met � jour les couleurs des slots pour montrer lequel est s�lectionn�
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].color = (i == selected) ? selectedColor : slotColor;
        }
    }

    public void AddItem(Sprite newItem)
    {
        // V�rifie si l'objet est d�j� dans l'inventaire
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].enabled && inventoryItems[i].sprite == newItem)
            {
                // Si l'objet est d�j� dans l'inventaire, on arr�te l'ajout
                Debug.Log("Cet objet est d�j� dans l'inventaire.");
                return; // Retourne imm�diatement pour �viter d'ajouter � nouveau cet item
            }
        }



        if (!inventoryItems[selected].enabled) // V�rifie si le slot est vide
        {
            inventoryItems[selected].sprite = newItem; // Assigne l�image au slot
            inventoryItems[selected].enabled = true;   // Active l�image pour l�afficher
            return; // Arr�te apr�s avoir ajout� l'item
        }


        // Si aucun slot vide n'est trouv�, on peut afficher un message ou g�rer la situation
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
