using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour,IInteractable
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
            inventory.AddItem(sp); // Example function call
            print("jeaddSprite");
        }
    }

    // Start is called before the first frame update
   
    

    void Start()
    {
        inventory = FindObjectOfType<Inventory>(); // Find the Inventory script in the scene

        if (inventory == null)
        {
            Debug.LogError("Inventory script not found!");
        }
    }



}
