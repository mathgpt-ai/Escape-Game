using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        
        collision.gameObject.transform.position=GameObject.Find("SpawnPoint").transform.position;
        
    }

}
