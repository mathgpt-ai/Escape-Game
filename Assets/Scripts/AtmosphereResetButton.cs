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
        if (ValeursSontVertes())
        {
            Debug.Log("Les valeurs sont bonnes !");
            GameManager.MarquerEnigmeComplete(indexEnigme);
            //oxygen se reset au max 
            if (OxygenBar.Instance != null)
                OxygenBar.Instance.SetOxygen(OxygenBar.Instance.GetMaxOxygen());

            if (HealthBar.Instance != null)
                HealthBar.Instance.SetHealth(HealthBar.Instance.GetMaxHealth());

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

