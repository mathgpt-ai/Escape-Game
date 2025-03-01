using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragons : MonoBehaviour,IInteractable
{
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    private LockedDoor lockedDoor;
    

    private void Start()
    {
        lockedDoor = FindObjectOfType<LockedDoor>(); // 🔹 Trouve la porte
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

    public string GetText()
    {
        throw new System.NotImplementedException();
    }
}
