using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggerZone : MonoBehaviour
{
    [SerializeField] private TaskObject taskObject; // L�objet de qu�te correspondant � cette t�che
    [SerializeField] private ChecklistManager checklistManager; // Le gestionnaire de qu�te

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ou ton tag de joueur
        {
            if (checklistManager != null && taskObject != null)
            {
                //checklistManager.ForcerCompl�tion(taskObject);
                gameObject.SetActive(false); // d�sactive le trigger apr�s usage
            }
        }
    }
}
