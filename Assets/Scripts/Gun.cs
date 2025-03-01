using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour,IInteractable
{
    [SerializeField] private Sprite sp;
    Canvas can;
    Inventory inventory;
    public string GetText()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        inventory.AddItem(sp);
    }

    // Start is called before the first frame update
    void Start()
    {
        can = FindObjectOfType<Canvas>();
        if(can!=null)
        inventory = can.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
