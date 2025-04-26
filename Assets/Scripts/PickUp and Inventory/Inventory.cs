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
            if (Input.GetKeyDown(Ikeys[i])) // Change seulement � l'appui, �vite le spam
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

    // Fonction pour v�rifier le contenu du slot s�lectionn�
    public Sprite GetCurrentItem()
    {
        if (inventoryItems[selected].enabled)
        {
            return inventoryItems[selected].sprite;
        }
        return null; // Retourne null si le slot est vide
    }

    // V�rifie si un slot sp�cifique est occup�
    public bool HasItemInSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < inventoryItems.Length)
        {
            return inventoryItems[slotIndex].enabled;
        }
        return false;
    }

    // V�rifie si le slot actuel est occup�
    public bool HasItemInCurrentSlot()
    {
        return inventoryItems[selected].enabled;
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

        // D'abord, essayez d'ajouter au slot s�lectionn�
        if (!inventoryItems[selected].enabled)
        {
            inventoryItems[selected].sprite = newItem;
            inventoryItems[selected].enabled = true;
            return;
        }

        // Si le slot s�lectionn� est plein, chercher un autre slot vide
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (!inventoryItems[i].enabled)
            {
                inventoryItems[i].sprite = newItem;
                inventoryItems[i].enabled = true;
                Debug.Log("Item ajout� au slot " + i);
                return;
            }
        }

        // Si on arrive ici, c'est que tous les slots sont pleins
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