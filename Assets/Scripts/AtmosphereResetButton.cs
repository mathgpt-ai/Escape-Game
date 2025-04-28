using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AtmosphereResetButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TextMeshProUGUI[] gazTexts; // les écrans à côté des valves
    [SerializeField]
    private int indexEnigme = 0; // Index de l'énigme dans GameManager
    [SerializeField]
    private Canvas canvas;

    public void Interact()
    {
        transform.localPosition += new Vector3(0.1f, 0f, 0f); // Pousse le bouton vers l’intérieur (axe X)

        if (ValeursSontVertes())
        {
            Debug.Log("Les valeurs sont bonnes !");
            GameManager.MarquerEnigmeComplete(indexEnigme);
            FindObjectOfType<OxygenSystem>().HandlePuzzleTuyauxCompleted();
            Debug.Log("Énigme réussie !");
        }
        else
        {
            Debug.Log("Valeurs incorrectes !");
        }
    }

    private bool ValeursSontVertes()
    {
        foreach (TextMeshProUGUI text in gazTexts)
        {
            if (text.color != Color.green)
                return false;
        }
        return true;
    }
    public Canvas GetCanvas()
    {
        return canvas;
    }
}

