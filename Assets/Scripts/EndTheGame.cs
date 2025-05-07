using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTheGame : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int Finale;
    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Interact()
    {
        SceneManager.LoadScene(Finale, LoadSceneMode.Single);
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
