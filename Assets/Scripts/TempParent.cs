using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParent : MonoBehaviour
{
    public static TempParent Instance { get; private set; }

    [SerializeField] private Transform player; // Assigne ici le joueur dans l'inspecteur
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 2f); // Position relative au joueur

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Assure qu'il n'y a qu'une seule instance
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // Met à jour la position pour rester devant le joueur
            transform.position = player.position + player.forward * offset.z + player.up * offset.y;
            transform.rotation = Quaternion.LookRotation(player.forward);
        }
    }
}
//public class TempParent : MonoBehaviour
//{
//    public static TempParent Instance { get; private set; }

//    private void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(this);
//    }
//}
