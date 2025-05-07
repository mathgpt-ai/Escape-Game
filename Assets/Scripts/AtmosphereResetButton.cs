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
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestObject questObject;

    public void Interact()
    {
        if (ValeursSontVertes())
        {
            if (questManager != null && questObject != null)
            {
                questManager.ForcerComplétion(questObject);
            }
            Debug.Log("Les valeurs sont bonnes !");
            GameManager.MarquerEnigmeComplete(indexEnigme);
            //oxygen se reset au max 
            if (OxygenBar.Instance != null)
                OxygenBar.Instance.SetOxygen(OxygenBar.Instance.GetMaxOxygen());

            if (HealthBar.Instance != null)
                HealthBar.Instance.SetHealth(HealthBar.Instance.GetMaxHealth());

            //Désactive complètement le système d’oxygène
            GameObject oxygenSystemObj = GameObject.Find("OxygenSystem");
            if (oxygenSystemObj != null)
                oxygenSystemObj.SetActive(false);

            Debug.Log("Le systeme d'atmosphere est réparé !");
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

