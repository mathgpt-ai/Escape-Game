using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParent : MonoBehaviour
{
    public static TempParent Instance { get; private set; }

    [SerializeField] private Transform player; 
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 2f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Update()
    {
        if (player != null)
        {
            
            transform.position = player.position + player.forward * offset.z + player.up * offset.y;
            transform.rotation = Quaternion.LookRotation(player.forward);
        }
    }
}

