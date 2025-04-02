using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipePuzzlePortal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int �nigmeTuyeaux; 

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Interact()
    {
        SceneManager.LoadScene(�nigmeTuyeaux, LoadSceneMode.Single);
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
