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
    public Text uiText; // UI Text à afficher pour chaque tâche
    public VideoPlayer videoPlayer; // Vidéo à jouer à la fin

    private int currentStepIndex = 0;

    private void Start()
    {
        ShowCurrentStep();
    }

    private void Update()
    {
        // Appuyer sur flèche du haut pour passer manuellement à la prochaine tâche
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PasserALaProchaineTache();
        }
    }

    public void CompleteStep(QuestObject obj)
    {
        if (currentStepIndex < questSteps.Length && questSteps[currentStepIndex].targetObject == obj)
        {
            obj.Deactivate(); // Optionnel : désactive l'objet interacté
            currentStepIndex++;
            ShowCurrentStep();
        }
    }

    public void ForcerComplétion(QuestObject obj)
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
            uiText.text = "Toutes les tâches sont complétées !";

            if (videoPlayer != null)
            {
                //videoPlayer.gameObject.SetActive(true);
                //videoPlayer.Play();
            }
        }
    }
}
