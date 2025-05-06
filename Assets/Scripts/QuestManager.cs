using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestStep
    {
        public string description;
        public QuestObject targetObject;
    }

    public QuestStep[] questSteps;
    public Text uiText; // UI Text � afficher pour chaque t�che
    public VideoPlayer videoPlayer; // Vid�o � jouer � la fin

    private int currentStepIndex = 0;

    private void Start()
    {
        ShowCurrentStep();
    }

    private void Update()
    {
        // Appuyer sur fl�che du haut pour passer manuellement � la prochaine t�che
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PasserALaProchaineTache();
        }
    }

    public void CompleteStep(QuestObject obj)
    {
        if (currentStepIndex < questSteps.Length && questSteps[currentStepIndex].targetObject == obj)
        {
            obj.Deactivate(); // Optionnel : d�sactive l'objet interact�
            currentStepIndex++;
            ShowCurrentStep();
        }
    }

    public void ForcerCompl�tion(QuestObject obj)
    {
        CompleteStep(obj);
    }

    public void PasserALaProchaineTache()
    {
        if (currentStepIndex < questSteps.Length)
        {
            currentStepIndex++;
            ShowCurrentStep();
        }
    }

    void ShowCurrentStep()
    {
        if (currentStepIndex < questSteps.Length)
        {
            uiText.text = questSteps[currentStepIndex].description;
        }
        else
        {
            uiText.text = "Toutes les t�ches sont compl�t�es !";

            if (videoPlayer != null)
            {
                //videoPlayer.gameObject.SetActive(true);
                //videoPlayer.Play();
            }
        }
    }
}
