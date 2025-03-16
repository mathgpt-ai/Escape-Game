using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragons : MonoBehaviour,IInteractable
{
    [SerializeField] private AudioClip clip;

    private LockedDoor lockedDoor;
    Canvas canvas;
    

    private void Start()
    {
        lockedDoor = FindObjectOfType<LockedDoor>(); // 🔹 Trouve la porte
        canvas = GetComponentInChildren<Canvas>();
    }
    public void Interact()
    {
        AudioSource.PlayClipAtPoint(clip,transform.position,2f);
        Destroy(this.gameObject);
    }
    
    private void OnDestroy()
    {
        if (lockedDoor != null)
        {
            lockedDoor.DragonDefeated();
        }
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
