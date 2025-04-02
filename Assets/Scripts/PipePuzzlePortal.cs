using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipePuzzlePortal : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int ÉnigmeTuyeaux; 

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Interact()
    {
        SceneManager.LoadScene(ÉnigmeTuyeaux, LoadSceneMode.Single);
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
