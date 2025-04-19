using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3Trigger : MonoBehaviour
{
    public string sceneName; // Set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {   
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}
