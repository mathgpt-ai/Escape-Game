using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Teleport the colliding object to the spawn point
        collision.gameObject.transform.position = GameObject.Find("SpawnPoint").transform.position;

        // Find the specific GameObject by name
        GameObject spawner = GameObject.Find("rickorme");

        // Activate the object if found
        if (spawner != null)
        {
            spawner.SetActive(true);
        }
        else
            Console.Write("yo");

            // Find all GameObjects with the "delete" tag and destroy them
            GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("delete");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }
}