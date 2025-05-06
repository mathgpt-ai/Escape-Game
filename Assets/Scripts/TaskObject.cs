using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    public ChecklistManager ChecklistManager;

    // Exemple : interaction avec souris
    private void OnMouseDown()
    {
        ChecklistManager.CompleteStep(this);
    }

    
}
