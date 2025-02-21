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
    }
    public void SelectedItem()
    {
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

    public void AddItem()
    {
       
    }

    public void DropItem()
    {
        
    }
}
