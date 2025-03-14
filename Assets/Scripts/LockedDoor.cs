using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private Canvas canvas;
    public bool isLock = true;
    private Animator Animator;
    private int dragonsRemaining = 3; // 🔹 Nombre total de dragons
    [SerializeField] private AudioClip doorUnlocked;

    void Start()
    {
        Animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        if (!isLock)
            canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLock)
            Animator.SetTrigger("open");
    }

    // 🔹 Appelé quand un dragon meurt
    public void DragonDefeated()
    {
        dragonsRemaining--;

        if (dragonsRemaining <= 0)
        {
            UnlockDoor();
        }
    }

    // 🔹 Déverrouille la porte
    private void UnlockDoor()
    {
        isLock = false;
        AudioSource.PlayClipAtPoint(doorUnlocked,Camera.main.transform.position,5f);
        
        
    }
}
