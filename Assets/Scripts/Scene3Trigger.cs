using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3Trigger : MonoBehaviour
{
    public string sceneName;
    private bool isActive = false;
    public bool IsActive
    {
        get { return isActive; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isActive)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                isActive = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            isActive = false;
        }


    }
}
