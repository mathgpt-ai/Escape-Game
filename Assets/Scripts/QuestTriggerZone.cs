using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggerZone : MonoBehaviour
{
    [SerializeField] private TaskObject taskObject; // L’objet de quête correspondant à cette tâche
    [SerializeField] private ChecklistManager checklistManager; // Le gestionnaire de quête

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ou ton tag de joueur
        {
            if (checklistManager != null && taskObject != null)
            {
                //checklistManager.ForcerComplétion(taskObject);
                gameObject.SetActive(false); // désactive le trigger après usage
            }
        }
    }
}
