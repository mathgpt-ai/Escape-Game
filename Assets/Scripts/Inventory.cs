using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Image[] inventorySlots;
    [SerializeField]
    private Image[] inventoryItems;

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color slotColor;

    private int selected = 0;

    private KeyCode[] Ikeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    private void Update()
    {
        for (int i = 0; i < Ikeys.Length; i++)
        {
            if (Input.GetKey(Ikeys[i]))
            {
                selected = i;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i == selected)
            {
                inventorySlots[i].color = selectedColor;
            }
            else
            {
                inventorySlots[i].color = slotColor;
            }
        }

    }
    public bool AddItem(Sprite newItem)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].sprite == null) // Check for an empty slot
            {
                inventoryItems[i].sprite = newItem; // Assign the sprite
                inventoryItems[i].enabled = true;   // Make sure it's visible
                return true; // Item added successfully
            }
        }
        return false; // Inventory is full
    }
}



