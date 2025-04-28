using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AtmosphereResetButton : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TextMeshProUGUI[] gazTexts; // les �crans � c�t� des valves
    [SerializeField]
    private int indexEnigme = 0; // Index de l'�nigme dans GameManager
    [SerializeField]
    private Canvas canvas;

    public void Interact()
    {
        transform.localPosition += new Vector3(0.1f, 0f, 0f); // Pousse le bouton vers l�int�rieur (axe X)

        if (ValeursSontVertes())
        {
            Debug.Log("Les valeurs sont bonnes !");
            GameManager.MarquerEnigmeComplete(indexEnigme);
            FindObjectOfType<OxygenSystem>().HandlePuzzleTuyauxCompleted();
            Debug.Log("�nigme r�ussie !");
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

