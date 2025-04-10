using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChecklistManager : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI[] enigmeTexts;


    private void Start()
    {
        for (int i = 0; i < enigmeTexts.Length; i++)
        {
            if(PlayerPrefs.GetInt("Enigme_" + i, 0)== 1)
            {
                MarquerEnigeDone(i);
            }
        }
    }
    private void MarquerEnigeDone(int i)
    {
        if(i >= 0 && i < enigmeTexts.Length)
        {
            enigmeTexts[i].text = "<s>" + enigmeTexts[i].text + "<s>"; //raye
            enigmeTexts[i].color = Color.green;
        }
        
        //sauvegarder tache comme faite
        PlayerPrefs.SetInt("Enigme_" + i, 1);
        PlayerPrefs.Save();
    }

    //reinitialiser la checklist
    private void ResetChecklist()
    {
        for(int i = 0;i < enigmeTexts.Length;i++)
        {
            PlayerPrefs.DeleteKey("Enigme_" + i);
        }
        PlayerPrefs.Save();

    }

}
