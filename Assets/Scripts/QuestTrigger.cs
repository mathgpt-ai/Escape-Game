using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestObject questObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (questManager != null && questObject != null)
            {
                questManager.ForcerComplétion(questObject);
            }

            gameObject.SetActive(false);
        }
    }
}
