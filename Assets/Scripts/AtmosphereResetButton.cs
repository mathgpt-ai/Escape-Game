using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AtmosphereResetButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] gazTexts; // les 3 écrans à côté des valves
    [SerializeField] private Animator animator; // facultatif si tu veux une anim
    [SerializeField] private int indexEnigme = 0; // Index de l'énigme dans GameManager
    [SerializeField] private string scenePrincipale = "MapPrincipale"; // à changer selon ton nom de scène

    public void Interact()
    {
        if (animator != null)
            animator.SetTrigger("Pressed"); // déclenche une anim (facultatif mais cool)

        if (ToutesLesValeursSontVertes())
        {
            Debug.Log("Énigme réussie ! Retour à la map principale...");
            //GameManager.Instance.MarquerEnigmeComplete(indexEnigme);
            SceneManager.LoadScene(scenePrincipale);
        }
        else
        {
            Debug.Log("Valeurs incorrectes ! Corrige les gaz avant de réinitialiser.");
        }
    }

    private bool ToutesLesValeursSontVertes()
    {
        foreach (TextMeshProUGUI text in gazTexts)
        {
            if (text.color != Color.green)
                return false;
        }
        return true;
    }
}
